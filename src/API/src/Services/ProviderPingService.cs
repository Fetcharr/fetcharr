using Fetcharr.API.Pipeline;
using Fetcharr.Provider;
using Fetcharr.Provider.Exceptions;

namespace Fetcharr.API.Services
{
    /// <summary>
    ///   Hosted service for pinging external services and verifying the connection to them.
    /// </summary>
    public class ProviderPingService(
        IEnumerable<ExternalProvider> providers,
        ILogger<ProviderPingService> logger)
        : BasePeriodicService(TimeSpan.FromSeconds(15), logger)
    {
        public override async Task InvokeAsync(CancellationToken cancellationToken)
        {
            foreach(ExternalProvider provider in providers)
            {
                try
                {
                    await provider.PingAsync(cancellationToken);
                }
                catch(ExternalProviderUnreachableException ex)
                {
                    logger.LogError(
                        "Provider '{ProviderName}' is unreachable: {ExceptionMessage}",
                        ex.ProviderName,
                        ex.InnerException?.Message ?? ex.Message);
                }
                catch(Exception ex)
                {
                    logger.LogError(
                        ex, "Provider '{ProviderName}' is unreachable: {ExceptionMessage}",
                        provider.ProviderName,
                        ex.Message);
                }
            }
        }
    }
}