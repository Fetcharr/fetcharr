using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Plex.Models
{
    /// <summary>
    ///   Representation of a friend user account container.
    /// </summary>
    public class PlexFriendUserContainer
    {
        [JsonPropertyName("user")]
        public PlexFriendUser User { get; set; } = new();
    }
}