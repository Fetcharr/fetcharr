using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Plex.Models
{
    /// <summary>
    ///   Representation of a single Plex metadata item.
    /// </summary>
    public class PlexMetadataItem
    {
        /// <summary>
        ///   Gets or sets the title of the item.
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("originalTitle")]
        public string OriginalTitle { get; set; } = string.Empty;

        [JsonPropertyName("ratingKey")]
        public string RatingKey { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("duration")]
        public long Duration { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("Genre")]
        public List<PlexMetadataGenre> Genre { get; set; } = [];

        [JsonPropertyName("Guid")]
        public List<PlexMetadataGuid> Guid { get; set; } = [];

        /// <summary>
        ///   Gets whether this item is considered to be anime.
        /// </summary>
        public bool IsAnime => this.Genre.Any(v => v.Slug == "anime");

        /// <summary>
        ///   Gets the IMDB ID of the item, if available.
        /// </summary>
        public string? ImdbId =>
            this.Guid
                .FirstOrDefault(v => v.Id.StartsWith("imdb"))?.Id
                .Split("//")[^1];

        /// <summary>
        ///   Gets the TVDB ID of the item, if available.
        /// </summary>
        public string? TvdbId =>
            this.Guid
                .FirstOrDefault(v => v.Id.StartsWith("tvdb"))?.Id
                .Split("//")[^1];

        /// <summary>
        ///   Gets the TMDB ID of the item, if available.
        /// </summary>
        public string? TmdbId =>
            this.Guid
                .FirstOrDefault(v => v.Id.StartsWith("tmdb"))?.Id
                .Split("//")[^1];
    }
}