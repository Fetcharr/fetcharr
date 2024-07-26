using Fetcharr.Cache.Core;

namespace Fetcharr.Cache.InMemory
{
    /// <summary>
    ///   Options for the in-memory caching provider, <see cref="InMemoryCachingProvider"/>.
    /// </summary>
    public class InMemoryCachingProviderOptions(string name) : BaseCachingProviderOptions(name)
    {
        /// <summary>
        ///   Gets or sets the size limit of the in-memory cache.
        ///   If above 0, limits the in-memory cache to <see cref="SizeLimit"/> elements.
        /// </summary>
        public long SizeLimit { get; set; } = 1024;
    }
}