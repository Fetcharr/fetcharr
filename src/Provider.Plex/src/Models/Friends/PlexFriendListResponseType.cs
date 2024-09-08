using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Plex.Models
{
    public class PlexFriendListResponseType
    {
        [JsonPropertyName("allFriendsV2")]
        public List<PlexFriendUserContainer> Friends { get; set; } = [];
    }
}