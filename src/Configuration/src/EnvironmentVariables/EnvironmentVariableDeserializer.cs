using Fetcharr.Configuration.EnvironmentVariables.Exceptions;
using Fetcharr.Models;

using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace Fetcharr.Configuration.EnvironmentVariables
{
    public record EnvironmentVariableValue;

    public class EnvironmentVariableDeserializer(
        IEnvironment environment)
        : INodeDeserializer
    {
        public bool Deserialize(
            IParser reader,
            Type expectedType,
            Func<IParser, Type, object?> nestedObjectDeserializer,
            out object? value,
            ObjectDeserializer rootDeserializer)
        {
            if(expectedType != typeof(EnvironmentVariableValue))
            {
                value = null;
                return false;
            }

            Scalar scalar = reader.Consume<Scalar>();
            string[] segments = scalar.Value.Trim().Split(' ', count: 2);

            string environmentVariableName = segments[0];
            string? environmentVariableDefault = segments.ElementAtOrDefault(1)?.Trim();
            string? environmentVariableValue = environment.GetEnvironmentVariable(environmentVariableName);

            if(string.IsNullOrEmpty(environmentVariableValue))
            {
                environmentVariableValue = environmentVariableDefault;
            }

            value = environmentVariableValue ?? throw new EnvironmentVariableNotFoundException(environmentVariableName);
            return true;
        }
    }
}