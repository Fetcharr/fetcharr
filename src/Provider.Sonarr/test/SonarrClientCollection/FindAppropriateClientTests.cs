using Fetcharr.Models.Configuration;
using Fetcharr.Models.Configuration.Sonarr;
using Fetcharr.Provider.Sonarr.Models;
using Fetcharr.Testing.Layers;

namespace Fetcharr.Provider.Sonarr.Tests.SonarrClientCollectionTests
{
    public class FindAppropriateClientTests : SonarrTestingLayer
    {
        [Fact]
        public void FindAppropriateClient_ReturnsNull_GivenNoClients()
        {
            // Arrange
            SonarrClientCollection collection = this.CreateClientCollection();

            // Act
            SonarrClient? client = collection.FindAppropriateClient(new SonarrSeries()
            {
                Genres = ["anime"],
                Certification = "TV-MA"
            });

            // Assert
            client.Should().BeNull();
        }

        [Fact]
        public void FindAppropriateClient_PrefersExplicitFilter()
        {
            // Arrange
            SonarrClientCollection collection = this.CreateClientCollection(
            [
                new FetcharrSonarrConfiguration()
                {
                    Name = "sonarr-1",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = [],
                        Certification = []
                    }
                },
                new FetcharrSonarrConfiguration()
                {
                    Name = "sonarr-2",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = ["anime"],
                        Certification = []
                    }
                }
            ]);

            // Act
            SonarrClient? client = collection.FindAppropriateClient(new SonarrSeries()
            {
                Genres = ["anime"],
                Certification = "TV-MA"
            });

            // Assert
            client.Should().NotBeNull();
            client?.Name.Should().Be("sonarr-2");
        }

        [Fact]
        public void FindAppropriateClient_PrefersImplicitOverNothing()
        {
            // Arrange
            SonarrClientCollection collection = this.CreateClientCollection(
            [
                new FetcharrSonarrConfiguration()
                {
                    Name = "sonarr-1",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = ["action"],
                        Certification = []
                    }
                },
                new FetcharrSonarrConfiguration()
                {
                    Name = "sonarr-2",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = ["fantasy"],
                        Certification = []
                    }
                },
                new FetcharrSonarrConfiguration()
                {
                    Name = "sonarr-3",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = [],
                        Certification = []
                    }
                }
            ]);

            // Act
            SonarrClient? client = collection.FindAppropriateClient(new SonarrSeries()
            {
                Genres = ["anime"],
                Certification = "TV-MA"
            });

            // Assert
            client.Should().NotBeNull();
            client?.Name.Should().Be("sonarr-3");
        }
    }
}