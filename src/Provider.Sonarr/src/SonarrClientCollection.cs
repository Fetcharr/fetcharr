using Fetcharr.Models.Configuration;
using Fetcharr.Provider.Sonarr.Models;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fetcharr.Provider.Sonarr
{
    /// <summary>
    ///   Collection of <see cref="SonarrClient"/>-instances.
    /// </summary>
    public class SonarrClientCollection : List<SonarrClient>
    {
        private readonly ILogger<SonarrClientCollection> _logger;

        public SonarrClientCollection(
            IServiceProvider provider,
            FetcharrConfiguration configuration,
            ILogger<SonarrClientCollection> logger)
            : base(SonarrClientCollection.Construct(configuration, provider))
            => this._logger = logger;

        internal SonarrClientCollection(
            IEnumerable<SonarrClient> clients,
            ILogger<SonarrClientCollection> logger)
            : base(clients)
            => this._logger = logger;

        /// <inheritdoc cref="SonarrClient.GetSeriesByTvdbAsync" />
        public async Task<SonarrSeries?> GetSeriesByTvdbAsync(string tvdbId)
        {
            foreach(SonarrClient client in this)
            {
                SonarrSeries? series = await client.GetSeriesByTvdbAsync(tvdbId);
                if(series is not null)
                {
                    return series;
                }

                this._logger.LogInformation(
                    "Series 'tvdb:{TvdbID}' could not be found on Sonarr instance '{SonarrName}'.",
                    tvdbId, client.Name);
            }

            return null;
        }

        /// <summary>
        ///   Find the most appropriate client to contain the given series,
        ///   <paramref name="series"/>, based on the Sonarr instance's filters.
        /// </summary>
        public SonarrClient? FindAppropriateClient(SonarrSeries series) =>
            this.Select(client => new
            {
                Client = client,
                Score = client.GetFilterScore(series)
            })
            .OrderByDescending(v => v.Score)
            .FirstOrDefault()?.Client;

        private static IEnumerable<SonarrClient> Construct(
            FetcharrConfiguration configuration,
            IServiceProvider provider) =>
            configuration.Sonarr
                .Where(v => v.Enabled)
                .Select(config => ActivatorUtilities.CreateInstance<SonarrClient>(provider, config));
    }
}