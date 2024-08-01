using Fetcharr.API.Extensions;
using Fetcharr.Provider;

using Microsoft.Extensions.DependencyInjection;

namespace Fetcharr.Testing.Layers
{
    /// <summary>
    ///   Base layer for testing Fetcharr services.
    /// </summary>
    /// <typeparam name="TService">Type of service to test.</typeparam>
    public abstract class BaseServiceTestingLayer<TService>
        where TService : ExternalProvider
    {
        /// <summary>
        ///   Gets a <see cref="IServiceProvider" />-instance with all Fetcharr services registered.
        /// </summary>
        private readonly IServiceProvider _provider = new ServiceCollection()
            .AddFetcharr()
            .BuildServiceProvider();

        /// <summary>
        ///   Creates a service of type <typeparamref name="T" />,
        ///   with optional constructor arguments from <paramref name="parameters"/>.
        /// </summary>
        protected T CreateService<T>(params object[] parameters)
            => ActivatorUtilities.CreateInstance<T>(this._provider, parameters);
    }
}