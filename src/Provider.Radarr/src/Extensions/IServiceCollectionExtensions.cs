using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Provider.Radarr.Extensions
{
    public static class IServieCollectionExtensions
    {
        /// <summary>
        ///   Registers Radarr services onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddRadarrClient(this IServiceCollection services)
        {
            services.AddSingleton<RadarrClientCollection>();

            return services;
        }
    }
}