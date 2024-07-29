using System.Collections.Frozen;

using Fetcharr.Configuration.Parsing;

using YamlDotNet.Serialization;

namespace Fetcharr.Configuration.Secrets
{
    /// <summary>
    ///   Represents a provider for reading secrets from the <c>secrets.yml</c> configuration file.
    /// </summary>
    public interface ISecretsProvider
    {
        /// <summary>
        ///   Gets all the secrets within the provider.
        /// </summary>
        FrozenDictionary<string, string> Secrets { get; }
    }

    /// <summary>
    ///   Default implementation of <see cref="ISecretsProvider" />.
    /// </summary>
    public class SecretsProvider : ISecretsProvider
    {
        private readonly IConfigurationLocator _configurationLocator;
        private readonly Lazy<Dictionary<string, string>> _secrets;

        public FrozenDictionary<string, string> Secrets => this._secrets.Value.ToFrozenDictionary();

        public SecretsProvider(IConfigurationLocator configurationLocator)
        {
            this._configurationLocator = configurationLocator;
            this._secrets = new(this.LoadSecrets);
        }

        private Dictionary<string, string> LoadSecrets()
        {
            FileInfo? secretsFile = this._configurationLocator.Get("secrets");
            if(secretsFile is null)
            {
                return [];
            }

            using StreamReader secretsStream = secretsFile.OpenText();
            return new DeserializerBuilder()
                .Build()
                .Deserialize<Dictionary<string, string>>(secretsStream);
        }
    }
}