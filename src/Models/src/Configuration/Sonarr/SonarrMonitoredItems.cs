namespace Fetcharr.Models.Configuration.Sonarr
{
    /// <summary>
    ///   Enumeration of all possible monitoring states for series in Sonarr.
    /// </summary>
    public enum SonarrMonitoredItems
    {
        /// <summary>
        ///   No monitoring.
        /// </summary>
        None,

        /// <summary>
        ///   Monitor all seasons.
        /// </summary>
        All,

        /// <summary>
        ///   Monitor only the first season.
        /// </summary>
        FirstSeason,

        /// <summary>
        ///   Monitor all season if series is short. Otherwise, monitor first season.
        /// </summary>
        /// <seealso cref="FetcharrSonarrConfiguration.ShortSeriesThreshold" />
        OnlyShortSeries
    }
}