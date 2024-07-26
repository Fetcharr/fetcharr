using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.Core.Services
{
    public class CacheEvictionService(
        ILogger<CacheEvictionService> logger,
        IServiceProvider services,
        IOptions<CachingProviderOptions> options)
        : IHostedService
        , IDisposable
    {
        private Timer? timer { get; set; }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Starting cache eviction service.");

            this.timer = new Timer(
                callback: async _ => await this.EvictCachesAsync(cancellationToken),
                state: null,
                dueTime: options.Value.EvictionPeriod,
                period: options.Value.EvictionPeriod);

            await Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Stopping cache eviction service.");
            this.timer?.Change(Timeout.Infinite, 0);

            await Task.CompletedTask;
        }

        private async Task EvictCachesAsync(CancellationToken cancellationToken)
        {
            if(cancellationToken.IsCancellationRequested)
            {
                return;
            }

            logger.LogInformation("Evicting expired cache entries...");

            foreach(ICachingProvider provider in services.GetServices<ICachingProvider>())
            {
                await provider.EvictExpiredAsync(cancellationToken);
            }
        }

        public void Dispose()
        {
            this.timer?.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}