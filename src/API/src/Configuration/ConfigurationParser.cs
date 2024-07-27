using Fetcharr.Models.Configuration;

using YamlDotNet.Serialization;

namespace Fetcharr.API.Configuration
{
    /// <summary>
    ///   Parser for reading the YAML config of Fetcharr into memory.
    /// </summary>
    public class ConfigurationParser(IAppDataSetup appDataSetup)
    {
        /// <summary>
        ///   Read and parse the YAML configuration file and return it.
        /// </summary>
        public FetcharrConfiguration ReadConfig()
        {
            string configFilePath = appDataSetup.ConfigurationFilePath;
            if(!File.Exists(configFilePath))
            {
                throw new FileNotFoundException($"Configuration file '{configFilePath}' could not be found.");
            }

            using StreamReader configContent = File.OpenText(configFilePath);
            string config = configContent.ReadToEnd();

            IDeserializer deserializer = new DeserializerBuilder()
                .WithNodeDeserializer(new ServiceInstanceNodeDeserializer(), syntax => syntax.OnTop())
                .Build();

            return deserializer.Deserialize<FetcharrConfiguration>(config);
        }
    }
}