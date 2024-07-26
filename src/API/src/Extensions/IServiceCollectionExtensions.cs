using Fetcharr.API.Services;
using Fetcharr.Provider.Plex.Extensions;
using Fetcharr.Provider.Radarr.Extensions;
using Fetcharr.Provider.Sonarr.Extensions;

namespace Fetcharr.API.Extensions
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        ///   Registers Plex services onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddPlexServices(this IServiceCollection services)
        {
            services.AddPlexClient();
            services.AddHostedService<WatchlistSyncService>();

            return services;
        }

        /// <summary>
        ///   Registers Radarr services onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddRadarrServices(this IServiceCollection services)
        {
            services.AddRadarrClient();
            services.AddSingleton<RadarrMovieQueue>();
            services.AddHostedService<RadarrMovieService>();

            return services;
        }

        /// <summary>
        ///   Registers Sonarr services onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddSonarrServices(this IServiceCollection services)
        {
            services.AddSonarrClient();
            services.AddSingleton<SonarrSeriesQueue>();
            services.AddHostedService<SonarrSeriesService>();

            return services;
        }

        /// <summary>
        ///   Registers pinging services onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddPingingServices(this IServiceCollection services)
        {
            services.AddHostedService<ProviderPingService>();

            return services;
        }
    }
}