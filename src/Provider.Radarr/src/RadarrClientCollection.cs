using Fetcharr.Models.Configuration;
using Fetcharr.Provider.Radarr.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fetcharr.Provider.Radarr
{
    /// <summary>
    ///   Collection of <see cref="RadarrClient"/>-instances.
    /// </summary>
    public class RadarrClientCollection : List<RadarrClient>
    {
        private readonly ILogger<RadarrClientCollection> _logger;

        public RadarrClientCollection(
            IServiceProvider provider,
            IOptions<FetcharrConfiguration> configuration,
            ILogger<RadarrClientCollection> logger)
            : base(RadarrClientCollection.Construct(configuration, provider))
            => this._logger = logger;

        internal RadarrClientCollection(
            IEnumerable<RadarrClient> clients,
            ILogger<RadarrClientCollection> logger)
            : base(clients)
            => this._logger = logger;

        private static IEnumerable<RadarrClient> Construct(
            IOptions<FetcharrConfiguration> configuration,
            IServiceProvider provider) =>
            configuration.Value.Radarr
                .Where(v => v.Value.Enabled)
                .Select(config => ActivatorUtilities.CreateInstance<RadarrClient>(provider, config.Value));

        /// <inheritdoc cref="RadarrClient.GetMovieByImdbAsync" />
        public async Task<RadarrMovie?> GetMovieByTmdbAsync(string tmdbId)
        {
            foreach(RadarrClient client in this)
            {
                RadarrMovie? movie = await client.GetMovieByTmdbAsync(tmdbId);
                if(movie is not null)
                {
                    return movie;
                }

                this._logger.LogInformation(
                    "Movie 'tmdb:{TmdbID}' could not be found on Radarr instance '{RadarrName}'.",
                    tmdbId, client.Name);
            }

            return null;
        }

        /// <summary>
        ///   Find the most appropriate client to contain the given movie,
        ///   <paramref name="movie"/>, based on the Radarr instance's filters.
        /// </summary>
        public RadarrClient? FindAppropriateClient(RadarrMovie movie) =>
            this.Select(client => new
            {
                Client = client,
                Score = client.GetFilterScore(movie)
            })
            .OrderByDescending(v => v.Score)
            .FirstOrDefault()?.Client;
    }
}