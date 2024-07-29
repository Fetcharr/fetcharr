using Fetcharr.Models.Configuration.Plex;
using Fetcharr.Models.Configuration.Radarr;
using Fetcharr.Models.Configuration.Sonarr;

using YamlDotNet.Serialization;

using static System.StringComparer;

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
        [YamlMember(Alias = "plex")]
        public FetcharrPlexConfiguration Plex { get; set; } = new();

        /// <summary>
        ///   Gets or sets the configuration for Radarr instances within Fetcharr.
        /// </summary>
        [YamlMember(Alias = "radarr")]
        public Dictionary<string, FetcharrRadarrConfiguration> Radarr { get; set; } = new(InvariantCultureIgnoreCase);

        /// <summary>
        ///   Gets or sets the configuration for Sonarr instances within Fetcharr.
        /// </summary>
        [YamlMember(Alias = "sonarr")]
        public Dictionary<string, FetcharrSonarrConfiguration> Sonarr { get; set; } = new(InvariantCultureIgnoreCase);

        /// <summary>
        ///   Gets or sets a list of inclusions for the configuration.
        /// </summary>
        [YamlMember(Alias = "include")]
        public List<ConfigurationInclude> Includes { get; set; } = [];
    }
}