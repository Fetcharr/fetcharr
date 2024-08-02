using Fetcharr.Testing.Layers;

using Flurl.Http;

namespace Fetcharr.Testing.Containers.Tests.Integration
{
    [Collection(nameof(RadarrIntegrationLayer))]
    public class RadarrIntegrationTests(
        RadarrIntegrationLayer layer)
    {
        [Fact]
        public async Task AssertRadarrInstanceHealthy()
        {
            // Arrange

            // Act
            IFlurlResponse response = await layer.RadarrApiClient.Request("/api/v3/health").GetAsync();

            // Assert
            response.StatusCode.Should().Be(200);
        }
    }
}