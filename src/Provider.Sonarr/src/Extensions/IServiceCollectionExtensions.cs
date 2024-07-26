using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Provider.Sonarr.Extensions
{
    public static class IServieCollectionExtensions
    {
        /// <summary>
        ///   Registers Sonarr services onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddSonarrClient(this IServiceCollection services)
        {
            services.AddSingleton<SonarrClientCollection>();

            return services;
        }
    }
}