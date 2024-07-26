namespace Fetcharr.Provider.Radarr.Models
{
    /// <summary>
    ///   Representation of a Radarr root folder.
    /// </summary>
    public class RadarrRootFolder
    {
        /// <summary>
        ///   Gets or sets the unique ID of the root folder.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///   Gets or sets the absolute path of the root folder.
        /// </summary>
        public string Path { get; set; } = string.Empty;
    }
}