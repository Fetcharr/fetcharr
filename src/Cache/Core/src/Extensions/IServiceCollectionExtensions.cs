using Fetcharr.Cache.Core.Services;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.Core.Extensions
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        ///   Add caching to the given <see cref="IServiceCollection"/>-instance.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>-instance to register caching providers onto.</param>
        /// <param name="configure">Action for configuring caching providers.</param>
        public static IServiceCollection AddCaching(
            this IServiceCollection services,
            Action<CachingProviderOptions> configure)
        {
            CachingProviderOptions options = new(services);
            configure(options);

            services.AddLogging(builder => builder.AddConsole());
            services.AddSingleton(Options.Create(options));

            services.AddHostedService<CacheInitializationService>();
            services.AddHostedService<CacheEvictionService>();

            return services;
        }

        /// <summary>
        ///   Add caching to the given <see cref="IServiceCollection"/>-instance.
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/>-instance to register caching providers onto.</param>
        public static IServiceCollection AddCaching(this IServiceCollection services)
            => services.AddCaching(_ => {});
    }
}