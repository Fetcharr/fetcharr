using Fetcharr.Configuration.EnvironmentVariables.Exceptions;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Fetcharr.Configuration.Secrets
{
    public record SecretsValue;

    public class SecretsDeserializer(
        ISecretsProvider secretsProvider)
        : INodeDeserializer
    {
        public bool Deserialize(
            IParser reader,
            Type expectedType,
            Func<IParser, Type, object?> nestedObjectDeserializer,
            out object? value,
            ObjectDeserializer rootDeserializer)
        {
            if(expectedType != typeof(SecretsValue))
            {
                value = null;
                return false;
            }

            Scalar scalar = reader.Consume<Scalar>();
            string secretName = scalar.Value;

            if(secretsProvider.Secrets.TryGetValue(secretName, out string? secretValue))
            {
                value = secretValue;
                return true;
            }

            throw new SecretValueNotFoundException(secretName);
        }
    }
}