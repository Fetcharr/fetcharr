using Fetcharr.API.Pipeline;
using Fetcharr.API.Pipeline.Queues;
using Fetcharr.Provider.Plex.Models;
using Fetcharr.Provider.Radarr;
using Fetcharr.Provider.Radarr.Models;

namespace Fetcharr.API.Services
{
    /// <summary>
    ///   Queue for storing Radarr movie requests, to be processed by <see cref="RadarrMovieService"/>.
    /// </summary>
    public class RadarrMovieQueue()
        : BaseUniqueTaskQueue<PlexMetadataItem>()
    {

    }

    /// <summary>
    ///   Hosted service for receiving items from the Plex watchlist and sending them to Radarr.
    /// </summary>
    public class RadarrMovieService(
        RadarrMovieQueue radarrMovieQueue,
        RadarrClientCollection radarrClientCollection,
        ILogger<RadarrMovieService> logger)
        : BasePeriodicService(TimeSpan.FromSeconds(20), logger)
    {
        public override async Task InvokeAsync(CancellationToken cancellationToken)
        {
            await foreach(PlexMetadataItem item in radarrMovieQueue.DequeueRangeAsync(max: 20, cancellationToken))
            {
                if(item.TmdbId is null)
                {
                    logger.LogError("Movie '{Title} ({Year})' has no TMDB ID.", item.Title, item.Year);
                    continue;
                }

                logger.LogDebug("Sending movie '{Title} ({Year})' to Radarr...", item.Title, item.Year);

                RadarrMovie? movie = await radarrClientCollection.GetMovieByTmdbAsync(item.TmdbId);
                if(movie is null)
                {
                    logger.LogError("Movie '{Title} ({Year})' could not be found on Radarr.", item.Title, item.Year);
                    continue;
                }

                RadarrClient? client = radarrClientCollection.FindAppropriateClient(movie);
                if(client is null)
                {
                    logger.LogError("Could not find appropriate Radarr instance for movie '{Title} ({Year})'.", item.Title, item.Year);
                    continue;
                }

                await client.AddMovieAsync(new RadarrMovieOptions(item.TmdbId));
            }
        }
    }
}