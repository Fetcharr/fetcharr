namespace Fetcharr.Provider.Sonarr.Models
{
    /// <summary>
    ///   Representation of a Sonarr series.
    /// </summary>
    public class SonarrSeries
    {
        /// <summary>
        ///   Gets or sets the ID of the series, within Sonarr.
        ///   If <see langword="null" />, the series does not yet exist in Sonarr.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        ///   Gets or sets the title of the series.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the release year of the series.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        ///   Gets or sets the status of the series.
        /// </summary>
        public SonarrSeriesStatus Status { get; set; }

        /// <summary>
        ///   Gets or sets the certification of the series.
        /// </summary>
        public string Certification { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the genres of the series.
        /// </summary>
        public List<string> Genres { get; set; } = [];

        /// <summary>
        ///   Gets or sets the folder to store the series in.
        /// </summary>
        public string Folder { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets some statistics about the series.
        /// </summary>
        public SonarrSeriesStatistics Statistics { get; set; } = new();

        /// <summary>
        ///   Gets whether this series is still in production.
        /// </summary>
        public bool IsInProduction => this.Genres.Count == 0 || string.IsNullOrEmpty(this.Certification);
    }
}