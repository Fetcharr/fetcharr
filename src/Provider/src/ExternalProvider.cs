using Fetcharr.Provider.Exceptions;

namespace Fetcharr.Provider
{
    /// <summary>
    ///   Base class for external API providers.
    /// </summary>
    public abstract class ExternalProvider
    {
        /// <summary>
        ///   Gets the name of the provider.
        /// </summary>
        public abstract string ProviderName { get; }

        /// <summary>
        ///   Pings the provider to verify the connection.
        /// </summary>
        /// <exception cref="ExternalProviderUnreachableException">Thrown if the ping failed.</exception>
        public abstract Task PingAsync(CancellationToken cancellationToken = default);
    }
}