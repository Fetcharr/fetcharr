using Fetcharr.Cache.Core;
using Fetcharr.Cache.InMemory.Extensions;
using Fetcharr.Cache.SQLite.Extensions;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.Hybrid.Extensions
{
    public static partial class CachingProviderOptionsExtensions
    {
        /// <summary>
        ///   Use the hybrid caching provider as an available cache.
        /// </summary>
        /// <param name="options"><see cref="CachingProviderOptions"/>-instance to attach the provider onto.</param>
        /// <param name="name">Identifiable name of the caching provider. Must be unique.</param>
        public static CachingProviderOptions UseHybrid(this CachingProviderOptions options, string name)
            => options.UseHybrid(name, _ => {});

        /// <inheritdoc cref="UseHybrid(CachingProviderOptions, string)" />
        /// <param name="configure"></param>
        public static CachingProviderOptions UseHybrid(
            this CachingProviderOptions options,
            string name,
            Action<HybridCachingProviderOptions> configure)
        {
            HybridCachingProviderOptions providerOptions = new(name);
            configure(providerOptions);

            options.UseInMemory(providerOptions.InMemory.Name, providerOptions.InMemory);
            options.UseSQLite(providerOptions.SQLite.Name, providerOptions.SQLite);

            options.Services.AddSingleton(_ => Options.Create(providerOptions));
            options.Services.AddSingleton<ICachingProvider, HybridCachingProvider>();
            options.Services.AddKeyedSingleton<ICachingProvider, HybridCachingProvider>(name);

            return options;
        }
    }
}