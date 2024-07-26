using Microsoft.Extensions.Logging;

namespace Fetcharr.Cache.Core.Logging
{
    public static partial class CacheProviderLogging
    {
        [LoggerMessage(
            EventId = 0,
            Level = LogLevel.Information,
            Message = "Cache miss: cache={Cache}, key={Key}")]
        public static partial void CacheMiss(this ILogger logger, string cache, string key);

        [LoggerMessage(
            EventId = 1,
            Level = LogLevel.Information,
            Message = "Cache hit: cache={Cache}, key={Key}")]
        public static partial void CacheHit(this ILogger logger, string cache, string key);
    }
}