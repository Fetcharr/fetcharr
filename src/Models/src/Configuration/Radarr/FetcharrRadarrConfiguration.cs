using YamlDotNet.Serialization;

namespace Fetcharr.Models.Configuration.Radarr
{
    /// <summary>
    ///   Representation of a Radarr configuration.
    /// </summary>
    public sealed class FetcharrRadarrConfiguration : FetcharrServiceConfiguration
    {
        /// <summary>
        ///   Gets or sets the minimum availability of the movie, before attempting to fetch it.
        /// </summary>
        [YamlMember(Alias = "minimum_availability")]
        public RadarrMovieStatus MinimumAvailability { get; set; } = RadarrMovieStatus.Released;

        /// <summary>
        ///   Gets or sets which items should be monitored, when adding the movie.
        /// </summary>
        [YamlMember(Alias = "monitored_items")]
        public RadarrMonitoredItems MonitoredItems { get; set; } = RadarrMonitoredItems.MovieOnly;
    }
}