using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using RockStars.API.Controllers;
using RockStars.API.Entities;
using RockStars.API.Models;
using SongLibrary.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity;

namespace RockStars.API.Tests.Controllers
{
    public class ArtistCollectionControllerTests
    {
        private UnityContainer container;
        private ArtistCollectionsController artistCollectionController;

        private Mock<ISongLibraryRepository> songLibraryRepositoryMock;
        private Mock<IMapper> mapperMock;
        private Mock<ILogger<ArtistCollectionsController>> loggerMock;

        [SetUp]
        public void InitializeController()
        {
            songLibraryRepositoryMock = new Mock<ISongLibraryRepository>();
            mapperMock = new Mock<IMapper>();
            loggerMock = new Mock<ILogger<ArtistCollectionsController>>();

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
            artistCollectionController = new ArtistCollectionsController(songLibraryRepositoryMock.Object, loggerMock.Object, mapperMock.Object);

            container = new UnityContainer();
            container.RegisterInstance(artistCollectionController);
        }

        [Test]
        public void ArtistCollectionController_GetArtists_RepositoryCallsMadeSuccesfully()
        {
            // Act
            var controller = container.Resolve<ArtistCollectionsController>();
            controller.GetArtistCollection(new List<int> { 1, 2 });

            songLibraryRepositoryMock.Verify(s => s.GetArtists(It.IsAny<List<int>>()), Times.Once);
        }

        [Test]
        public async Task ArtistCollectionController_CreateArtistsAsync_RepositoryCallsMadeSuccesfully()
        {
            // Arrange
            songLibraryRepositoryMock.Setup(s => s.ArtistExists(It.IsAny<int>())).Returns(false);
            mapperMock.Setup(m => m.Map<IEnumerable<Artist>>(It.IsAny<IEnumerable<ArtistForCreationDto>>())).Returns(new List<Artist>
                {
                    new Artist
                    {
                        Id = 1,
                        Name = "Artist 1",
                    },
                     new Artist
                    {
                        Id = 2,
                        Name = "Artist 2",
                    }
                }
            );

            // Act
            var controller = container.Resolve<ArtistCollectionsController>();
            await controller.CreateArtistCollection(new List<ArtistForCreationDto>
            {
                 new ArtistForCreationDto
                {
                    Id = 1,
                    Name = "Artist 1",
                },
                 new ArtistForCreationDto
                {
                    Id = 2,
                    Name = "Artist 2",
                }
            });

            songLibraryRepositoryMock.Verify(s => s.AddArtistAsync(It.IsAny<Artist>()), Times.Exactly(2));
        }
    }
}
