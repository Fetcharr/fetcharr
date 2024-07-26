namespace Fetcharr.Provider.Sonarr.Models
{
    /// <summary>
    ///   Statistics for a Sonarr series.
    /// </summary>
    public class SonarrSeriesStatistics
    {
        /// <summary>
        ///   Gets or sets the season count of a Sonarr series.
        /// </summary>
        public int SeasonCount { get; set; }

        /// <summary>
        ///   Gets or sets the episode count of a Sonarr series, which are monitored.
        /// </summary>
        public int EpisodeCount { get; set; }

        /// <summary>
        ///   Gets or sets the total episode count of a Sonarr series, including unmonitored episodes.
        /// </summary>
        public int TotalEpisodeCount { get; set; }
    }
}