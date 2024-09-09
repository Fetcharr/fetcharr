using System.Net;

using Fetcharr.Models.Configuration;
using Fetcharr.Models.Configuration.Sonarr;
using Fetcharr.Provider.Sonarr.Models;

using Flurl.Http;

using Microsoft.Extensions.Logging;

namespace Fetcharr.Provider.Sonarr
{
    /// <summary>
    ///   Client for communicating with and instrumenting Sonarr instances.
    /// </summary>
    public class SonarrClient(
        FetcharrSonarrConfiguration configuration,
        ILogger<SonarrClient> logger)
        : ExternalProvider
    {
        /// <inheritdoc />
        public override string ProviderName { get; } = $"Sonarr ({configuration.Name})";

        /// <summary>
        ///   Gets the identifier of the Sonarr instance.
        /// </summary>
        public readonly string Name = configuration.Name;

        /// <summary>
        ///   Gets the filters for the Sonarr instance.
        /// </summary>
        public readonly ServiceFilterCollection Filters = configuration.Filters;

        /// <summary>
        ///   Gets the <see cref="FlurlClient"/> for interacting with the Sonarr API.
        /// </summary>
        private readonly FlurlClient _client =
            new FlurlClient(configuration.BaseUrl)
                .WithHeader("X-Api-Key", configuration.ApiKey);

        /// <inheritdoc />
        public override async Task<bool> PingAsync(CancellationToken cancellationToken)
        {
            IFlurlResponse response = await this._client.Request("/ping").GetAsync(cancellationToken: cancellationToken);
            return response.StatusCode == (int) HttpStatusCode.OK;
        }

        /// <summary>
        ///   Get all series from Sonarr.
        /// </summary>
        public virtual async Task<IEnumerable<SonarrSeries>> GetAllSeriesAsync() =>
            await this._client
                .Request("/api/v3/series")
                .GetJsonAsync<IEnumerable<SonarrSeries>>();

        /// <summary>
        ///   Get all root folders from Sonarr.
        /// </summary>
        public async Task<IEnumerable<SonarrRootFolder>> GetRootFoldersAsync() =>
            await this._client
                .Request("/api/v3/rootfolder")
                .GetJsonAsync<IEnumerable<SonarrRootFolder>>();

        /// <summary>
        ///   Get all quality profiles from Sonarr.
        /// </summary>
        public async Task<IEnumerable<SonarrQualityProfile>> GetQualityProfilesAsync() =>
            await this._client
                .Request("/api/v3/qualityprofile")
                .GetJsonAsync<IEnumerable<SonarrQualityProfile>>();

        /// <summary>
        ///   Gets a Sonarr series from it's TVDB ID.
        /// </summary>
        /// <param name="tvdbId">TVDB ID to get the series from.</param>
        /// <returns><see cref="SonarrSeries"/> if the series was found; otherwise, <see langword="null" />.</returns>
        public async Task<SonarrSeries?> GetSeriesByTvdbAsync(string tvdbId)
        {
            IEnumerable<SonarrSeries> series = await this._client
                .Request("/api/v3/series/lookup")
                .AppendQueryParam("term", $"tvdb:{tvdbId}")
                .GetJsonAsync<IEnumerable<SonarrSeries>>();

            return series.FirstOrDefault();
        }

        /// <summary>
        ///   Adds a series to Sonarr using the given <see cref="SonarrSeriesOptions"/>.
        /// </summary>
        /// <param name="options">Options and information about the series.</param>
        /// <returns><see cref="SonarrSeries"/> if the series was added; otherwise, <see langword="null" />.</returns>
        public async Task<SonarrSeries?> AddSeriesAsync(SonarrSeriesOptions options)
        {
            SonarrSeries? series = await this.GetSeriesByTvdbAsync(options.TvdbID);
            if(series is null)
            {
                logger.LogError(
                    "[{Instance}] Failed to find series on Sonarr: tvdb:{TvdbID}",
                    this.Name,
                    options.TvdbID);

                return null;
            }

            SonarrRootFolder rootFolder = await this.DetermineRootFolderAsync(options);
            SonarrQualityProfile qualityProfile = await this.DetermineQualityProfileAsync(options);

            object requestBody = new
            {
                tvdbId = options.TvdbID,
                title = series.Title,
                qualityProfileId = qualityProfile.Id,
                rootFolderPath = rootFolder.Path,
                path = $"{rootFolder.Path}/{options.Folder ?? series.Folder}",
                seasonFolder = options.SeasonFolder ?? configuration.SeasonFolder,
                monitored = options.Monitored ?? configuration.Monitored,
                monitorNewItems = (options.MonitorNewItems ?? configuration.MonitorNewItems) ? "all" : "none",
                seriesType = options.SeriesType ?? configuration.SeriesType,
                seasons = Array.Empty<int>(),
                tags = Array.Empty<int>(),
                addOptions = new
                {
                    ignoreEpisodesWithFiles = true,
                    searchForMissingEpisodes = configuration.SearchImmediately,
                    monitor = (options.MonitoredItems ?? configuration.MonitoredItems) switch
                    {
                        SonarrMonitoredItems.OnlyShortSeries => series switch
                        {
                            SonarrSeries s when s.Statistics.SeasonCount <= configuration.ShortSeriesThreshold
                                => "all",

                            _ => "firstSeason"
                        },

                        // Since all other enums are named after Sonarr's types,
                        // we should be able to just lower the first letter.
                        SonarrMonitoredItems item => char.ToLower(item.ToString()[0]) + item.ToString()[1..],
                    }
                }
            };

            // If the series already exists, just update it.
            if(series.Id is not null)
            {
                IFlurlResponse newSeriesResponse = await this._client
                    .Request($"/api/v3/series/{series.Id}")
                    .PutJsonAsync(requestBody);

                logger.LogDebug(
                    "Updated '{Title} ({Year})' in Sonarr instance '{Instance}'.",
                    series.Title,
                    series.Year,
                    this.Name);

                return await newSeriesResponse.GetJsonAsync<SonarrSeries>();
            }

            IFlurlResponse createdSeriesResponse = await this._client
                .Request("/api/v3/series")
                .PostJsonAsync(requestBody);

            logger.LogDebug(
                "Added '{Title} ({Year})' to Sonarr instance '{Instance}'.",
                series.Title,
                series.Year,
                this.Name);

            return await createdSeriesResponse.GetJsonAsync<SonarrSeries>();
        }

        /// <summary>
        ///   Adds a series to Sonarr using the given TVDB ID.
        /// </summary>
        /// <inheritdoc cref="AddSeriesAsync(SonarrSeriesOptions)" />
        public async Task<SonarrSeries?> AddSeriesAsync(string tvdbId)
            => await this.AddSeriesAsync(new SonarrSeriesOptions(tvdbId));

        /// <summary>
        ///   Gets a "filter score" for the given Sonarr series, given it's filter rules. The higher, the more preferred it is.
        /// </summary>
        public int GetFilterScore(SonarrSeries series)
        {
            int score = 0;

            if(series.IsInProduction)
            {
                return configuration.AllowInProduction ? 0 : -100;
            }

            score += this.Filters.Genre.GetFilterScore(series.Genres);
            score += this.Filters.Certification.GetFilterScore(series.Certification);

            return score;
        }

        /// <summary>
        ///   Determine the appropriate root folder for the series, given the series options,
        ///   the instance's configuration and any pre-defined root folders on the instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if no root folders are available on the instance.</exception>
        private async Task<SonarrRootFolder> DetermineRootFolderAsync(SonarrSeriesOptions options)
        {
            IEnumerable<SonarrRootFolder> rootFolders = await this.GetRootFoldersAsync();

            string path = options.RootFolder ?? configuration.RootFolder ?? rootFolders.FirstOrDefault()?.Path
                ?? throw new InvalidOperationException($"[{this.Name}] No root folder defined.");

            return rootFolders.FirstOrDefault(v =>
                v.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase))
                    ?? throw new InvalidOperationException($"[{this.Name}] No root folder defined.");
        }

        /// <summary>
        ///   Determine the appropriate quality profile for the series, given the series options,
        ///   the instance's configuration and any pre-defined quality profiles on the instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if no quality profiles are available on the instance.</exception>
        private async Task<SonarrQualityProfile> DetermineQualityProfileAsync(SonarrSeriesOptions options)
        {
            IEnumerable<SonarrQualityProfile> qualityProfiles = await this.GetQualityProfilesAsync();

            string name = options.QualityProfile ?? configuration.QualityProfile ?? qualityProfiles.FirstOrDefault()?.Name
                ?? throw new InvalidOperationException($"[{this.Name}] No quality profile defined.");

            return qualityProfiles.FirstOrDefault(v =>
                v.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    ?? throw new InvalidOperationException($"[{this.Name}] No quality profile defined.");
        }
    }
}