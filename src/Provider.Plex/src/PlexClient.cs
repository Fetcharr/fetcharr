using System.Net;

using Fetcharr.Models.Configuration;

using Flurl.Http;

using Microsoft.Extensions.Options;

namespace Fetcharr.Provider.Plex
{
    /// <summary>
    ///   Client for interacting with Plex.
    /// </summary>
    public class PlexClient(
        IOptions<FetcharrConfiguration> configuration,
        PlexMetadataClient metadataClient,
        PlexWatchlistClient watchlistClient)
        : ExternalProvider
    {
        private readonly FlurlClient _client =
            new FlurlClient("https://plex.tv/")
                .WithHeader("X-Plex-Token", configuration.Value.Plex.ApiToken)
                .WithHeader("X-Plex-Client-Identifier", "fetcharr");

        /// <inheritdoc />
        public override string ProviderName { get; } = "Plex";

        /// <summary>
        ///   Gets the underlying client for interacting with metadata from Plex.
        /// </summary>
        public readonly PlexMetadataClient Metadata = metadataClient;

        /// <summary>
        ///   Gets the underlying client for interacting with Plex watchlists.
        /// </summary>
        public readonly PlexWatchlistClient Watchlist = watchlistClient;

        /// <inheritdoc />
        public override async Task<bool> PingAsync(CancellationToken cancellationToken)
        {
            IFlurlResponse response = await this._client.Request("/api/v2/ping").GetAsync(cancellationToken: cancellationToken);
            return response.StatusCode == (int) HttpStatusCode.OK;
        }
    }
}