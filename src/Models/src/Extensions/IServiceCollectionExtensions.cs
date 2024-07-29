using Fetcharr.Models.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Models.Extensions
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        ///   Add the default <see cref="IEnvironment" />-implementation to the given <see cref="IServiceCollection"/>-instance.
        /// </summary>
        public static IServiceCollection AddDefaultEnvironment(this IServiceCollection services) =>
            services
                .AddScoped<IEnvironment, DefaultEnvironment>()
                .AddSingleton<IAppDataSetup, EnvironmentalAppDataSetup>();
    }
}