using Fetcharr.Testing.Containers.Radarr;

using Flurl.Http;

using Xunit;

namespace Fetcharr.Testing.Layers
{
    /// <summary>
    ///   Base layer for testing Radarr instances, using <see href="https://testcontainers.com/">TestContainers.</see>
    /// </summary>
    public abstract class RadarrIntegrationLayer
        : RadarrTestingLayer
        , IAsyncLifetime
    {
        /// <summary>
        ///   Gets the Radarr container instance.
        /// </summary>
        private readonly RadarrContainer _container;

        /// <summary>
        ///   Gets an HTTP client for interacting with the Radarr instance.
        /// </summary>
        public readonly FlurlClient RadarrApiClient;

        public RadarrIntegrationLayer()
        {
            this._container = new RadarrBuilder()
                .Build();

            this.RadarrApiClient = new FlurlClient(this._container.EndpointBase)
                .WithHeader("X-Api-Key", this._container.Configuration.ApiKey);
        }

        public async Task InitializeAsync()
            => await this._container.StartAsync();

        public async Task DisposeAsync()
            => await this._container.StopAsync();
    }
}