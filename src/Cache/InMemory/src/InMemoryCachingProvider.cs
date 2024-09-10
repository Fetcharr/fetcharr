using System.Collections.Concurrent;

using Fetcharr.Cache.Core;
using Fetcharr.Cache.Core.Logging;
using Fetcharr.Cache.InMemory.Models;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.InMemory
{
    /// <summary>
    ///   Provider implementation for in-memory caching.
    /// </summary>
    public class InMemoryCachingProvider(
        IOptions<InMemoryCachingProviderOptions> options,
        ILogger<InMemoryCachingProvider> logger)
        : BaseCachingProvider
    {
        /// <inheritdoc />
        public override string Name => options.Value.Name;

        /// <summary>
        ///   Event handler for when a cache item has been evicted.
        /// </summary>
        public event EventHandler<CacheEvictionEventArgs>? Evicted;

        /// <summary>
        ///   Gets or sets the current size of the cache, in items.
        /// </summary>
        /// <remarks>
        ///   Should only be interacted with using <see cref="Interlocked"/>!
        /// </remarks>
        private long CacheSize = 0;

        /// <summary>
        ///   Gets the underlying cache database.
        /// </summary>
        private readonly ConcurrentDictionary<string, InMemoryCacheItem> _database = [];

        /// <inheritdoc />
        public override async Task EvictExpiredAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<string> keysToRemove = this._database
                .ToArray()
                .Where(v => v.Value.ExpiresAt < DateTime.Now)
                .Select(v => v.Key);

            foreach(string keyToRemove in keysToRemove)
            {
                if(this._database.TryRemove(keyToRemove, out InMemoryCacheItem? removedValue))
                {
                    this.Evicted?.Invoke(this, new CacheEvictionEventArgs(keyToRemove, removedValue));
                    Interlocked.Decrement(ref this.CacheSize);
                }
            }

            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public override async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key, nameof(key));

            TimeSpan _expiration = expiration ?? options.Value.DefaultExpiration;

            if(options.Value.MaxExpirationDilation > 0)
            {
                TimeSpan dilationTime = TimeSpan.FromSeconds(Random.Shared.Next(options.Value.MaxExpirationDilation));
                _expiration += dilationTime;
            }

            if(this._database.TryGetValue(key, out InMemoryCacheItem? item))
            {
                item.Update(value, _expiration);
            }
            else
            {
                this._database[key] = new InMemoryCacheItem(value, _expiration);

                Interlocked.Increment(ref this.CacheSize);

                if(options.Value.SizeLimit > 0 && Interlocked.Read(ref this.CacheSize) >= options.Value.SizeLimit)
                {
                    int itemsToRemove = (int) (Interlocked.Read(ref this.CacheSize) - options.Value.SizeLimit);

                    IEnumerable<string> keysToRemove = this._database
                        .ToArray()
                        .OrderBy(v => v.Value.LastAccessTime)
                        .Take(itemsToRemove)
                        .Select(v => v.Key);

                    foreach(string keyToRemove in keysToRemove)
                    {
                        if(this._database.TryRemove(keyToRemove, out InMemoryCacheItem? removedValue))
                        {
                            this.Evicted?.Invoke(this, new CacheEvictionEventArgs(keyToRemove, removedValue));
                            Interlocked.Decrement(ref this.CacheSize);
                        }
                    }
                }
            }

            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public override async Task<CacheValue<T>> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key, nameof(key));

            if(this._database.TryGetValue(key, out InMemoryCacheItem? item))
            {
                if(item.ExpiresAt < DateTime.Now)
                {
                    this.Evicted?.Invoke(this, new CacheEvictionEventArgs(key, item.Value));
                    this._database.Remove(key, out _);

                    if(options.Value.EnableLogging)
                    {
                        logger.CacheMiss(this.Name, key);
                    }

                    return CacheValue<T>.NoValue;
                }

                if(options.Value.EnableLogging)
                {
                    logger.CacheHit(this.Name, key);
                }

                item.LastAccessTime = DateTime.Now.Ticks;
                return new CacheValue<T>((T?) item.Value, true);
            }

            return await Task.FromResult(CacheValue<T>.NoValue);
        }

        /// <inheritdoc />
        public override async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            if(this._database.TryRemove(key, out InMemoryCacheItem? item))
            {
                this.Evicted?.Invoke(this, new CacheEvictionEventArgs(key, item.Value));
                Interlocked.Decrement(ref this.CacheSize);
            }

            await Task.CompletedTask;
        }

        /// <inheritdoc />
        public override async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
            => await Task.FromResult(this._database.ContainsKey(key));

        private sealed class InMemoryCacheItem(object? value, TimeSpan expiration)
        {
            /// <summary>
            ///   Gets or sets the underlying cache item value.
            /// </summary>
            private object? value { get; set; } = value;

            /// <summary>
            ///   Gets the value within the cache item.
            /// </summary>
            public object? Value
            {
                get
                {
                    this.LastAccessTime = DateTime.Now.Ticks;
                    return this.value;
                }
            }

            /// <summary>
            ///   Expiration date for the cache item.
            /// </summary>
            public DateTime ExpiresAt { get; set; } = DateTime.Now + expiration;

            /// <summary>
            ///   Gets or sets the latest access time for the item, in ticks.
            /// </summary>
            public long LastAccessTime { get; set; } = DateTime.Now.Ticks;

            /// <summary>
            ///   Update the cache item to use the given values.
            /// </summary>
            /// <param name="value">New cache item value.</param>
            /// <param name="expiration">New cache item expiration.</param>
            public void Update(object? value, TimeSpan expiration)
            {
                this.value = value;
                this.ExpiresAt = DateTime.Now + expiration;
                this.LastAccessTime = DateTime.Now.Ticks;
            }
        }
    }
}