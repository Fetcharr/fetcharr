using Fetcharr.Cache.Core;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.InMemory.Extensions
{
    public static partial class CachingProviderOptionsExtensions
    {
        /// <summary>
        ///   Use the in-memory caching provider as an available cache.
        /// </summary>
        /// <param name="options"><see cref="CachingProviderOptions"/>-instance to attach the provider onto.</param>
        /// <param name="name">Identifiable name of the caching provider. Must be unique.</param>
        public static CachingProviderOptions UseInMemory(this CachingProviderOptions options, string name)
            => options.UseInMemory(name, _ => { });

        /// <inheritdoc cref="UseInMemory(CachingProviderOptions, string)" />
        public static CachingProviderOptions UseInMemory(
            this CachingProviderOptions options,
            string name,
            InMemoryCachingProviderOptions providerOptions)
        {
            options.Services.AddSingleton(_ => Options.Create(providerOptions));
            options.Services.AddSingleton<ICachingProvider>(sp => sp.GetRequiredKeyedService<ICachingProvider>(name));
            options.Services.AddKeyedSingleton<ICachingProvider, InMemoryCachingProvider>(name);

            return options;
        }

        /// <inheritdoc cref="UseInMemory(CachingProviderOptions, string)" />
        public static CachingProviderOptions UseInMemory(
            this CachingProviderOptions options,
            string name,
            Action<InMemoryCachingProviderOptions> configure)
        {
            InMemoryCachingProviderOptions providerOptions = new(name);
            configure(providerOptions);

            return options.UseInMemory(name, providerOptions);
        }
    }
}