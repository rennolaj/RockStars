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
using RockStars.API.ResourceParameters;

namespace RockStars.API.Controllers
{
    [ApiController]
    [ResponseCache(CacheProfileName = "Default30")]
    [Route("api/songcollections")]
    public class SongCollectionsController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ISongLibraryRepository songLibraryRepository;
        private readonly IMapper mapper;

        public SongCollectionsController(ISongLibraryRepository songLibraryRepository,
            ILogger<SongCollectionsController> logger,
            IMapper mapper)
        {
            this.logger = logger;
            this.songLibraryRepository = songLibraryRepository ??
                throw new ArgumentNullException(nameof(songLibraryRepository));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<SongDto>> GetSongCollection(
            [FromQuery] SongsResourceParameters songssResourceParameters)
        {
            var songsFromRepo = songLibraryRepository.GetSongs(songssResourceParameters);
            var songsToReturn = mapper.Map<IEnumerable<SongDto>>(songsFromRepo);
            return Ok(songsToReturn);
        }

        [HttpGet("({ids})", Name = "GetSongCollection")]
        public IActionResult GetSongCollection(
        [FromRoute]
        [ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<int> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var songEntities = songLibraryRepository.GetSongs(ids);
            if (ids.Count() != songEntities.Count())
            {
                return NotFound();
            }

            var songsToReturn = mapper.Map<IEnumerable<SongDto>>(songEntities);
            return Ok(songsToReturn);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<SongDto>>> CreateSongCollection(
                IEnumerable<SongForCreationDto> songCollection)
        {
            var songEntities = mapper.Map<IEnumerable<Entities.Song>>(songCollection);

            await AddSongsAsync(songEntities);
            await songLibraryRepository.SaveAsync();

            var songCollectionToReturn = mapper.Map<IEnumerable<SongDto>>(songEntities);
            var idsAsString = string.Join(",", songCollectionToReturn.Select(a => a.Id));

            return CreatedAtRoute("GetSongCollection",
             new { ids = idsAsString },
             songCollectionToReturn);
        }

        private int CreateArtistId()
        {
            int idCandidate;
            do
            {
                idCandidate = Math.Abs(Guid.NewGuid().GetHashCode());
            }
            while (songLibraryRepository.ArtistExists(idCandidate));

            return idCandidate;
        }

        private async Task AddSongsAsync(IEnumerable<Entities.Song> songEntities)
        {
            try
            {
                foreach (var song in songEntities)
                {
                    var artistId = songLibraryRepository.GetArtistId(song.Artist);
                    if (artistId == null)
                    {
                        var artist = new Entities.Artist()
                        {
                            Id = CreateArtistId(),
                            Name = song.Artist
                        };

                        await songLibraryRepository.AddArtistAsync(artist);
                        artistId = artist.Id;
                    }
                    
                    if (!songLibraryRepository.SongExists(song.Id))
                    {                   
                        await songLibraryRepository.AddSongAsync((int)artistId, song);
                        
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
