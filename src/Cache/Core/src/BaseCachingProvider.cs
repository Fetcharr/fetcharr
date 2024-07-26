namespace Fetcharr.Cache.Core
{
    /// <summary>
    ///   Base class for all caching providers.
    /// </summary>
    public abstract class BaseCachingProvider : ICachingProvider
    {
        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public virtual async Task InitializeAsync(CancellationToken cancellationToken = default)
            => await Task.CompletedTask;

        /// <inheritdoc />
        public virtual async Task PingAsync(CancellationToken cancellationToken = default)
            => await Task.CompletedTask;

        /// <inheritdoc />
        public virtual async Task FlushAsync(CancellationToken cancellationToken = default)
            => await Task.CompletedTask;

        /// <inheritdoc />
        public virtual async Task EvictExpiredAsync(CancellationToken cancellationToken = default)
            => await Task.CompletedTask;

        /// <inheritdoc />
        public abstract Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public abstract Task<CacheValue<T>> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public virtual async Task<T> GetAsync<T>(
            string key,
            Func<T> retriever,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default)
        {
            CacheValue<T> cacheValue = await this.GetAsync<T>(key, cancellationToken);
            if(cacheValue.HasValue)
            {
                return cacheValue.Value;
            }

            T value = retriever();
            await this.SetAsync(key, value, expiration, cancellationToken);

            return value;
        }

        /// <inheritdoc />
        public virtual async Task<T> GetAsync<T>(
            string key,
            Func<Task<T>> retriever,
            TimeSpan? expiration = null,
            CancellationToken cancellationToken = default)
        {
            CacheValue<T> cacheValue = await this.GetAsync<T>(key, cancellationToken);
            if(cacheValue.HasValue)
            {
                return cacheValue.Value;
            }

            T value = await retriever();
            await this.SetAsync(key, value, expiration, cancellationToken);

            return value;
        }

        /// <inheritdoc />
        public abstract Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        /// <inheritdoc />
        public abstract Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
    }
}