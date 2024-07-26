using System.ComponentModel.DataAnnotations;

using Fetcharr.Models.Configuration.Plex;
using Fetcharr.Models.Configuration.Radarr;
using Fetcharr.Models.Configuration.Sonarr;

using YamlDotNet.Serialization;

namespace Fetcharr.Models.Configuration
{
    /// <summary>
    ///   Represents the configuration of Fetcharr.
    /// </summary>
    public sealed class FetcharrConfiguration
    {
        /// <summary>
        ///   Gets or sets the configuration for Plex within Fetcharr.
        /// </summary>
        [Required]
        [YamlMember(Alias = "plex")]
        public FetcharrPlexConfiguration Plex { get; set; } = new();

        /// <summary>
        ///   Gets or sets the configuration for Radarr instances within Fetcharr.
        /// </summary>
        [Required]
        [YamlMember(Alias = "radarr")]
        public FetcharrRadarrConfiguration[] Radarr { get; set; } = [];

        /// <summary>
        ///   Gets or sets the configuration for Sonarr instances within Fetcharr.
        /// </summary>
        [Required]
        [YamlMember(Alias = "sonarr")]
        public FetcharrSonarrConfiguration[] Sonarr { get; set; } = [];
    }
}