using Fetcharr.Models.Configuration.Radarr;
using Fetcharr.Provider.Radarr;

using Microsoft.Extensions.Logging;

using Moq;

namespace Fetcharr.Testing.Layers
{
    public class RadarrTestingLayer
        : BaseServiceTestingLayer<RadarrClient>
    {
        public RadarrClient CreateClient(FetcharrRadarrConfiguration? configuration = null)
            => this.CreateService<RadarrClient>(configuration ?? new FetcharrRadarrConfiguration());

        public RadarrClientCollection CreateClientCollection(IEnumerable<FetcharrRadarrConfiguration>? configurations = null)
        {
            IEnumerable<RadarrClient> clients = (configurations ?? [])
                .Select(v => this.CreateClient(v));

            return new RadarrClientCollection(clients, new Mock<ILogger<RadarrClientCollection>>().Object);
        }
    }
}