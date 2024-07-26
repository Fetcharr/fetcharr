using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Plex.Models
{
    /// <summary>
    ///   Type of a Plex watchlist item.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum WatchlistMetadataItemType
    {
        /// <summary>
        ///   The watchlist item is a TV show.
        /// </summary>
        Show,

        /// <summary>
        ///   The watchlist item is a movie.
        /// </summary>
        Movie,
    }
}