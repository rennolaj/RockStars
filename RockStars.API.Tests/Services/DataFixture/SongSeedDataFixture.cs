using Microsoft.EntityFrameworkCore;
using RockStars.API.DbContexts;
using RockStars.API.Entities;
using System;
using System.Collections.Generic;

namespace RockStars.API.Tests.Services.DataFixture
{
    public class SongSeedDataFixture : IDisposable
    {
        public SongLibraryContext SongContext { get; private set; } = new SongLibraryContext(new DbContextOptionsBuilder<SongLibraryContext>().UseInMemoryDatabase(databaseName: "TestDb").Options);

        public SongSeedDataFixture()
        {
            var artist1 = new Artist { Id = 1, Name = "Artist 1" };
            var artist2 = new Artist { Id = 2, Name = "Artist 2" };
            var artist3 = new Artist { Id = 3, Name = "Artist 3" };

            var artists = new List<Artist>()
            {
                artist1,
                artist2,
                artist3
            };

            var songs = new List<Song>()
            {
                new Song { Id = 1, Name = "Song 1", Year = 2018, Genre = "Rock" , ArtistId = artist1.Id, Artist = artist1.Name, Bpm = 250, Duration = 45646, SpotifyId = "ahfhHDAKSDHAK", Album="Album 1", ShortName ="song1" },
                new Song { Id = 2, Name = "Song 2", Year = 2018, Genre = "Metal", ArtistId = artist1.Id, Artist = artist1.Name, Bpm = 250, Duration = 45646, SpotifyId = "bhfhHDAKSDHAK", Album="Album 1", ShortName ="song2" },
                new Song { Id = 3, Name = "Song 3", Year = 2019, Genre = "Metal-Rock", ArtistId = artist2.Id, Artist = artist2.Name, Bpm = 250, Duration = 45646, SpotifyId = "chfhHDAKSDHAK", Album="Album 1", ShortName ="song3" },
                new Song { Id = 4, Name = "Song 4", Year = 2021, Genre = "Pop-Rock", ArtistId = artist2.Id, Artist = artist2.Name, Bpm = 250, Duration = 45646, SpotifyId = "dhfhHDAKSDHAK", Album="Album 1", ShortName ="song4" },
                new Song { Id = 5, Name = "Song 5", Year = 2018, Genre = "Pop",  ArtistId = artist3.Id, Artist = artist3.Name, Bpm = 250, Duration = 45646, SpotifyId = "ehfhHDAKSDHAK", Album="Album 1", ShortName ="song5" },
                new Song { Id = 6, Name = "Song 6", Year = 2022, Genre = "Hip-Hop",ArtistId = artist3.Id, Artist = artist3.Name, Bpm = 250, Duration = 45646, SpotifyId = "fhfhHDAKSDHAK", Album="Album 1", ShortName ="song6" }
            };

            SongContext.Artists.AddRange(artists);
            SongContext.Songs.AddRange(songs);

            SongContext.SaveChanges();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            SongContext.Dispose();
        }
    }
}
