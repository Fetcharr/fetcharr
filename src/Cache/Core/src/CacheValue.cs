namespace Fetcharr.Cache.Core
{
    public class CacheValue<T>(T? value, bool hasValue)
    {
        public static CacheValue<T> Null { get; } = new CacheValue<T>(default, true);

        public static CacheValue<T> NoValue { get; } = new CacheValue<T>(default, false);

        private readonly T? _value = value;

        /// <summary>
        ///   Gets the underlying value of the <see cref="CacheValue{T}"/>.
        /// </summary>
        /// <exception cref="InvalidDataException">Thrown if the <see cref="CacheValue{T}"/> has no value.</exception>
        public T Value
        {
            get
            {
                if(!this.HasValue)
                {
                    throw new InvalidDataException($"CacheValue<{nameof(T)}>.Value is null.");
                }

                return this._value ?? default!;
            }
        }

        /// <summary>
        ///   Gets whether <see cref="Value"/> has any value.
        /// </summary>
        public bool HasValue { get; } = hasValue;

        /// <summary>
        ///   Gets whether <see cref="Value"/> is <see langword="null"/>.
        /// </summary>
        public bool IsNull => this._value is null;
    }
}