using Fetcharr.Models.Configuration;

namespace Fetcharr.Configuration.Parsing
{
    /// <summary>
    ///   Defines a mechanism for locating configuration files.
    /// </summary>
    public interface IConfigurationLocator
    {
        /// <summary>
        ///   Locates the configuration file of name <paramref name="name"/> and returns it, if found; otherwise, <see langword="null" />
        /// </summary>
        /// <param name="name">Name of the configuration file. Can be with or without extension.</param>
        FileInfo? Get(string name);

        /// <summary>
        ///   Locate and return all files within the default, or configured, configuration directory.
        /// </summary>
        /// <exception cref="DirectoryNotFoundException">Thrown if the configuration directory is unreachable or doesn't exist.</exception>
        IEnumerable<FileInfo> GetAll();
    }

    /// <summary>
    ///   Default implementation of the <see cref="IConfigurationLocator" /> interface.
    /// </summary>
    public class ConfigurationLocator(
        IAppDataSetup appDataSetup)
        : IConfigurationLocator
    {
        private readonly string[] _configSearchPatterns = ["*.yml", "*.yaml"];

        /// <inheritdoc />
        public FileInfo? Get(string name)
        {
            name = Path.GetFileNameWithoutExtension(name);

            foreach(FileInfo file in this.GetAll())
            {
                if(Path.GetFileNameWithoutExtension(file.Name).Equals(name, StringComparison.InvariantCultureIgnoreCase))
                {
                    return file;
                }
            }

            return null;
        }

        /// <inheritdoc />
        public IEnumerable<FileInfo> GetAll()
        {
            DirectoryInfo directory = new(appDataSetup.ConfigDirectory);
            if(!directory.Exists)
            {
                throw new DirectoryNotFoundException($"Configuration directory could not be found: '{appDataSetup.ConfigDirectory}'");
            }

            return this._configSearchPatterns.SelectMany(directory.EnumerateFiles);
        }
    }
}