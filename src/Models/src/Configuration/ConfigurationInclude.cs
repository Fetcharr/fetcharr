using YamlDotNet.Serialization;

namespace Fetcharr.Models.Configuration
{
    /// <summary>
    ///   Represents an inclusion of another configuration file.
    /// </summary>
    public sealed class ConfigurationInclude
    {
        /// <summary>
        ///   Gets or sets the path of the YAML configuration to include.
        /// </summary>
        [YamlMember(Alias = "config")]
        public string? Config { get; set; }
    }
}