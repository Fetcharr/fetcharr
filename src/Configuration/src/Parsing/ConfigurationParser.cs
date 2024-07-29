using Fetcharr.Configuration.EnvironmentVariables.Extensions;
using Fetcharr.Configuration.Secrets.Extensions;
using Fetcharr.Configuration.Validation;
using Fetcharr.Models.Configuration;

using Microsoft.Extensions.Logging;

using YamlDotNet.Serialization;

namespace Fetcharr.Configuration.Parsing
{
    /// <summary>
    ///   Interface for reading and parsing configuration files for Fetcharr.
    /// </summary>
    public interface IConfigurationParser
    {
        /// <summary>
        ///   Read the configuration file for Fetcharr.
        /// </summary>
        /// <param name="force">Whether to skip the cache and read the configuration file anyway.</param>
        FetcharrConfiguration ReadConfig(bool force = false);
    }

    /// <summary>
    ///   Default implementation for <see cref="IConfigurationParser" />.
    /// </summary>
    public class ConfigurationParser(
        IConfigurationLocator configurationLocator,
        IValidationPipeline validationPipeline,
        IServiceProvider serviceProvider,
        ILogger<ConfigurationParser> logger)
        : IConfigurationParser
    {
        private FetcharrConfiguration? cachedConfiguration { get; set; } = null;

        private readonly string[] _reservedConfigFiles = ["secrets"];

        public FetcharrConfiguration ReadConfig(bool force = false)
        {
            if(!force && this.cachedConfiguration is not null)
            {
                return this.cachedConfiguration;
            }

            foreach(FileInfo configFile in configurationLocator.GetAll())
            {
                logger.LogTrace("Found configuration file: {Path}", configFile.FullName);
            }

            IDeserializer deserializer = new DeserializerBuilder()
                .WithNodeDeserializer(new ServiceInstanceNodeDeserializer())
                .WithEnvironmentVariables(serviceProvider)
                .WithSecrets(serviceProvider)
                .Build();

            ConfigurationMerger configurationMerger = new(deserializer);

            FileInfo? fetcharrYamlConfig = configurationLocator.Get("fetcharr");
            if(fetcharrYamlConfig is null)
            {
                logger.LogWarning("No configuration file found.");
                return new FetcharrConfiguration();
            }

            configurationMerger.AddConfig(fetcharrYamlConfig);

            foreach(ConfigurationInclude include in configurationMerger.Merge().Includes)
            {
                if(string.IsNullOrEmpty(include.Config))
                {
                    logger.LogWarning("Invalid include statement: empty `config` property.");
                    continue;
                }

                FileInfo configFileInfo = configurationLocator.Get(include.Config)
                    ?? throw new FileNotFoundException(
                        $"Configuration file was included, but not found: '{include.Config}'");

                if(this.IsConfigurationFileReserved(configFileInfo))
                {
                    logger.LogWarning(
                        "Skipping configuration include: '{Path}' is reserved and cannot be included.",
                        include.Config);

                    continue;
                }

                configurationMerger.AddConfig(configFileInfo);
            }

            FetcharrConfiguration config = configurationMerger.Merge();
            if(!validationPipeline.Validate(config))
            {
                throw new InvalidDataException("Failed to load configuration file. See above for errors.");
            }

            this.cachedConfiguration = config;
            return this.cachedConfiguration;
        }

        /// <summary>
        ///   Returns whether the given file is reserved.
        /// </summary>
        private bool IsConfigurationFileReserved(FileInfo file) =>
            this._reservedConfigFiles.Any(r => r.Equals(
                Path.GetFileNameWithoutExtension(file.Name),
                StringComparison.InvariantCultureIgnoreCase));
    }
}