using System.Net;

using Fetcharr.Models.Configuration;
using Fetcharr.Models.Configuration.Radarr;
using Fetcharr.Provider.Radarr.Models;

using Flurl.Http;

using Microsoft.Extensions.Logging;

namespace Fetcharr.Provider.Radarr
{
    /// <summary>
    ///   Client for communicating with and instrumenting Radarr instances.
    /// </summary>
    public class RadarrClient(
        FetcharrRadarrConfiguration configuration,
        ILogger<RadarrClient> logger)
        : ExternalProvider
    {
        /// <inheritdoc />
        public override string ProviderName { get; } = $"Radarr ({configuration.Name})";

        /// <summary>
        ///   Gets the identifier of the Radarr instance.
        /// </summary>
        public readonly string Name = configuration.Name;

        /// <summary>
        ///   Gets the filters for the Radarr instance.
        /// </summary>
        public readonly ServiceFilterCollection Filters = configuration.Filters;

        /// <summary>
        ///   Gets the <see cref="FlurlClient"/> for interacting with the Radarr API.
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
        ///   Get all movies from Radarr.
        /// </summary>
        public virtual async Task<IEnumerable<RadarrMovie>> GetAllMoviesAsync() =>
            await this._client
                .Request("/api/v3/movies")
                .GetJsonAsync<IEnumerable<RadarrMovie>>();

        /// <summary>
        ///   Get all root folders from Radarr.
        /// </summary>
        public virtual async Task<IEnumerable<RadarrRootFolder>> GetRootFoldersAsync() =>
            await this._client
                .Request("/api/v3/rootfolder")
                .GetJsonAsync<IEnumerable<RadarrRootFolder>>();

        /// <summary>
        ///   Get all quality profiles from Radarr.
        /// </summary>
        public virtual async Task<IEnumerable<RadarrQualityProfile>> GetQualityProfilesAsync() =>
            await this._client
                .Request("/api/v3/qualityprofile")
                .GetJsonAsync<IEnumerable<RadarrQualityProfile>>();

        /// <summary>
        ///   Gets a Radarr movie from a search term.
        /// </summary>
        /// <param name="term">Search term to query Radarr for.</param>
        /// <returns><see cref="RadarrMovie"/> if the movie was found; otherwise, <see langword="null" />.</returns>
        public virtual async Task<RadarrMovie?> LookupMovieAsync(string term)
        {
            IEnumerable<RadarrMovie> movie = await this._client
                .Request("/api/v3/movie/lookup")
                .AppendQueryParam("term", term)
                .GetJsonAsync<IEnumerable<RadarrMovie>>();

            return movie.FirstOrDefault();
        }

        /// <summary>
        ///   Gets a Radarr movie from it's IMDB ID.
        /// </summary>
        /// <param name="imdbId">IMDB ID to get the movie from.</param>
        /// <returns><see cref="RadarrMovie"/> if the movie was found; otherwise, <see langword="null" />.</returns>
        public virtual async Task<RadarrMovie?> GetMovieByImdbAsync(string imdbId)
            => await this.LookupMovieAsync($"imdb:{imdbId}");

        /// <summary>
        ///   Gets a Radarr movie from it's TMDB ID.
        /// </summary>
        /// <param name="tmdbId">TMDB ID to get the movie from.</param>
        /// <returns><see cref="RadarrMovie"/> if the movie was found; otherwise, <see langword="null" />.</returns>
        public virtual async Task<RadarrMovie?> GetMovieByTmdbAsync(string tmdbId)
            => await this.LookupMovieAsync($"tmdb:{tmdbId}");

        /// <summary>
        ///   Gets a Radarr movie from it's metadata.
        /// </summary>
        /// <param name="options">Information about the movie to query for.</param>
        /// <returns><see cref="RadarrMovie"/> if the movie was found; otherwise, <see langword="null" />.</returns>
        public virtual async Task<RadarrMovie?> GetMovieAsync(RadarrMovieOptions options)
        {
            RadarrMovie? movie = await this.GetMovieByTmdbAsync(options.TmdbID);
            if(movie is not null)
            {
                return movie;
            }

            if(options.ImdbID is not null)
            {
                return await this.GetMovieByImdbAsync(options.ImdbID);
            }

            return null;
        }

        /// <summary>
        ///   Adds a movie to Radarr using the given <see cref="RadarrMovieOptions"/>.
        /// </summary>
        /// <param name="options">Options and information about the movie.</param>
        /// <returns><see cref="RadarrMovie"/> if the movie was added; otherwise, <see langword="null" />.</returns>
        public virtual async Task<RadarrMovie?> AddMovieAsync(RadarrMovieOptions options)
        {
            RadarrMovie? movie = await this.GetMovieAsync(options);
            if(movie is null)
            {
                logger.LogError(
                    "[{Instance}] Failed to find movie on Radarr: imdb:{ImdbID}, tmdb:{TmdbID}",
                    this.Name,
                    options.ImdbID,
                    options.TmdbID);

                return null;
            }

            RadarrRootFolder rootFolder = await this.DetermineRootFolderAsync(options);
            RadarrQualityProfile qualityProfile = await this.DetermineQualityProfileAsync(options);

            object requestBody = new
            {
                tmdbId = Convert.ToInt32(options.TmdbID),
                imdbId = options.ImdbID,
                title = movie.Title,
                qualityProfileId = qualityProfile.Id,
                rootFolderPath = rootFolder.Path,
                path = $"{rootFolder.Path}/{options.Folder ?? movie.Folder}",
                minimumAvailability = (options.MinimumAvailability ?? configuration.MinimumAvailability) switch
                {
                    RadarrMovieStatus.TBA => "tba",
                    RadarrMovieStatus.Announced => "announced",
                    RadarrMovieStatus.InCinemas => "inCinemas",
                    RadarrMovieStatus.Released => "released",
                    RadarrMovieStatus.Deleted => "deleted",

                    _ => throw new NotSupportedException()
                },
                monitored = options.Monitored ?? configuration.Monitored,
                tags = Array.Empty<int>(),
                addOptions = new
                {
                    ignoreEpisodesWithFiles = true,
                    searchForMovie = configuration.SearchImmediately,
                    monitor = (options.MonitoredItems ?? configuration.MonitoredItems) switch
                    {
                        RadarrMonitoredItems.None => "none",
                        RadarrMonitoredItems.MovieOnly => "movieOnly",
                        RadarrMonitoredItems.MovieAndCollection => "movieAndCollection",

                        _ => throw new NotSupportedException()
                    }
                }
            };

            // If the movie already exists, just update it.
            if(movie.Id is not null)
            {
                IFlurlResponse newMovieResponse = await this._client
                    .Request($"/api/v3/movie/{movie.Id}")
                    .PutJsonAsync(requestBody);

                logger.LogInformation(
                    "Updated '{Title} ({Year})' in Radarr instance '{Instance}'.",
                    movie.Title,
                    movie.Year,
                    this.Name);

                return await newMovieResponse.GetJsonAsync<RadarrMovie>();
            }

            IFlurlResponse createdMovieResponse = await this._client
                .Request("/api/v3/movie")
                .PostJsonAsync(requestBody);

            logger.LogInformation(
                "Added '{Title} ({Year})' to Radarr instance '{Instance}'.",
                movie.Title,
                movie.Year,
                this.Name);

            return await createdMovieResponse.GetJsonAsync<RadarrMovie>();
        }

        /// <summary>
        ///   Adds a movie to Radarr using the given IMDB ID.
        /// </summary>
        /// <inheritdoc cref="AddMovieAsync(RadarrMovieOptions)" />
        public virtual async Task<RadarrMovie?> AddMovieAsync(string imdbId)
            => await this.AddMovieAsync(new RadarrMovieOptions(imdbId));

        /// <summary>
        ///   Gets a "filter score" for the given Radarr movie, given it's filter rules. The higher, the more preferred it is.
        /// </summary>
        public virtual int GetFilterScore(RadarrMovie movie)
        {
            int score = 0;

            if(movie.IsInProduction)
            {
                return configuration.AllowInProduction ? 0 : -100;
            }

            score += this.Filters.Genre.GetFilterScore(movie.Genres);
            score += this.Filters.Certification.GetFilterScore(movie.Certification);

            return score;
        }

        /// <summary>
        ///   Determine the appropriate root folder for the movie, given the movie options,
        ///   the instance's configuration and any pre-defined root folders on the instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if no root folders are available on the instance.</exception>
        private async Task<RadarrRootFolder> DetermineRootFolderAsync(RadarrMovieOptions options)
        {
            IEnumerable<RadarrRootFolder> rootFolders = await this.GetRootFoldersAsync();

            string path = options.RootFolder ?? configuration.RootFolder ?? rootFolders.FirstOrDefault()?.Path
                ?? throw new InvalidOperationException($"[{this.Name}] No root folder defined.");

            return rootFolders.FirstOrDefault(v =>
                v.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase))
                    ?? throw new InvalidOperationException($"[{this.Name}] No root folder defined.");
        }

        /// <summary>
        ///   Determine the appropriate quality profile for the movie, given the movie options,
        ///   the instance's configuration and any pre-defined quality profiles on the instance.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if no quality profiles are available on the instance.</exception>
        private async Task<RadarrQualityProfile> DetermineQualityProfileAsync(RadarrMovieOptions options)
        {
            IEnumerable<RadarrQualityProfile> qualityProfiles = await this.GetQualityProfilesAsync();

            string name = options.QualityProfile ?? configuration.QualityProfile ?? qualityProfiles.FirstOrDefault()?.Name
                ?? throw new InvalidOperationException($"[{this.Name}] No quality profile defined.");

            return qualityProfiles.FirstOrDefault(v =>
                v.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
                    ?? throw new InvalidOperationException($"[{this.Name}] No quality profile defined.");
        }
    }
}