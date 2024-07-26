using Fetcharr.Models.Extensions;

namespace Fetcharr.Models.Configuration
{
    /// <summary>
    ///   Represents a filter for a service, which can limit what items can get added to the service.
    /// </summary>
    public class ServiceFilter : List<string>
    {
        /// <summary>
        ///   Gets whether the given value is allowed through the filter.
        ///   If the filter is empty, it is always allowed.
        ///   Otherwise, it is only allowed if the filter contains <paramref name="value"/>.
        /// </summary>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <see langword="null"/>.</exception>
        public ServiceFilterAllowType IsAllowed(string value)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(value, nameof(value));

            if(this.Count == 0)
            {
                return ServiceFilterAllowType.ImplicitlyAllowed;
            }

            if(this.Count > 0 && this.Contains(value, StringComparer.InvariantCultureIgnoreCase))
            {
                return ServiceFilterAllowType.ExplicitlyAllowed;
            }

            return ServiceFilterAllowType.NotAllowed;
        }

        /// <inheritdoc cref="IsAllowed(string)" />
        public ServiceFilterAllowType IsAllowed(IEnumerable<string> values)
        {
            ArgumentNullException.ThrowIfNull(values, nameof(values));

            if(!values.Any())
            {
                throw new ArgumentException("values cannot be empty.", nameof(values));
            }

            if(this.Count == 0)
            {
                return ServiceFilterAllowType.ImplicitlyAllowed;
            }

            if(this.Count > 0 && this.ContainsAny(values, StringComparer.InvariantCultureIgnoreCase))
            {
                return ServiceFilterAllowType.ExplicitlyAllowed;
            }

            return ServiceFilterAllowType.NotAllowed;
        }

        /// <inheritdoc cref="IsAllowed(string)" />
        /// <returns>Numeric score of how "much" it's allowed.</returns>
        public int GetFilterScore(string value) =>
            this.IsAllowed(value) switch
            {
                ServiceFilterAllowType.ExplicitlyAllowed => 10,
                ServiceFilterAllowType.ImplicitlyAllowed => 5,
                _ or ServiceFilterAllowType.NotAllowed   => -100,
            };

        /// <inheritdoc cref="IsAllowed(IEnumerable{string})" />
        /// <returns>Numeric score of how "much" it's allowed.</returns>
        public int GetFilterScore(IEnumerable<string> values) =>
            this.IsAllowed(values) switch
            {
                ServiceFilterAllowType.ExplicitlyAllowed => 10,
                ServiceFilterAllowType.ImplicitlyAllowed => 5,
                _ or ServiceFilterAllowType.NotAllowed   => -100,
            };
    }
}