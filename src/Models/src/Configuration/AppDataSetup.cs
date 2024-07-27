using static System.Environment;

namespace Fetcharr.Models.Configuration
{
    /// <summary>
    ///   Representation of app data directories.
    /// </summary>
    public interface IAppDataSetup
    {
        /// <summary>
        ///   Gets the base directory for all other Fetcharr data, unless explicitly overridden.
        /// </summary>
        string BaseDirectory { get; }

        /// <summary>
        ///   Gets the base location for caches.
        /// </summary>
        string CacheDirectory { get; }

        /// <summary>
        ///   Gets the base location for configurations.
        /// </summary>
        string ConfigDirectory { get; }

        /// <summary>
        ///   Gets the absolute path for the Fetcharr configuration file.
        /// </summary>
        string ConfigurationFilePath { get; }
    }

    /// <summary>
    ///   Default representation of app data directories, using environment variables for configuration.
    /// </summary>
    public class EnvironmentalAppDataSetup : IAppDataSetup
    {
        /// <inheritdoc />
        public string BaseDirectory => this.GetBaseDirectory();

        /// <inheritdoc />
        public string CacheDirectory =>
            Environment.GetEnvironmentVariable("FETCHARR_CACHE_DIR") ??
            Path.Join(this.BaseDirectory, "cache");

        /// <inheritdoc />
        public string ConfigDirectory =>
            Environment.GetEnvironmentVariable("FETCHARR_CONFIG_DIR") ??
            Path.Join(this.BaseDirectory, "config");

        /// <inheritdoc />
        public string ConfigurationFilePath =>
            Path.Join(this.ConfigDirectory, "config.yaml");

        public EnvironmentalAppDataSetup()
        {
            Directory.CreateDirectory(this.BaseDirectory);
            Directory.CreateDirectory(this.CacheDirectory);
            Directory.CreateDirectory(this.ConfigDirectory);
        }

        private string GetBaseDirectory()
        {
            string? basePath = Environment.GetEnvironmentVariable("FETCHARR_BASE_DIR");
            if(!string.IsNullOrEmpty(basePath))
            {
                return basePath;
            }

            basePath = Environment.GetFolderPath(SpecialFolder.LocalApplicationData, SpecialFolderOption.Create);
            if(!string.IsNullOrEmpty(basePath))
            {
                return Path.Join(basePath, "fetcharr");
            }

            throw new DirectoryNotFoundException(
                "Unable to find or create the base directory for Fetcharr app data. " +
                "Please set the FETCHARR_BASE_DIR environment variable to explicitly set a location.");
        }
    }
}