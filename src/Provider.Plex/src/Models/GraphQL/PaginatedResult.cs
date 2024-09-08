using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Plex.Models
{
    public class PaginatedResult<T>
    {
        [JsonPropertyName("nodes")]
        public List<T> Nodes { get; set; } = [];
    }
}