using Fetcharr.Provider.Sonarr;

namespace Fetcharr.Testing.Assertions.Extensions
{
    public static partial class SonarrClientExtensions
    {
        public static SonarrClientAssertions Should(this SonarrClient instance)
            => new(instance);
    }
}