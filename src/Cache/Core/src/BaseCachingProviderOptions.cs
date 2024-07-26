namespace Fetcharr.Cache.Core
{
    /// <summary>
    ///   Base class for all caching provider options.
    /// </summary>
    public abstract class BaseCachingProviderOptions(string name)
    {
        /// <summary>
        ///   Gets or sets the name of the cache.
        /// </summary>
        /// <remarks>
        ///   Used for identification - does not have to be unique.
        /// </remarks>
        public string Name { get; init; } = name;

        /// <summary>
        ///   Gets or sets whether to log cache-hits and -misses to console output.
        /// </summary>
        public bool EnableLogging { get; set; } = false;

        /// <summary>
        ///   Gets or sets the random value added onto expiration times, in seconds.
        /// </summary>
        /// <remarks>
        ///   Used to prevent Cache Crash.
        /// </remarks>
        /// <seealso href="https://github.com/dotnetcore/EasyCaching/blob/8ef13b75266508f074c932d7eb608a504c4900a6/src/EasyCaching.Core/Configurations/BaseProviderOptions.cs#L8-L16" />
        public int MaxExpirationDilation { get; set; } = 120;

        /// <summary>
        ///   Gets or sets the default expiration, when none is explicitly set.
        /// </summary>
        public TimeSpan DefaultExpiration { get; set; } = TimeSpan.FromHours(4);
    }
}