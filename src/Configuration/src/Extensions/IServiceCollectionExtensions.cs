using Fetcharr.Configuration.Parsing;
using Fetcharr.Configuration.Secrets;
using Fetcharr.Models.Configuration;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fetcharr.Configuration.Extensions
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        ///   Registers configuration services onto the given <see cref="IServiceCollection"/>.
        /// </summary>
        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            services.AddScoped<IConfigurationLocator, ConfigurationLocator>();
            services.AddScoped<IConfigurationParser, ConfigurationParser>();
            services.AddScoped<ISecretsProvider, SecretsProvider>();

            services.AddTransient<IOptions<FetcharrConfiguration>>(provider =>
            {
                IConfigurationParser parser = provider.GetRequiredService<IConfigurationParser>();
                FetcharrConfiguration configuration = parser.ReadConfig();

                return Options.Create(configuration);
            });

            return services;
        }
    }
}