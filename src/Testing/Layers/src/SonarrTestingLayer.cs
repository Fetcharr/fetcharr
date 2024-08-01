using Fetcharr.Models.Configuration.Sonarr;
using Fetcharr.Provider.Sonarr;

using Microsoft.Extensions.Logging;

using Moq;

namespace Fetcharr.Testing.Layers
{
    public class SonarrTestingLayer
        : BaseServiceTestingLayer<SonarrClient>
    {
        public SonarrClient CreateClient(FetcharrSonarrConfiguration? configuration = null)
            => this.CreateService<SonarrClient>(configuration ?? new FetcharrSonarrConfiguration());

        public SonarrClientCollection CreateClientCollection(IEnumerable<FetcharrSonarrConfiguration>? configurations = null)
        {
            IEnumerable<SonarrClient> clients = (configurations ?? [])
                .Select(v => this.CreateClient(v));

            return new SonarrClientCollection(clients, new Mock<ILogger<SonarrClientCollection>>().Object);
        }
    }
}