using Microsoft.Extensions.DependencyInjection;

using YamlDotNet.Serialization;

namespace Fetcharr.Configuration.Secrets.Extensions
{
    public static partial class DeserializerBuilderExtensions
    {
        public static DeserializerBuilder WithSecrets(
            this DeserializerBuilder builder,
            IServiceProvider provider) => builder
                .WithNodeDeserializer(ActivatorUtilities.CreateInstance<SecretsDeserializer>(provider))
                .WithTagMapping("!secret", typeof(SecretsValue));
    }
}