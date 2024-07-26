namespace Fetcharr.Provider.Plex.Models
{
    /// <summary>
    ///   Representation of a single watchlist item within a Plex watchlist.
    /// </summary>
    public class WatchlistMetadataItem
    {
        /// <summary>
        ///   Gets or sets the title of the item.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the rating key of the item, which can be used to retrieve metadata.
        /// </summary>
        public string RatingKey { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the year of the item's release.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        ///   Gets or sets the type of item this is.
        /// </summary>
        public WatchlistMetadataItemType Type { get; set; } = WatchlistMetadataItemType.Show;
    }
}