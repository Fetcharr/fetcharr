using Fetcharr.Testing.Containers.Sonarr;

using Flurl.Http;

using Xunit;

namespace Fetcharr.Testing.Layers
{
    /// <summary>
    ///   Base layer for testing Sonarr instances, using <see href="https://testcontainers.com/">TestContainers.</see>
    /// </summary>
    public class SonarrIntegrationLayer
        : SonarrTestingLayer
        , IAsyncLifetime
    {
        /// <summary>
        ///   Gets the Sonarr container instance.
        /// </summary>
        private readonly SonarrContainer _container = new SonarrBuilder()
            .Build();

        /// <summary>
        ///   Gets an HTTP client for interacting with the Sonarr instance.
        /// </summary>
        public FlurlClient SonarrApiClient =>
            new FlurlClient(this._container.EndpointBase)
                .WithHeader("X-Api-Key", this._container.Configuration.ApiKey);

        public async Task InitializeAsync()
            => await this._container.StartAsync();

        public async Task DisposeAsync()
            => await this._container.StopAsync();
    }
}