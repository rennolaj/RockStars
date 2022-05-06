using AutoMapper;
using RockStars.API.Models;
using RockStars.API.ResourceParameters;
using Microsoft.AspNetCore.Mvc;
using SongLibrary.API.Services;
using System;
using System.Collections.Generic;

namespace RockStars.API.Controllers
{
    [ApiController]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    [Route("api/artists")]
    public class ArtistsController : ControllerBase
    {
        private readonly ISongLibraryRepository songLibraryRepository;
        private readonly IMapper mapper;

        public ArtistsController(ISongLibraryRepository songLibraryRepository,
            IMapper mapper)
        {
            this.songLibraryRepository = songLibraryRepository ??
                throw new ArgumentNullException(nameof(songLibraryRepository));
            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [HttpHead]
        public ActionResult<IEnumerable<ArtistDto>> GetArtists(
            [FromQuery] ArtistsResourceParameters artistsResourceParameters)
        {
            var artistsFromRepo = songLibraryRepository.GetArtists(artistsResourceParameters);
            return Ok(mapper.Map<IEnumerable<ArtistDto>>(artistsFromRepo));
        }

        [HttpGet("{artistId}", Name = "GetArtist")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult GetArtist(int artistId)
        {
            var artistFromRepo = songLibraryRepository.GetArtist(artistId);
            if (artistFromRepo == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<ArtistDto>(artistFromRepo));
        }

        [HttpPost]
        public ActionResult<ArtistDto> CreateArtist(ArtistForCreationDto artist)
        {
            var artistEntity = mapper.Map<Entities.Artist>(artist);

            if (!songLibraryRepository.ArtistExists(artist.Id))
            {
                songLibraryRepository.AddArtist(artistEntity);
                songLibraryRepository.Save();
            }

            var artistToReturn = mapper.Map<ArtistDto>(artistEntity);
            return CreatedAtRoute("GetArtist",
                new { artistId = artistToReturn.Id },
                artistToReturn);
        }

        [HttpOptions]
        public IActionResult GetArtistsOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpDelete("{artistId}")]
        public ActionResult DeleteArtist(int artistId)
        {
            var artistFromRepo = songLibraryRepository.GetArtist(artistId);
            if (artistFromRepo == null)
            {
                return NotFound();
            }

            songLibraryRepository.DeleteArtist(artistFromRepo);
            songLibraryRepository.Save();

            return NoContent();
        }
    }
}
