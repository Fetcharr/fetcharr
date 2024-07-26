using Fetcharr.Cache.Core;
using Fetcharr.Cache.Core.Logging;
using Fetcharr.Cache.InMemory;
using Fetcharr.Cache.SQLite;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.Hybrid
{
    /// <summary>
    ///   Provider implementation for hybrid caching.
    /// </summary>
    public class HybridCachingProvider(
        IOptions<HybridCachingProviderOptions> options,
        ILogger<HybridCachingProvider> logger,
        IServiceProvider serviceProvider) : BaseCachingProvider
    {
        private readonly InMemoryCachingProvider _inMemoryCachingProvider
            = (InMemoryCachingProvider) serviceProvider.GetRequiredKeyedService<ICachingProvider>(options.Value.InMemory.Name);

        private readonly SQLiteCachingProvider _sqliteCachingProvider
            = (SQLiteCachingProvider) serviceProvider.GetRequiredKeyedService<ICachingProvider>(options.Value.SQLite.Name);

        /// <inheritdoc />
        public override string Name => options.Value.Name;

        /// <inheritdoc />
        public override async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            this._inMemoryCachingProvider.Evicted += async (sender, args)
                => await this._sqliteCachingProvider.SetAsync(args.Key, args.Value, expiration: null, cancellationToken);

            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public override async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
            => await this._inMemoryCachingProvider.SetAsync<T>(key, value, expiration, cancellationToken);

        /// <inheritdoc />
        public override async Task<CacheValue<T>> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            CacheValue<T> value = await this._inMemoryCachingProvider.GetAsync<T>(key, cancellationToken);
            if(value.HasValue)
            {
                if(options.Value.EnableLogging)
                {
                    logger.CacheHit(this.Name, key);
                }
                return value;
            }

            value = await this._sqliteCachingProvider.GetAsync<T>(key, cancellationToken);
            if(value.HasValue)
            {
                if(options.Value.EnableLogging)
                {
                    logger.CacheHit(this.Name, key);
                }
                return value;
            }

            if(options.Value.EnableLogging)
            {
                logger.CacheMiss(this.Name, key);
            }

            return CacheValue<T>.NoValue;
        }

        /// <inheritdoc />
        public override async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await this._inMemoryCachingProvider.RemoveAsync(key, cancellationToken);
            await this._sqliteCachingProvider.RemoveAsync(key, cancellationToken);
        }

        /// <inheritdoc />
        public override async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default) =>
            await this._inMemoryCachingProvider.ExistsAsync(key, cancellationToken) ||
            await this._sqliteCachingProvider.ExistsAsync(key, cancellationToken);
    }
}