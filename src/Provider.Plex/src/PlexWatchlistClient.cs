using System.Net;

using Cachedeer;

using Fetcharr.Models.Configuration;
using Fetcharr.Provider.Plex.Models;

using Flurl.Http;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fetcharr.Provider.Plex
{
    /// <summary>
    ///   Client for fetching watchlists from Plex.
    /// </summary>
    public class PlexWatchlistClient(
        IOptions<FetcharrConfiguration> configuration,
        [FromKeyedServices("watchlist")] ICachingProvider cachingProvider,
        ILogger<PlexWatchlistClient> logger)
    {
        private readonly FlurlClient _client =
            new FlurlClient("https://metadata.provider.plex.tv/library/sections/watchlist/")
                .WithHeader("X-Plex-Token", configuration.Value.Plex.ApiToken)
                .WithHeader("X-Plex-Client-Identifier", "fetcharr")
                .AllowHttpStatus((int) HttpStatusCode.NotModified);

        /// <summary>
        ///   If not <see langword="null" />, contains the E-Tag value of the last watchlist request.
        /// </summary>
        private string? lastEtag { get; set; } = null;

        /// <summary>
        ///   Fetch the watchlist for the current Plex account and return it.
        /// </summary>
        /// <param name="offset">Offset into the watchlist to fetch from.</param>
        /// <param name="limit">Maximum amount of items to retrieve from the watchlist.</param>
        public async Task<IEnumerable<WatchlistMetadataItem>> FetchWatchlistAsync(int offset = 0, int limit = 20)
        {
            IFlurlResponse response = await this._client
                .Request("all")
                .AppendQueryParam("X-Plex-Container-Start", offset.ToString())
                .AppendQueryParam("X-Plex-Container-Size", limit.ToString())
                .AppendQueryParam("format", "json")
                .WithHeader("If-None-Match", this.lastEtag)
                .GetAsync();

            if(response.StatusCode == (int) HttpStatusCode.NotModified)
            {
                CacheItem<IEnumerable<WatchlistMetadataItem>> cacheValue =
                    await cachingProvider.GetAsync<IEnumerable<WatchlistMetadataItem>>("watchlist");

                // If Plex returned NotModified, but the cache is empty, it must've been evicted
                // which means we have to resend the request without E-Tag caching.
                if(!cacheValue.HasValue)
                {
                    logger.LogInformation("Watchlist cache has been evicted; re-sending request...");

                    this.lastEtag = null;
                    return await this.FetchWatchlistAsync(offset, limit);
                }

                return cacheValue.Value;
            }

            MediaResponse<WatchlistMetadataItem> watchlistContainer = await response
                .GetJsonAsync<MediaResponse<WatchlistMetadataItem>>();

            if(response.Headers.TryGetFirst("etag", out string? etag))
            {
                this.lastEtag = etag;
            }

            IEnumerable<WatchlistMetadataItem> watchlist = watchlistContainer.MediaContainer.Metadata;
            await cachingProvider.SetAsync("watchlist", watchlist, expiration: TimeSpan.FromHours(1));

            return watchlist;
        }
    }
}