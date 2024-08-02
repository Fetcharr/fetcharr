using Fetcharr.Testing.Layers;

namespace Fetcharr.Testing.Containers.Tests.Integration
{
    [CollectionDefinition(nameof(SonarrIntegrationLayer))]
    public class SonarrIntegrationLayerCollection : ICollectionFixture<SonarrIntegrationLayer>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}