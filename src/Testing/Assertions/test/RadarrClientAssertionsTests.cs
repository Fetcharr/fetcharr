using Fetcharr.Models.Configuration.Radarr;
using Fetcharr.Provider.Radarr;
using Fetcharr.Provider.Radarr.Models;

using Microsoft.Extensions.Logging;

namespace Fetcharr.Testing.Assertions.Tests
{
    [Category("Assertions")]
    public class RadarrClientAssertions
    {
        [Fact(DisplayName = "ContainMovieAsync should return true, given movie list with matching title.")]
        public async Task ContainMovieAsync_ShouldReturnTrue_GivenMovieListWithMovie()
        {
            // Arrange
            RadarrClient client = Substitute.For<RadarrClient>(new FetcharrRadarrConfiguration(), Substitute.For<ILogger<RadarrClient>>());
            client.GetAllMoviesAsync().Returns(
            [
                new RadarrMovie()
                {
                    Title = "Green Mile"
                }
            ]);

            // Act

            // Assert
            await client.Should().ContainMovieAsync("Green Mile");
        }

        [Fact(DisplayName = "ContainMovieAsync should return true, given movie list with case-insensitive title.")]
        public async Task ContainMovieAsync_ShouldReturnTrue_GivenMovieListWithCaseInsensitiveMovie()
        {
            // Arrange
            RadarrClient client = Substitute.For<RadarrClient>(new FetcharrRadarrConfiguration(), Substitute.For<ILogger<RadarrClient>>());
            client.GetAllMoviesAsync().Returns(
            [
                new RadarrMovie()
                {
                    Title = "green mile"
                }
            ]);

            // Act

            // Assert
            await client.Should().ContainMovieAsync("Green Mile");
        }

        [Fact(DisplayName = "NotContainMovieAsync should return true, given empty movie list.")]
        public async Task NotContainMovieAsync_ShouldReturnTrue_GivenEmptyMovieList()
        {
            // Arrange
            RadarrClient client = Substitute.For<RadarrClient>(new FetcharrRadarrConfiguration(), Substitute.For<ILogger<RadarrClient>>());
            client.GetAllMoviesAsync().Returns([]);

            // Act

            // Assert
            await client.Should().NotContainMovieAsync("Green Mile");
        }

        [Fact(DisplayName = "NotContainMovieAsync should return true, given movie list without matching movie.")]
        public async Task NotContainMovieAsync_ShouldReturnTrue_GivenMovieListWithoutMatchingMovie()
        {
            // Arrange
            RadarrClient client = Substitute.For<RadarrClient>(new FetcharrRadarrConfiguration(), Substitute.For<ILogger<RadarrClient>>());
            client.GetAllMoviesAsync().Returns(
            [
                new RadarrMovie()
                {
                    Title = "Forrest Gump"
                }
            ]);

            // Act

            // Assert
            await client.Should().NotContainMovieAsync("Green Mile");
        }
    }
}