namespace Fetcharr.Models
{
    /// <summary>
    ///   Contract for interfacing with environment environment variables.
    /// </summary>
    public interface IEnvironment
    {
        /// <inheritdoc cref="Environment.GetEnvironmentVariable(string)" />
        string? GetEnvironmentVariable(string name);

        /// <inheritdoc cref="Environment.SetEnvironmentVariable(string, string?)" />
        void SetEnvironmentVariable(string name, string? value);
    }

    /// <summary>
    ///   Default implementation for <see cref="IEnvironment" />, which uses actual environment variables.
    /// </summary>
    public class DefaultEnvironment : IEnvironment
    {
        /// <inheritdoc />
        public string? GetEnvironmentVariable(string name)
            => Environment.GetEnvironmentVariable(name);

        /// <inheritdoc />
        public void SetEnvironmentVariable(string name, string? value)
            => Environment.SetEnvironmentVariable(name, value);
    }
}