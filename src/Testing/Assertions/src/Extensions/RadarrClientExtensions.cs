using Fetcharr.Provider.Radarr;

namespace Fetcharr.Testing.Assertions.Extensions
{
    public static partial class RadarrClientExtensions
    {
        public static RadarrClientAssertions Should(this RadarrClient instance)
            => new(instance);
    }
}