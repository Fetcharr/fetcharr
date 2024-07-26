using Fetcharr.Models.Configuration.Sonarr;

namespace Fetcharr.Provider.Sonarr.Models
{
    /// <summary>
    ///   Options for adding a series to Sonarr.
    /// </summary>
    /// <param name="tvdbId">TVDB ID of the series.</param>
    public class SonarrSeriesOptions(string tvdbId)
    {
        /// <summary>
        ///   Gets or sets the TVDB ID of the series.
        /// </summary>
        public string TvdbID { get; set; } = tvdbId;

        /// <summary>
        ///   Gets or sets the root folder to place the series in. If not set, defaults to instance default.
        /// </summary>
        public string? RootFolder { get; set; }

        /// <summary>
        ///   Gets or sets the folder to place the series in. If not set, defaults to instance default.
        /// </summary>
        public string? Folder { get; set; }

        /// <summary>
        ///   Gets or sets the type of the series. If not set, defaults to instance default.
        /// </summary>
        public SonarrSeriesType? SeriesType { get; set; }

        /// <summary>
        ///   Gets or sets whether to place seasons in their own folders. If not set, defaults to instance default.
        /// </summary>
        public bool? SeasonFolder { get; set; }

        /// <summary>
        ///   Gets or sets whether the series should be monitored. If not set, defaults to instance default.
        /// </summary>
        public bool? Monitored { get; set; }

        /// <summary>
        ///   Gets or sets whether to monitor new seasons in the series. If not set, defaults to instance default.
        /// </summary>
        public bool? MonitorNewItems { get; set; }

        /// <summary>
        ///   Gets or sets the quality profile to assign to the series. If not set, defaults to instance default.
        /// </summary>
        public string? QualityProfile { get; set; }

        /// <summary>
        ///   Gets or sets which items to monitor, if the series is monitored. If not set, defaults to instance default.
        /// </summary>
        public SonarrMonitoredItems? MonitoredItems { get; set; }
    }
}