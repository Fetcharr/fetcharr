using Fetcharr.API.Extensions;
using Fetcharr.Provider;
using Fetcharr.Shared.Http.Extensions;

using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Testing.Services
{
    public abstract class BaseServiceTestingLayer<TService>
        where TService : ExternalProvider
    {
        private readonly IServiceProvider _provider = new ServiceCollection()
            .AddLogging()
            .AddPlexServices()
            .AddSonarrServices()
            .AddRadarrServices()
            .AddPingingServices()
            .AddFlurlErrorHandler()
            .BuildServiceProvider();

        protected T CreateService<T>(params object[] parameters)
            => ActivatorUtilities.CreateInstance<T>(this._provider, parameters);
    }
}