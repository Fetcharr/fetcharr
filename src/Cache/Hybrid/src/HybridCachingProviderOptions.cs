using Fetcharr.Cache.Core;
using Fetcharr.Cache.InMemory;
using Fetcharr.Cache.SQLite;

namespace Fetcharr.Cache.Hybrid
{
    /// <summary>
    ///   Options for the hybrid caching provider, <see cref="HybridCachingProvider"/>.
    /// </summary>
    public class HybridCachingProviderOptions(string name) : BaseCachingProviderOptions(name)
    {
        /// <summary>
        ///   Gets or sets the options of the in-memory cache.
        /// </summary>
        public InMemoryCachingProviderOptions InMemory { get; set; } = new($"{name}-mem")
        {
            DefaultExpiration = TimeSpan.FromMinutes(5),
            SizeLimit = 512,
            EnableLogging = false,
        };

        /// <summary>
        ///   Gets or sets the options of the disk cache.
        /// </summary>
        public SQLiteCachingProviderOptions SQLite { get; set; } = new($"{name}-disk")
        {
            DefaultExpiration = TimeSpan.FromHours(1),
            EnableLogging = false,
        };
    }
}