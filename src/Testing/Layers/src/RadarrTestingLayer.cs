using Fetcharr.Models.Configuration.Radarr;
using Fetcharr.Provider.Radarr;

using Microsoft.Extensions.Logging;

using Moq;

namespace Fetcharr.Testing.Layers
{
    /// <summary>
    ///   Base layer for testing Radarr clients, <see cref="RadarrClient" />, and related types.
    /// </summary>
    public abstract class RadarrTestingLayer
        : BaseServiceTestingLayer<RadarrClient>
    {
        /// <summary>
        ///   Creates an instance of <see cref="RadarrClient"/>; optionally with a configuration.
        /// </summary>
        /// <param name="configuration">Configuration for the client. If not set, uses the default values.</param>
        public RadarrClient CreateClient(FetcharrRadarrConfiguration? configuration = null)
            => this.CreateService<RadarrClient>(configuration ?? new FetcharrRadarrConfiguration());

        /// <summary>
        ///   Creates a collection of <see cref="RadarrClient"/> instances; optionally with a list of configurations.
        /// </summary>
        /// <param name="configurations">List of configuration for the clients. If not set, creates an empty list.</param>
        public RadarrClientCollection CreateClientCollection(IEnumerable<FetcharrRadarrConfiguration>? configurations = null)
        {
            IEnumerable<RadarrClient> clients = (configurations ?? [])
                .Select(v => this.CreateClient(v));

            return new RadarrClientCollection(clients, new Mock<ILogger<RadarrClientCollection>>().Object);
        }
    }
}