using Fetcharr.Cache.Core;

namespace Fetcharr.Cache.SQLite
{
    /// <summary>
    ///   Options for the SQLite caching provider, <see cref="SQLiteCachingProvider"/>.
    /// </summary>
    public class SQLiteCachingProviderOptions(string name) : BaseCachingProviderOptions(name)
    {
        /// <summary>
        ///   Gets or sets the path for the SQLite database file.
        /// </summary>
        public string DatabasePath { get; set; } = $"{name}.sqlite";
    }
}