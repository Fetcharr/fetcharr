using Fetcharr.API.Extensions;
using Fetcharr.Provider;

using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Testing.Layers
{
    public abstract class BaseServiceTestingLayer<TService>
        where TService : ExternalProvider
    {
        private readonly IServiceProvider _provider = new ServiceCollection()
            .AddFetcharr()
            .BuildServiceProvider();

        protected T CreateService<T>(params object[] parameters)
            => ActivatorUtilities.CreateInstance<T>(this._provider, parameters);
    }
}