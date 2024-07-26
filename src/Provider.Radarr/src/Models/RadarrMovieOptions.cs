using Fetcharr.Models.Configuration.Radarr;

namespace Fetcharr.Provider.Radarr.Models
{
    /// <summary>
    ///   Options for adding a movie to Radarr.
    /// </summary>
    /// <param name="tmdbId">TMDB ID of the movie.</param>
    public class RadarrMovieOptions(string tmdbId)
    {
        /// <summary>
        ///   Gets or sets the IMDB ID of the movie.
        /// </summary>
        public string? ImdbID { get; set; }

        /// <summary>
        ///   Gets or sets the TMDB ID of the movie.
        /// </summary>
        public string TmdbID { get; set; } = tmdbId;

        /// <summary>
        ///   Gets or sets the root folder to place the movie in. If not set, defaults to instance default.
        /// </summary>
        public string? RootFolder { get; set; }

        /// <summary>
        ///   Gets or sets the folder to place the movie in. If not set, defaults to instance default.
        /// </summary>
        public string? Folder { get; set; }

        /// <summary>
        ///   Gets or sets the minimum availability required before attempting to fetch the movie.
        ///   If not set, defaults to instance default.
        /// </summary>
        public RadarrMovieStatus? MinimumAvailability { get; set; }

        /// <summary>
        ///   Gets or sets whether the movie should be monitored. If not set, defaults to instance default.
        /// </summary>
        public bool? Monitored { get; set; }

        /// <summary>
        ///   Gets or sets which items to monitor, if the movie is monitored. If not set, defaults to instance default.
        /// </summary>
        public RadarrMonitoredItems? MonitoredItems { get; set; }

        /// <summary>
        ///   Gets or sets the quality profile to assign to the movie. If not set, defaults to instance default.
        /// </summary>
        public string? QualityProfile { get; set; }
    }
}