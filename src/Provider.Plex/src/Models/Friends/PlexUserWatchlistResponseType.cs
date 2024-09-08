using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Plex.Models
{
    public class PlexUserWatchlistResponseType
    {
        [JsonPropertyName("user")]
        public PlexWatchlistResponseType User { get; set; } = new();
    }

    public class PlexWatchlistResponseType
    {
        [JsonPropertyName("watchlist")]
        public PaginatedResult<WatchlistMetadataItem> Watchlist { get; set; } = new();
    }
}