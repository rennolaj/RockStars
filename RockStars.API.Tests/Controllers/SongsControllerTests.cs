using AutoMapper;
using Moq;
using NUnit.Framework;
using RockStars.API.Controllers;
using RockStars.API.Entities;
using SongLibrary.API.Services;
using System.Collections.Generic;
using Unity;

namespace RockStars.API.Tests.Controllers
{
    public class SongsControllerTests
    {
        private UnityContainer container;
        private SongsController songsController;

        private Mock<ISongLibraryRepository> songLibraryRepositoryMock;
        private Mock<IMapper> mapperMock;

        [SetUp]
        public void InitializeController()
        {
            songLibraryRepositoryMock = new Mock<ISongLibraryRepository>();
            mapperMock = new Mock<IMapper>();

            songLibraryRepositoryMock.Setup(s => s.GetSongs(It.IsAny<int>())).Returns(
                new List<Song>()
                {
                    new Song
                    {
                        Id = 1,
                        Name = "Song 1",
                        Year = 2018, Genre = "Rock",
                        Artist = "Artist 1",
                        Bpm = 250, Duration = 45646,
                        SpotifyId = "ahfhHDAKSDHAK",
                        Album = "Album 1",
                        ShortName = "song1"
                    }
                }
            );

            songLibraryRepositoryMock.Setup(s => s.ArtistExists(It.IsAny<int>())).Returns(true);
            songsController = new SongsController(songLibraryRepositoryMock.Object, mapperMock.Object);

            container = new UnityContainer();
            container.RegisterInstance(songsController);
        }

        [Test]
        public void SongsController_GetSongsFromArtist_RepositoryCallsMadeSuccesfully()
        {
            // Act
            var controller = container.Resolve<SongsController>();
            controller.GetSongsForArtist(1);

            songLibraryRepositoryMock.Verify(s => s.GetSongs(1), Times.Once);
            songLibraryRepositoryMock.Verify(s => s.ArtistExists(1), Times.Once);
        }

        [Test]
        public void SongsController_GetSongFromArtist_RepositoryCallsMadeSuccesfully()
        {
            // Arrange
            songLibraryRepositoryMock.Setup(s => s.GetSong(It.IsAny<int>(), It.IsAny<int>())).Returns(
                new Song
                {
                    Id = 1,
                    Name = "Song 1",
                    Year = 2018,
                    Genre = "Rock",
                    Artist = "Artist 1",
                    Bpm = 250,
                    Duration = 45646,
                    SpotifyId = "ahfhHDAKSDHAK",
                    Album = "Album 1",
                    ShortName = "song1"
                }
            );

            // Act
            var controller = container.Resolve<SongsController>();
            controller.GetSongForArtist(1, 1);

            songLibraryRepositoryMock.Verify(s => s.GetSong(1, 1), Times.Once);
            songLibraryRepositoryMock.Verify(s => s.ArtistExists(1), Times.Once);
        }
    }
}
