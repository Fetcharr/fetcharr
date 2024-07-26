namespace Fetcharr.Provider.Sonarr.Models
{
    /// <summary>
    ///   Representation of a Sonarr root folder.
    /// </summary>
    public class SonarrRootFolder
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