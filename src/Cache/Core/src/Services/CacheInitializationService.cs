using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fetcharr.Cache.Core.Services
{
    public class CacheInitializationService(
        ILogger<CacheInitializationService> logger,
        IServiceProvider services)
        : IHostedService
    {
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Initializing caches.");

            using IServiceScope scope = services.CreateScope();

            foreach(ICachingProvider provider in scope.ServiceProvider.GetServices<ICachingProvider>())
            {
                await provider.InitializeAsync(cancellationToken);
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            foreach(ICachingProvider provider in services.GetServices<ICachingProvider>())
            {
                await provider.FlushAsync(cancellationToken);
            }
        }
    }
}