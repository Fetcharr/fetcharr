using Fetcharr.Testing.Layers;

using Flurl.Http;

namespace Fetcharr.Testing.Containers.Tests.Integration
{
    [IntegrationTest]
    [Collection(nameof(SonarrIntegrationLayer))]
    public class SonarrIntegrationTests(
        SonarrIntegrationLayer layer)
    {
        [Fact]
        public async Task AssertSonarrInstanceHealthy()
        {
            // Arrange

            // Act
            IFlurlResponse response = await layer.SonarrApiClient.Request("/api/v3/health").GetAsync();

            // Assert
            response.StatusCode.Should().Be(200);
        }
    }
}