namespace Fetcharr.Models.Configuration.Sonarr
{
    /// <summary>
    ///   Enumeration of all available Sonarr series types.
    /// </summary>
    public enum SonarrSeriesType
    {
        /// <summary>
        ///   Standard item numbering (S01E05)
        /// </summary>
        Standard,

        /// <summary>
        ///   Date (2020-05-25)
        /// </summary>
        Daily,

        /// <summary>
        ///   Absolute episode number (005)
        /// </summary>
        Anime,
    }
}