using AutoMapper;
using RockStars.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SongLibrary.API.Services;
using System;
using System.Collections.Generic;

namespace RockStars.API.Controllers
{
    [ApiController]
    [ResponseCache(VaryByHeader = "User-Agent", Duration = 30)]
    [Route("api/artists/{artistId}/songs")]
    public class SongsController : ControllerBase
    {
        private readonly ISongLibraryRepository songLibraryRepository;
        private readonly IMapper mapper;

        public SongsController(ISongLibraryRepository songLibraryRepository,
            IMapper mapper)
        {
            this.songLibraryRepository = songLibraryRepository ??
                throw new ArgumentNullException(nameof(songLibraryRepository));

            this.mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<IEnumerable<SongDto>> GetSongsForArtist(int artistId)
        {
            if (!songLibraryRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var songsForArtistFromRepo = songLibraryRepository.GetSongs(artistId);
            return Ok(mapper.Map<IEnumerable<SongDto>>(songsForArtistFromRepo));
        }

        [HttpGet("{songId}", Name = "GetSongForArtist")]
        [ResponseCache(Duration = 10, Location = ResponseCacheLocation.Any, NoStore = false)]
        public ActionResult<SongDto> GetSongForArtist(int artistId, int songId)
        {
            if (!songLibraryRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var songForArtistFromRepo = songLibraryRepository.GetSong(artistId, songId);
            if (songForArtistFromRepo == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<SongDto>(songForArtistFromRepo));
        }

        [HttpPost]
        public ActionResult<SongDto> CreateSongForArtist(
            int artistId, SongForCreationDto song)
        {
            if (!songLibraryRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var songEntity = mapper.Map<Entities.Song>(song);

            AddSongToRepository(artistId, songEntity);
            
            var songToReturn = mapper.Map<SongDto>(songEntity);
            return CreatedAtRoute("GetSongForArtist",
                new { artistId = artistId, songId = songToReturn.Id },
                songToReturn);
        }

        [HttpPost]
        public ActionResult<SongDto> CreateSongForArtist(SongForCreationDto song)
        {
            var artistId = songLibraryRepository.GetArtistId(song.Name);
            if (artistId == null)
            {
                return NotFound();
            }

            var songEntity = mapper.Map<Entities.Song>(song);

            AddSongToRepository((int)artistId, songEntity);
            
            var songToReturn = mapper.Map<SongDto>(songEntity);
            return CreatedAtRoute("GetSongForArtist",
                new { artistId = artistId, songId = songToReturn.Id },
                songToReturn);
        }

        [HttpPut("{songId}")]
        public IActionResult UpdateSongForArtist(int artistId,
            int songId,
            SongForUpdateDto song)
        {
            if (!songLibraryRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var songForArtistFromRepo = songLibraryRepository.GetSong(artistId, songId);
            if (songForArtistFromRepo == null)
            {
                var songToAdd = mapper.Map<Entities.Song>(song);
                songToAdd.Id = songId;

                songLibraryRepository.AddSong(artistId, songToAdd);
                songLibraryRepository.Save();

                var songToReturn = mapper.Map<SongDto>(songToAdd);

                return CreatedAtRoute("GetSongForArtist",
                    new { artistId, songId = songToReturn.Id },
                    songToReturn);
            }

            mapper.Map(song, songForArtistFromRepo);
            songLibraryRepository.UpdateSong(songForArtistFromRepo);

            songLibraryRepository.Save();
            return NoContent();
        }

        [HttpPatch("{songId}")]
        public ActionResult PartiallyUpdateSongForArtist(int artistId,
            int songId,
            JsonPatchDocument<SongForUpdateDto> patchDocument)
        {
            if (!songLibraryRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var songForArtistFromRepo = songLibraryRepository.GetSong(artistId, songId);
            if (songForArtistFromRepo == null)
            {
                var songDto = new SongForUpdateDto();
                patchDocument.ApplyTo(songDto, ModelState);

                if (!TryValidateModel(songDto))
                {
                    return ValidationProblem(ModelState);
                }

                var songToAdd = mapper.Map<Entities.Song>(songDto);
                songToAdd.Id = songId;

                songLibraryRepository.AddSong(artistId, songToAdd);
                songLibraryRepository.Save();

                var songToReturn = mapper.Map<SongDto>(songToAdd);

                return CreatedAtRoute("GetSongForArtist",
                    new { artistId, songId = songToReturn.Id },
                    songToReturn);
            }

            var songToPatch = mapper.Map<SongForUpdateDto>(songForArtistFromRepo);
            patchDocument.ApplyTo(songToPatch, ModelState);

            if (!TryValidateModel(songToPatch))
            {
                return ValidationProblem(ModelState);
            }

            mapper.Map(songToPatch, songForArtistFromRepo);
            songLibraryRepository.UpdateSong(songForArtistFromRepo);
            songLibraryRepository.Save();

            return NoContent();
        }

        [HttpDelete("{songId}")]
        public ActionResult DeleteSongForArtist(int artistId, int songId)
        {
            if (!songLibraryRepository.ArtistExists(artistId))
            {
                return NotFound();
            }

            var songForArtistFromRepo = songLibraryRepository.GetSong(artistId, songId);
            if (songForArtistFromRepo == null)
            {
                return NotFound();
            }

            songLibraryRepository.DeleteSong(songForArtistFromRepo);
            songLibraryRepository.Save();

            return NoContent();
        }

        public override ActionResult ValidationProblem(
            [ActionResultObjectValue] ModelStateDictionary modelStateDictionary)
        {
            var options = HttpContext.RequestServices
                .GetRequiredService<IOptions<ApiBehaviorOptions>>();
            return (ActionResult)options.Value.InvalidModelStateResponseFactory(ControllerContext);
        }

        private void AddSongToRepository(int artistId, Entities.Song songEntity) 
        {
            if (!songLibraryRepository.SongExists(songEntity.Id))
            {
                songLibraryRepository.AddSong(artistId, songEntity);
                songLibraryRepository.Save();
            }
        }
    }
}
