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
    public class ArtistsControllerTests
    {
        private UnityContainer container;
        private ArtistsController artistsController;

        private Mock<ISongLibraryRepository> songLibraryRepositoryMock;
        private Mock<IMapper> mapperMock;

        [SetUp]
        public void InitializeController()
        {
            songLibraryRepositoryMock = new Mock<ISongLibraryRepository>();
            mapperMock = new Mock<IMapper>();

            songLibraryRepositoryMock.Setup(s => s.GetArtists()).Returns(
                new List<Artist>()
                {
                    new Artist
                    {
                        Id = 1,
                        Name = "Artist 1",
                    }
                }
            );

            songLibraryRepositoryMock.Setup(s => s.ArtistExists(It.IsAny<int>())).Returns(true);
            artistsController = new ArtistsController(songLibraryRepositoryMock.Object, mapperMock.Object);

            container = new UnityContainer();
            container.RegisterInstance(artistsController);
        }

        [Test]
        public void ArtistsController_GetArtists_RepositoryCallsMadeSuccesfully()
        {
            // Act
            var controller = container.Resolve<ArtistsController>();
            controller.GetArtists(
                new ResourceParameters.ArtistsResourceParameters()
                {
                    Genre = "Rock"
                }
            );

            songLibraryRepositoryMock.Verify(s => s.GetArtists(It.IsAny<ResourceParameters.ArtistsResourceParameters>()), Times.Once);
        }

        [Test]
        public void ArtistsController_GetArtistPerId_RepositoryCallsMadeSuccesfully()
        {
            // Arrange
            songLibraryRepositoryMock.Setup(s => s.GetArtist(It.IsAny<int>())).Returns(
                new Artist
                {
                    Id = 1,
                    Name = "Artist 1",
                }
            );

            // Act
            var controller = container.Resolve<ArtistsController>();
            controller.GetArtist(1);

            songLibraryRepositoryMock.Verify(s => s.GetArtist(It.IsAny<int>()), Times.Once);
        }
    }
}
