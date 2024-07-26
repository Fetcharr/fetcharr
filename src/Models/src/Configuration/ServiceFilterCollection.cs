using YamlDotNet.Serialization;

namespace Fetcharr.Models.Configuration
{
    /// <summary>
    ///   Collection of all service filters, which can be applied to service instances.
    /// </summary>
    public class ServiceFilterCollection
    {
        /// <summary>
        ///   Gets or sets the filters for the item genre.
        /// </summary>
        [YamlMember(Alias = "genre")]
        public ServiceFilter Genre { get; set; } = [];

        /// <summary>
        ///   Gets or sets the filters for the item certification.
        /// </summary>
        [YamlMember(Alias = "certification")]
        public ServiceFilter Certification { get; set; } = [];
    }
}