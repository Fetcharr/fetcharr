using Fetcharr.Models.Configuration;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using YamlDotNet.Serialization;

namespace Fetcharr.Configuration.Parsing
{
    /// <summary>
    ///   Represents a merger for configuration files.
    /// </summary>
    /// <param name="deserializer">Instance of the YAML deserializer to use.</param>
    public class ConfigurationMerger(IDeserializer deserializer)
    {
        private readonly List<FetcharrConfiguration> _configs = [];

        /// <summary>
        ///   Add a partial configuration file to the merger.
        /// </summary>
        public ConfigurationMerger AddConfig(FileInfo file)
        {
            using StreamReader content = file.OpenText();
            return this.AddConfig(content.ReadToEnd());
        }

        /// <summary>
        ///   Add partial configuration file YAML to the merger.
        /// </summary>
        public ConfigurationMerger AddConfig(string yaml)
            => this.AddConfig(deserializer.Deserialize<FetcharrConfiguration>(yaml));

        /// <summary>
        ///   Add a partial <see cref="FetcharrConfiguration" />-instance to the merger.
        /// </summary>
        public ConfigurationMerger AddConfig(FetcharrConfiguration config)
        {
            this._configs.Add(config);
            return this;
        }

        /// <summary>
        ///   Merge all the added configurations into a single <see cref="FetcharrConfiguration" />-instance.
        /// </summary>
        public FetcharrConfiguration Merge()
        {
            JObject destination = [];

            JsonMergeSettings mergeSettings = new()
            {
                MergeArrayHandling = MergeArrayHandling.Union,
                MergeNullValueHandling = MergeNullValueHandling.Merge,
            };

            foreach(FetcharrConfiguration configuration in this._configs)
            {
                JObject configObject = JObject.Parse(JsonConvert.SerializeObject(configuration));

                destination.Merge(configObject, mergeSettings);
            }

            return destination.ToObject<FetcharrConfiguration>()!;
        }
    }
}