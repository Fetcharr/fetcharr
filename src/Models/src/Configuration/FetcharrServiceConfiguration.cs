using System.ComponentModel.DataAnnotations;

using YamlDotNet.Serialization;

namespace Fetcharr.Models.Configuration
{
    /// <summary>
    ///   Base class for all service configurations in Fetcharr.
    /// </summary>
    public abstract class FetcharrServiceConfiguration
    {
        /// <summary>
        ///   Gets or sets the unique name of the service.
        /// </summary>
        [Required]
        [YamlMember(Alias = "name")]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets whether the service is enabled.
        /// </summary>
        [YamlMember(Alias = "enabled")]
        public bool Enabled { get; set; } = true;

        /// <summary>
        ///   Gets or sets base URL of the service.
        /// </summary>
        [Required]
        [YamlMember(Alias = "base_url")]
        public string BaseUrl { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the API key for accessing the service.
        /// </summary>
        [Required]
        [YamlMember(Alias = "api_key")]
        public string ApiKey { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the filers for the service.
        /// </summary>
        [YamlMember(Alias = "filters")]
        public ServiceFilterCollection Filters { get; set; } = new();

        /// <summary>
        ///   Gets or sets the default root folder for the service.
        /// </summary>
        [YamlMember(Alias = "root_folder")]
        public string? RootFolder { get; set; }

        /// <summary>
        ///   Gets or sets the default quality profile for the service.
        /// </summary>
        [YamlMember(Alias = "quality_profile")]
        public string? QualityProfile { get; set; }

        /// <summary>
        ///   Gets or sets whether added items should be monitored, by default.
        /// </summary>
        [YamlMember(Alias = "monitored")]
        public bool Monitored { get; set; } = true;

        /// <summary>
        ///   Gets or sets whether to immediately search after the item, after adding it.
        /// </summary>
        [YamlMember(Alias = "search_immediately")]
        public bool SearchImmediately { get; set; } = true;

        /// <summary>
        ///   Gets or sets whether items still in production can be added to the instance.
        /// </summary>
        [YamlMember(Alias = "allow_in_production")]
        public bool AllowInProduction { get; set; } = false;

        /// <summary>
        ///   Gets or sets whether existing items in the service should be updated or left as-is.
        /// </summary>
        [YamlMember(Alias = "update_existing")]
        public bool UpdateExisting { get; set; } = true;
    }
}