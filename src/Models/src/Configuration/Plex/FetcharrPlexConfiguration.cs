using System.ComponentModel.DataAnnotations;

using YamlDotNet.Serialization;

namespace Fetcharr.Models.Configuration.Plex
{
    /// <summary>
    ///   Representation of a Plex configuration.
    /// </summary>
    public sealed class FetcharrPlexConfiguration
    {
        /// <summary>
        ///   Gets or sets the Plex API token for querying the Plex API.
        /// </summary>
        [Required]
        [YamlMember(Alias = "api_token")]
        public string ApiToken { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets whether to include friends' watchlists in the sync.
        /// </summary>
        [Required]
        [YamlMember(Alias = "sync_friends_watchlist")]
        public bool IncludeFriendsWatchlist { get; set; } = false;
    }
}