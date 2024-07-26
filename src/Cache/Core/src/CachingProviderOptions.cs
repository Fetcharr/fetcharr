using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Cache.Core
{
    public class CachingProviderOptions(IServiceCollection services)
    {
        /// <summary>
        ///   Gets or sets the time between cache evictions, for keys that have expired.
        /// </summary>
        public TimeSpan EvictionPeriod { get; set; } = TimeSpan.FromMinutes(1);

        /// <summary>
        ///   Gets the <see cref="IServiceCollection"/>-instance for registering services.
        /// </summary>
        protected internal readonly IServiceCollection Services = services;
    }
}