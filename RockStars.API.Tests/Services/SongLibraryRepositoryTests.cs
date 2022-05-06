using NUnit.Framework;
using RockStars.API.Entities;
using RockStars.API.ResourceParameters;
using RockStars.API.Services;
using RockStars.API.Tests.Services.DataFixture;
using SongLibrary.API.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity;

namespace RockStars.API.Tests
{
    public class SongLibraryRepositoryTests
    {
        private readonly SongSeedDataFixture songSeedDataFixture = new SongSeedDataFixture();
        private UnityContainer container;
        
        private ISongLibraryRepository songLibraryRepository;

        [SetUp]
        public void InitializeRepository()
        {
            songLibraryRepository = new SongLibraryRepository(songSeedDataFixture.SongContext);

            container = new UnityContainer();
            container.RegisterInstance(songLibraryRepository);
        }

        [Test]
        public void SongLibraryRepository_GetArtistsFromRepository_ArtistsNamesAreEqual()
        {
            // Arrange
            var expectedArtist1 = new Artist { Id = 1, Name = "Artist 1" };
            var expectedArtist2 = new Artist { Id = 2, Name = "Artist 2" };
            var expectedArtist3 = new Artist { Id = 3, Name = "Artist 3" };

            // Act
            var songRepository = container.Resolve<ISongLibraryRepository>();
            var artists = songRepository.GetArtists();

            Assert.AreEqual(3, artists.Count());
            Assert.AreEqual(expectedArtist1.Name, artists.ElementAt(0).Name);
            Assert.AreEqual(expectedArtist2.Name, artists.ElementAt(1).Name);
            Assert.AreEqual(expectedArtist3.Name, artists.ElementAt(2).Name);
        }

        [Test]
        public void SongLibraryRepository_AddArtist_ArtistAddedSuccesfully()
        {
            // Arrange
            var artist = new Artist { Id = 4, Name = "Artist 4" };

            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            songRepository.AddArtist(artist);
            songRepository.Save();

            Assert.AreEqual(artist, songRepository.GetArtist(artist.Id));

            songRepository.DeleteArtist(artist);
            songRepository.Save();
        }

        [Test]
        public async Task SongLibraryRepository_AddArtistAsync_ArtistAddedSuccesfully()
        {
            // Arrange
            var artist = new Artist { Id = 4, Name = "Artist 4" };

            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            await songRepository.AddArtistAsync(artist);
            await songRepository.SaveAsync();

            Assert.AreEqual(artist, songRepository.GetArtist(artist.Id));

            songRepository.DeleteArtist(artist);
            songRepository.Save();
        }

        [Test]
        public void SongLibraryRepository_GetArtistId_ArtistIdObtainedSuccesfully()
        {
            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            var artistId = songRepository.GetArtistId("Artist 1");
            
            Assert.AreEqual(1, artistId);
        }

        [Test]
        public void SongLibraryRepository_GetArtistId_ArtistIdIsNull()
        {
            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            var artistId = songRepository.GetArtistId("Artist 4");

            Assert.IsNull(artistId);
        }

        [Test]
        public void SongLibraryRepository_DeleteArtist_ArtistDeletedSuccesfully()
        {
            // Arrange
            var artist = new Artist { Id = 4, Name = "Artist 4" };

            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            songRepository.AddArtist(artist);
            songRepository.Save();

            Assert.AreEqual(artist, songRepository.GetArtist(artist.Id));

            songRepository.DeleteArtist(artist);
            songRepository.Save();

            Assert.IsFalse(songRepository.ArtistExists(artist.Id));
        }

        [Test]
        public void SongLibraryRepository_AddSongForArtist_SongAddedSuccesfully()
        {
            // Arrange
            var artist = new Artist { Id = 3, Name = "Artist 3" };
            var song = new Song
            {
                Id = 7,
                Name = "Song 7",
                Year = 2018,
                Genre = "Rock",
                Artist = artist.Name,
                Bpm = 250,
                Duration = 45646,
                SpotifyId = "3hfhHDAKSDHAK",
                Album = "Album 1",
                ShortName = "song7"
            };

            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            songRepository.AddSong(artist.Id, song);
            songRepository.Save();

            Assert.IsTrue(songRepository.SongExists(song.Id));

            songRepository.DeleteSong(songRepository.GetSong(artist.Id, song.Id));
            songRepository.Save();
        }

        [Test]
        public async Task SongLibraryRepository_AddSongAsyncForArtist_SongAddedSuccesfully()
        {
            // Arrange
            var artist = new Artist { Id = 3, Name = "Artist 3" };
            var song = new Song
            {
                Id = 7,
                Name = "Song 7",
                Year = 2018,
                Genre = "Rock",
                Artist = artist.Name,
                Bpm = 250,
                Duration = 45646,
                SpotifyId = "3hfhHDAKSDHAK",
                Album = "Album 1",
                ShortName = "song7"
            };

            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            await songRepository.AddSongAsync(artist.Id, song);
            await songRepository.SaveAsync();

            Assert.IsTrue(songRepository.SongExists(song.Id));

            songRepository.DeleteSong(songRepository.GetSong(artist.Id, song.Id));
            songRepository.Save();
        }

        [Test]
        public void SongLibraryRepository_DeleteSongForArtist_SongDeletedSuccesfully()
        {
            // Arrange
            var artist = new Artist { Id = 3, Name = "Artist 3" };
            var song = new Song
            {
                Id = 7,
                Name = "Song 7",
                Year = 2018,
                Genre = "Rock",
                Artist = artist.Name,
                Bpm = 250,
                Duration = 45646,
                SpotifyId = "3hfhHDAKSDHAK",
                Album = "Album 1",
                ShortName = "song7"
            };

            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            songRepository.AddSong(artist.Id, song);
            songRepository.Save();

            Assert.IsTrue(songRepository.SongExists(song.Id));
            songRepository.DeleteSong(songRepository.GetSong(artist.Id, song.Id));
            songRepository.Save();

            Assert.IsFalse(songRepository.SongExists(song.Id));
        }

        [Test]
        public void SongLibraryRepository_GetSongsPerArtist_SongsObtainedSuccesfully()
        {
            // Arrange
            var artist = new Artist { Id = 2, Name = "Artist 2" };
            
            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            var songs = songRepository.GetSongs(artist.Id);

            Assert.AreEqual(2, songs.Count());
            Assert.AreEqual("Song 3", songs.ElementAt(0).Name);
            Assert.AreEqual("Song 4", songs.ElementAt(1).Name);
            Assert.AreEqual(2019, songs.ElementAt(0).Year);
            Assert.AreEqual(2021, songs.ElementAt(1).Year);
            Assert.AreEqual("Metal-Rock", songs.ElementAt(0).Genre);
            Assert.AreEqual("Pop-Rock", songs.ElementAt(1).Genre);
            Assert.AreEqual(250, songs.ElementAt(0).Bpm);
            Assert.AreEqual(250, songs.ElementAt(1).Bpm);
            Assert.AreEqual(45646, songs.ElementAt(0).Duration);
            Assert.AreEqual(45646, songs.ElementAt(1).Duration);
            Assert.AreEqual("chfhHDAKSDHAK", songs.ElementAt(0).SpotifyId);
            Assert.AreEqual("dhfhHDAKSDHAK", songs.ElementAt(1).SpotifyId);
            Assert.AreEqual("Album 1", songs.ElementAt(0).Album);
            Assert.AreEqual("Album 1", songs.ElementAt(1).Album);
            Assert.AreEqual("song3", songs.ElementAt(0).ShortName);
            Assert.AreEqual("song4", songs.ElementAt(1).ShortName);
        }

        [Test]
        public void SongLibraryRepository_GetSongsPerId_SongsObtainedSuccesfully()
        {
            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            var songs = songRepository.GetSongs(new List<int>() { 3, 4 });

            Assert.AreEqual(2, songs.Count());
            Assert.AreEqual("Song 3", songs.ElementAt(0).Name);
            Assert.AreEqual("Song 4", songs.ElementAt(1).Name);
            Assert.AreEqual(2019, songs.ElementAt(0).Year);
            Assert.AreEqual(2021, songs.ElementAt(1).Year);
            Assert.AreEqual("Metal-Rock", songs.ElementAt(0).Genre);
            Assert.AreEqual("Pop-Rock", songs.ElementAt(1).Genre);
            Assert.AreEqual(250, songs.ElementAt(0).Bpm);
            Assert.AreEqual(250, songs.ElementAt(1).Bpm);
            Assert.AreEqual(45646, songs.ElementAt(0).Duration);
            Assert.AreEqual(45646, songs.ElementAt(1).Duration);
            Assert.AreEqual("chfhHDAKSDHAK", songs.ElementAt(0).SpotifyId);
            Assert.AreEqual("dhfhHDAKSDHAK", songs.ElementAt(1).SpotifyId);
            Assert.AreEqual("Album 1", songs.ElementAt(0).Album);
            Assert.AreEqual("Album 1", songs.ElementAt(1).Album);
            Assert.AreEqual("song3", songs.ElementAt(0).ShortName);
            Assert.AreEqual("song4", songs.ElementAt(1).ShortName);
        }

        [Test]
        public void SongLibraryRepository_GetSongsFromQueryWithOnlyGenre_SongsObtainedSuccesfully()
        {
            // Arrange
            var query = new SongsResourceParameters()
            {
                Genre = "Rock"
            };

            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            var songs = songRepository.GetSongs(query);

            Assert.AreEqual(3, songs.Count());
            Assert.IsTrue(songs.ElementAt(0).Genre.Contains(query.Genre));
            Assert.IsTrue(songs.ElementAt(1).Genre.Contains(query.Genre));
            Assert.IsTrue(songs.ElementAt(2).Genre.Contains(query.Genre));
        }

        [Test]
        public void SongLibraryRepository_GetSongsFromQueryWithOnlyYear_SongsObtainedSuccesfully()
        {
            // Arrange
            var query = new SongsResourceParameters()
            {
                Year = 2019
            };

            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            var songs = songRepository.GetSongs(query);

            Assert.AreEqual(1, songs.Count());
            Assert.AreEqual(2019, songs.ElementAt(0).Year);
        }

        [Test]
        public void SongLibraryRepository_GetArtistsFromQueryWithOnlyGenre_ArtistsObtainedSuccesfully()
        {
            // Arrange
            var query = new ArtistsResourceParameters()
            {
                Genre = "Rock"
            };

            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            var artists = songRepository.GetArtists(query);

            Assert.AreEqual(2, artists.Count());
            Assert.AreEqual("Artist 1", artists.ElementAt(0).Name);
            Assert.AreEqual("Artist 2", artists.ElementAt(1).Name);
        }

        [Test]
        public void SongLibraryRepository_GetArtistsPerId_ArtistsObtainedSuccesfully()
        {
            // Act & Assert
            var songRepository = container.Resolve<ISongLibraryRepository>();
            var artists = songRepository.GetArtists(new List<int>() { 2, 3 });

            Assert.AreEqual(2, artists.Count());
            Assert.AreEqual("Artist 2", artists.ElementAt(0).Name);
            Assert.AreEqual("Artist 3", artists.ElementAt(1).Name);
        }
    }
}