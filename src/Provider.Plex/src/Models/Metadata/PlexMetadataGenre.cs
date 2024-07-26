using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Plex.Models
{
    /// <summary>
    ///   Representation of a single genre within a Plex metadata item.
    /// </summary>
    public class PlexMetadataGenre
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("key")]
        public string Key { get; set; } = string.Empty;

        [JsonPropertyName("ratingKey")]
        public string RatingKey { get; set; } = string.Empty;

        [JsonPropertyName("slug")]
        public string Slug { get; set; } = string.Empty;

        [JsonPropertyName("tag")]
        public string Tag { get; set; } = string.Empty;

        [JsonPropertyName("filter")]
        public string Filter { get; set; } = string.Empty;

        [JsonPropertyName("context")]
        public string Context { get; set; } = string.Empty;

        [JsonPropertyName("directory")]
        public bool Directory { get; set; }
    }
}