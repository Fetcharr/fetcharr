using Fetcharr.Models.Configuration.Sonarr;
using Fetcharr.Provider.Sonarr;
using Fetcharr.Provider.Sonarr.Models;

using Microsoft.Extensions.Logging;

namespace Fetcharr.Testing.Assertions.Tests
{
    [Category("Assertions")]
    public class SonarrClientAssertionsTests
    {
        [Fact(DisplayName = "ContainSeriesAsync should return true, given series list with matching title.")]
        public async Task ContainSeriesAsync_ShouldReturnTrue_GivenSeriesListWithSeries()
        {
            // Arrange
            SonarrClient client = Substitute.For<SonarrClient>(new FetcharrSonarrConfiguration(), Substitute.For<ILogger<SonarrClient>>());
            client.GetAllSeriesAsync().Returns(
            [
                new SonarrSeries()
                {
                    Title = "Succession"
                }
            ]);

            // Act

            // Assert
            await client.Should().ContainSeriesAsync("Succession");
        }

        [Fact(DisplayName = "ContainSeriesAsync should return true, given series list with case-insensitive title.")]
        public async Task ContainSeriesAsync_ShouldReturnTrue_GivenSeriesListWithCaseInsensitiveSeries()
        {
            // Arrange
            SonarrClient client = Substitute.For<SonarrClient>(new FetcharrSonarrConfiguration(), Substitute.For<ILogger<SonarrClient>>());
            client.GetAllSeriesAsync().Returns(
            [
                new SonarrSeries()
                {
                    Title = "succession"
                }
            ]);

            // Act

            // Assert
            await client.Should().ContainSeriesAsync("Succession");
        }

        [Fact(DisplayName = "NotContainSeriesAsync should return true, given empty series list.")]
        public async Task NotContainSeriesAsync_ShouldReturnTrue_GivenEmptySeriesList()
        {
            // Arrange
            SonarrClient client = Substitute.For<SonarrClient>(new FetcharrSonarrConfiguration(), Substitute.For<ILogger<SonarrClient>>());
            client.GetAllSeriesAsync().Returns([]);

            // Act

            // Assert
            await client.Should().NotContainSeriesAsync("Succession");
        }

        [Fact(DisplayName = "NotContainSeriesAsync should return true, given series list without matching series.")]
        public async Task NotContainSeriesAsync_ShouldReturnTrue_GivenSeriesListWithoutMatchingSeries()
        {
            // Arrange
            SonarrClient client = Substitute.For<SonarrClient>(new FetcharrSonarrConfiguration(), Substitute.For<ILogger<SonarrClient>>());
            client.GetAllSeriesAsync().Returns(
            [
                new SonarrSeries()
                {
                    Title = "Grey's Anatomy"
                }
            ]);

            // Act

            // Assert
            await client.Should().NotContainSeriesAsync("Succession");
        }
    }
}