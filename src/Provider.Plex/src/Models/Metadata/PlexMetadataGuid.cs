using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Plex.Models
{
    /// <summary>
    ///   Representation of a single GUID within a Plex metadata item.
    /// </summary>
    public class PlexMetadataGuid
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
    }
}