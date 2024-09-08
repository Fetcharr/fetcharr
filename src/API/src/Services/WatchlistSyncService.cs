using Fetcharr.API.Pipeline;
using Fetcharr.Models.Configuration;
using Fetcharr.Provider.Plex;
using Fetcharr.Provider.Plex.Models;

using Microsoft.Extensions.Options;

namespace Fetcharr.API.Services
{
    /// <summary>
    ///   Hosted service for fetching the watchlist for the configured Plex account
    ///   and enqueuing the items for other services.
    /// </summary>
    public class WatchlistSyncService(
        PlexClient plexClient,
        SonarrSeriesQueue sonarrSeriesQueue,
        RadarrMovieQueue radarrMovieQueue,
        IOptions<FetcharrConfiguration> configuration,
        ILogger<WatchlistSyncService> logger)
        : BasePeriodicService(TimeSpan.FromSeconds(30), logger)
    {
        public override async Task InvokeAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Syncing Plex watchlist...");

            IEnumerable<WatchlistMetadataItem> watchlistItems = await this.GetAllWatchlistsAsync();

            foreach(WatchlistMetadataItem item in watchlistItems)
            {
                PlexMetadataItem? metadata = await plexClient.Metadata.GetMetadataFromRatingKeyAsync(item.RatingKey);
                if(metadata is null)
                {
                    logger.LogError(
                        "Failed to retrieve metadata from Plex: '{Title} ({Year})' ({RatingKey})",
                        item.Title,
                        item.Year,
                        item.RatingKey);

                    continue;
                }

                ITaskQueue<PlexMetadataItem> queue = item.Type switch
                {
                    WatchlistMetadataItemType.Movie => radarrMovieQueue,
                    WatchlistMetadataItemType.Show => sonarrSeriesQueue,

                    _ => throw new NotSupportedException()
                };

                await queue.EnqueueAsync(metadata, cancellationToken);
            }
        }

        private async Task<IEnumerable<WatchlistMetadataItem>> GetAllWatchlistsAsync()
        {
            List<WatchlistMetadataItem> watchlistItems = [];

            // Add own watchlist
            watchlistItems.AddRange(await plexClient.Watchlist.FetchWatchlistAsync(limit: 5));

            // Add friends' watchlists, if enabled.
            if(configuration.Value.Plex.IncludeFriendsWatchlist)
            {
                watchlistItems.AddRange(await plexClient.FriendsWatchlistClient.FetchAllWatchlistsAsync());
            }

            return watchlistItems;
        }
    }
}