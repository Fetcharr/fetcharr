using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Plex.Models
{
    /// <summary>
    ///   Representation of a friend user account.
    /// </summary>
    public class PlexFriendUser
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("username")]
        public string Username { get; set; } = string.Empty;
    }
}