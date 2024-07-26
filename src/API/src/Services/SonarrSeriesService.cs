using Fetcharr.API.Pipeline;
using Fetcharr.API.Pipeline.Queues;
using Fetcharr.Provider.Plex.Models;
using Fetcharr.Provider.Sonarr;
using Fetcharr.Provider.Sonarr.Models;

namespace Fetcharr.API.Services
{
    /// <summary>
    ///   Queue for storing Sonarr series requests, to be processed by <see cref="SonarrSeriesService"/>.
    /// </summary>
    public class SonarrSeriesQueue()
        : BaseUniqueTaskQueue<PlexMetadataItem>()
    {

    }

    /// <summary>
    ///   Hosted service for receiving items from the Plex watchlist and sending them to Sonarr.
    /// </summary>
    public class SonarrSeriesService(
        SonarrSeriesQueue sonarrSeriesQueue,
        SonarrClientCollection sonarrClientCollection,
        ILogger<SonarrSeriesService> logger)
        : BasePeriodicService(TimeSpan.FromSeconds(20), logger)
    {
        public override async Task InvokeAsync(CancellationToken cancellationToken)
        {
            await foreach(PlexMetadataItem item in sonarrSeriesQueue.DequeueRangeAsync(max: 20, cancellationToken))
            {
                if(item.TvdbId is null)
                {
                    logger.LogError("Series '{Title} ({Year})' has no TVDB ID.", item.Title, item.Year);
                    continue;
                }

                logger.LogInformation("Sending series '{Title} ({Year})' to Sonarr...", item.Title, item.Year);

                SonarrSeries? series = await sonarrClientCollection.GetSeriesByTvdbAsync(item.TvdbId);
                if(series is null)
                {
                    logger.LogError("Series '{Title} ({Year})' could not be found on Sonarr.", item.Title, item.Year);
                    continue;
                }

                SonarrClient? client = sonarrClientCollection.FindAppropriateClient(series);
                if(client is null)
                {
                    logger.LogError("Could not find appropriate Sonarr instance for series '{Title} ({Year})'.", item.Title, item.Year);
                    continue;
                }

                await client.AddSeriesAsync(new SonarrSeriesOptions(item.TvdbId));
            }
        }
    }
}