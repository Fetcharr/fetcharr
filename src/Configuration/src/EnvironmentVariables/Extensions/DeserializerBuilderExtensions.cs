using Microsoft.Extensions.DependencyInjection;

using YamlDotNet.Serialization;

namespace Fetcharr.Configuration.EnvironmentVariables.Extensions
{
    public static partial class DeserializerBuilderExtensions
    {
        public static DeserializerBuilder WithEnvironmentVariables(
            this DeserializerBuilder builder,
            IServiceProvider provider) => builder
                .WithNodeDeserializer(ActivatorUtilities.CreateInstance<EnvironmentVariableDeserializer>(provider))
                .WithTagMapping("!env_var", typeof(EnvironmentVariableValue));
    }
}