using Fetcharr.Models.Configuration;
using Fetcharr.Models.Configuration.Radarr;
using Fetcharr.Provider.Radarr.Models;
using Fetcharr.Testing.Layers;

namespace Fetcharr.Provider.Radarr.Tests.RadarrClientCollectionTests
{
    public class FindAppropriateClientTests : RadarrTestingLayer
    {
        [Fact]
        public void FindAppropriateClient_ReturnsNull_GivenNoClients()
        {
            // Arrange
            RadarrClientCollection collection = this.CreateClientCollection();

            // Act
            RadarrClient? client = collection.FindAppropriateClient(new RadarrMovie()
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
            RadarrClientCollection collection = this.CreateClientCollection(
            [
                new FetcharrRadarrConfiguration()
                {
                    Name = "radarr-1",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = [],
                        Certification = []
                    }
                },
                new FetcharrRadarrConfiguration()
                {
                    Name = "radarr-2",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = ["anime"],
                        Certification = []
                    }
                }
            ]);

            // Act
            RadarrClient? client = collection.FindAppropriateClient(new RadarrMovie()
            {
                Genres = ["anime"],
                Certification = "TV-MA"
            });

            // Assert
            client.Should().NotBeNull();
            client?.Name.Should().Be("radarr-2");
        }

        [Fact]
        public void FindAppropriateClient_PrefersImplicitOverNothing()
        {
            // Arrange
            RadarrClientCollection collection = this.CreateClientCollection(
            [
                new FetcharrRadarrConfiguration()
                {
                    Name = "radarr-1",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = ["action"],
                        Certification = []
                    }
                },
                new FetcharrRadarrConfiguration()
                {
                    Name = "radarr-2",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = ["fantasy"],
                        Certification = []
                    }
                },
                new FetcharrRadarrConfiguration()
                {
                    Name = "radarr-3",
                    Filters = new ServiceFilterCollection()
                    {
                        Genre = [],
                        Certification = []
                    }
                }
            ]);

            // Act
            RadarrClient? client = collection.FindAppropriateClient(new RadarrMovie()
            {
                Genres = ["anime"],
                Certification = "TV-MA"
            });

            // Assert
            client.Should().NotBeNull();
            client?.Name.Should().Be("radarr-3");
        }
    }
}