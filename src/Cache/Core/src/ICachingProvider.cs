namespace Fetcharr.Cache.Core
{
    /// <summary>
    ///   Contract for all base caching providers.
    /// </summary>
    public interface ICachingProvider
    {
        /// <summary>
        ///   Gets the name of the caching provider.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///   Perform initialization for the provider and it's dependencies.
        /// </summary>
        Task InitializeAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///   Ping the provider and ensure a valid connection.
        /// </summary>
        Task PingAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///   Flush all in-memory cache lines into provider storage.
        /// </summary>
        Task FlushAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///   Evict all expired cache items out of the provider.
        /// </summary>
        Task EvictExpiredAsync(CancellationToken cancellationToken = default);

        /// <summary>
        ///   Caches <paramref name="value"/> at the key <paramref name="key"/>, with an expiration of <paramref name="expiration"/>.
        /// </summary>
        /// <typeparam name="T">Type of value to cache.</typeparam>
        /// <param name="key">Caching key for the item.</param>
        /// <param name="value">Value to cache.</param>
        /// <param name="expiration">Expiration for the cache item. If not set, uses the cache's default expiration.</param>
        Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///   Gets the value stored at <paramref name="key"/>, if it exists.
        /// </summary>
        /// <typeparam name="T">Type of value to cache.</typeparam>
        /// <param name="key">Caching key for the item.</param>
        Task<CacheValue<T>> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        /// <summary>
        ///   Gets the value stored at <paramref name="key"/>, if it exists.
        ///   If not, retrieves the value from <paramref name="retriever"/> and updates the cache with the data.
        /// </summary>
        /// <typeparam name="T">Type of value to cache.</typeparam>
        /// <param name="key">Caching key for the item.</param>
        /// <param name="retriever">
        ///   If the cache doesn't contain <paramref name="key"/>, adds the value from this action to the cache,
        ///   with the key of <paramref name="key"/>.
        /// </param>
        /// <param name="expiration">
        ///   If the cache doesn't contain <paramref name="key"/>, determines the new cache item's expiration.
        /// </param>
        Task<T> GetAsync<T>(string key, Func<T> retriever, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        /// <inheritdoc cref="GetAsync{T}(string, Func{T}, TimeSpan?, CancellationToken)" />
        Task<T> GetAsync<T>(string key, Func<Task<T>> retriever, TimeSpan? expiration = null, CancellationToken cancellationToken = default);

        /// <summary>
        ///   Removes the value stored at <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Caching key for the item.</param>
        Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        /// <summary>
        ///   Returns whether a value exists at <paramref name="key"/>.
        /// </summary>
        /// <param name="key">Caching key for the item.</param>
        Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default);
    }
}