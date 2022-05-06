using AutoMapper;
using RockStars.API.Helpers;
using RockStars.API.Models;
using Microsoft.AspNetCore.Mvc;
using SongLibrary.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace RockStars.API.Controllers
{
    [ApiController]
    [ResponseCache(CacheProfileName = "Default30")]
    [Route("api/artistcollections")]
    public class ArtistCollectionsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ISongLibraryRepository songLibraryRepository;
        private readonly IMapper mapper;

        public ArtistCollectionsController(ISongLibraryRepository songLibraryRepository,
            ILogger<ArtistCollectionsController> logger,
            IMapper mapper)
        {
            this.logger = logger;

            this.songLibraryRepository = songLibraryRepository ??
                throw new ArgumentNullException(nameof(songLibraryRepository));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("({ids})", Name = "GetArtistCollection")]
        public IActionResult GetArtistCollection(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var artistEntities = songLibraryRepository.GetArtists(ids);
            if (ids.Count() != artistEntities.Count())
            {
                return NotFound();
            }

            var artistsToReturn = mapper.Map<IEnumerable<ArtistDto>>(artistEntities);
            return Ok(artistsToReturn);
        }


        [HttpPost]
        public async Task<ActionResult<IEnumerable<ArtistDto>>> CreateArtistCollection(
            IEnumerable<ArtistForCreationDto> artistCollection)
        {
            var artistEntities = mapper.Map<IEnumerable<Entities.Artist>>(artistCollection);
            await AddArtistsAsync(artistEntities);

            await songLibraryRepository.SaveAsync();

            var artistCollectionToReturn = mapper.Map<IEnumerable<ArtistDto>>(artistEntities);
            var idsAsString = string.Join(",", artistCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetArtistCollection",
             new { ids = idsAsString },
             artistCollectionToReturn);
        }

        private async Task AddArtistsAsync(IEnumerable<Entities.Artist> artistEntities) 
        {
            try
            {
                foreach (var artist in artistEntities)
                {
                    if (!songLibraryRepository.ArtistExists(artist.Id))
                    {
                        await songLibraryRepository.AddArtistAsync(artist);
                    }
                }
            }
            catch (ArgumentException exception) 
            {
                logger.LogError(exception.Message); 
            }
        }
    }
}
