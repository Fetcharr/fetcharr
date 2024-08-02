using Fetcharr.Testing.Layers;

namespace Fetcharr.Testing.Containers.Tests.Integration
{
    [CollectionDefinition(nameof(RadarrIntegrationLayer))]
    public class RadarrIntegrationLayerCollection : ICollectionFixture<RadarrIntegrationLayer>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}