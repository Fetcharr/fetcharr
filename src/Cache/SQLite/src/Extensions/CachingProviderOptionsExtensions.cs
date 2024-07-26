using Fetcharr.Cache.Core;
using Fetcharr.Cache.SQLite.Contexts;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.SQLite.Extensions
{
    public static partial class CachingProviderOptionsExtensions
    {
        /// <summary>
        ///   Use the SQLite caching provider as an available cache.
        /// </summary>
        /// <param name="options"><see cref="CachingProviderOptions"/>-instance to attach the provider onto.</param>
        /// <param name="name">Identifiable name of the caching provider. Must be unique.</param>
        public static CachingProviderOptions UseSQLite(this CachingProviderOptions options, string name)
            => options.UseSQLite(name, _ => {});

        /// <inheritdoc cref="UseSQLite(CachingProviderOptions, string)" />
        public static CachingProviderOptions UseSQLite(
            this CachingProviderOptions options,
            string name,
            SQLiteCachingProviderOptions providerOptions)
        {
            options.Services.AddSingleton(_ => Options.Create(providerOptions));
            options.Services.AddSingleton<ICachingProvider>(sp => sp.GetRequiredKeyedService<ICachingProvider>(name));
            options.Services.AddKeyedSingleton<ICachingProvider, SQLiteCachingProvider>(name);

            options.Services.AddDbContextFactory<CacheContext>();

            return options;
        }

        /// <inheritdoc cref="UseSQLite(CachingProviderOptions, string)" />
        /// <param name="configure">Action for configuring the provider.</param>
        public static CachingProviderOptions UseSQLite(
            this CachingProviderOptions options,
            string name,
            Action<SQLiteCachingProviderOptions> configure)
        {
            SQLiteCachingProviderOptions providerOptions = new(name);
            configure(providerOptions);

            return options.UseSQLite(name, providerOptions);
        }
    }
}