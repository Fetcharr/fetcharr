using Fetcharr.Testing.Layers;

using Flurl.Http;

namespace Fetcharr.Testing.Containers.Tests.Integration
{
    public class RadarrIntegrationTests : RadarrIntegrationLayer
    {
        [Fact]
        [Trait("Category", "Integration")]
        public async Task AssertRadarrInstanceHealthy()
        {
            // Arrange

            // Act
            IFlurlResponse response = await this.RadarrApiClient.Request("/api/v3/health").GetAsync();

            // Assert
            response.StatusCode.Should().Be(200);
        }
    }
}