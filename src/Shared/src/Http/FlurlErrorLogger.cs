using Flurl.Http;

using Microsoft.Extensions.Logging;

namespace Fetcharr.Shared.Http
{
    public interface IFlurlErrorLogger : IFlurlEventHandler
    {

    }

    public class FlurlErrorLogger(ILogger logger)
        : FlurlEventHandler
        , IFlurlErrorLogger
    {
        public override async Task HandleAsync(FlurlEventType eventType, FlurlCall call)
        {
            logger.LogError("Failed API call to Sonarr: {Error}", await call.Response.GetStringAsync());
        }
    }
}