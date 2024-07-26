using Fetcharr.Models.Configuration;

using YamlDotNet.Serialization;

namespace Fetcharr.API.Configuration
{
    /// <summary>
    ///   Parser for reading the YAML config of Fetcharr into memory.
    /// </summary>
    public class ConfigurationParser
    {
        /// <summary>
        ///   Read and parse the YAML configuration file, located at <paramref name="path"/>, and return it.
        /// </summary>
        /// <param name="path">Path to the YAML configuration file.</param>
        public FetcharrConfiguration ReadConfig(string path)
        {
            using StreamReader configContent = File.OpenText(path);
            string config = configContent.ReadToEnd();

            IDeserializer deserializer = new DeserializerBuilder()
                .WithNodeDeserializer(new ServiceInstanceNodeDeserializer(), syntax => syntax.OnTop())
                .Build();

            return deserializer.Deserialize<FetcharrConfiguration>(config);
        }
    }
}