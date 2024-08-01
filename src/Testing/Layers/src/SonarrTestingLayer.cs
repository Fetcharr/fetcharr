using Fetcharr.Models.Configuration.Sonarr;
using Fetcharr.Provider.Sonarr;

using Microsoft.Extensions.Logging;

using Moq;

namespace Fetcharr.Testing.Layers
{
    /// <summary>
    ///   Base layer for testing Radarr clients, <see cref="SonarrClient" />, and related types.
    /// </summary>
    public class SonarrTestingLayer
        : BaseServiceTestingLayer<SonarrClient>
    {
        /// <summary>
        ///   Creates an instance of <see cref="SonarrClient"/>; optionally with a configuration.
        /// </summary>
        /// <param name="configuration">Configuration for the client. If not set, uses the default values.</param>
        public SonarrClient CreateClient(FetcharrSonarrConfiguration? configuration = null)
            => this.CreateService<SonarrClient>(configuration ?? new FetcharrSonarrConfiguration());

        /// <summary>
        ///   Creates a collection of <see cref="SonarrClient"/> instances; optionally with a list of configurations.
        /// </summary>
        /// <param name="configurations">List of configuration for the clients. If not set, creates an empty list.</param>
        public SonarrClientCollection CreateClientCollection(IEnumerable<FetcharrSonarrConfiguration>? configurations = null)
        {
            IEnumerable<SonarrClient> clients = (configurations ?? [])
                .Select(v => this.CreateClient(v));

            return new SonarrClientCollection(clients, new Mock<ILogger<SonarrClientCollection>>().Object);
        }
    }
}