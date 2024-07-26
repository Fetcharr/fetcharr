using YamlDotNet.Serialization;

namespace Fetcharr.Models.Configuration.Sonarr
{
    /// <summary>
    ///   Representation of a Sonarr configuration.
    /// </summary>
    public sealed class FetcharrSonarrConfiguration : FetcharrServiceConfiguration
    {
        /// <summary>
        ///   Gets or sets the type of series.
        /// </summary>
        [YamlMember(Alias = "series_type")]
        public SonarrSeriesType SeriesType { get; set; } = SonarrSeriesType.Standard;

        /// <summary>
        ///   Gets or sets whether to create a folder for each season.
        /// </summary>
        [YamlMember(Alias = "season_folder")]
        public bool SeasonFolder { get; set; } = true;

        /// <summary>
        ///   Gets or sets whether to monitor new items in the series.
        /// </summary>
        [YamlMember(Alias = "monitor_new_items")]
        public bool MonitorNewItems { get; set; } = false;

        /// <summary>
        ///   Gets or sets which items should be monitored.
        /// </summary>
        [YamlMember(Alias = "monitored_items")]
        public SonarrMonitoredItems MonitoredItems { get; set; } = SonarrMonitoredItems.FirstSeason;

        /// <summary>
        ///   Gets or sets the threshold for what counts as a short series, in seasons.
        /// </summary>
        [YamlMember(Alias = "short_series_threshold")]
        public int ShortSeriesThreshold { get; set; } = 3;
    }
}