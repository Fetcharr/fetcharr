using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Provider.Plex.Extensions
{
    public static class IServieCollectionExtensions
    {
        /// <summary>
        ///   Registers Plex services onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddPlexClient(this IServiceCollection services)
        {
            services.AddSingleton<PlexClient>();
            services.AddSingleton<PlexMetadataClient>();
            services.AddSingleton<PlexWatchlistClient>();

            return services;
        }
    }
}