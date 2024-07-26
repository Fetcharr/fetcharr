using Flurl.Http;
using Flurl.Http.Configuration;

using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Shared.Http.Extensions
{
    public static class IServieCollectionExtensions
    {
        public static IServiceCollection AddFlurlErrorHandler(this IServiceCollection services)
        {
            services.AddSingleton<IFlurlErrorLogger, FlurlErrorLogger>();

            services.AddSingleton<IFlurlClientCache>(sp => new FlurlClientCache()
                .WithDefaults(builder =>
                    builder.EventHandlers.Add((FlurlEventType.OnError, sp.GetService<IFlurlErrorLogger>()))));

            return services;
        }
    }
}