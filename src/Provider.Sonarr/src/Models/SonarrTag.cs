namespace Fetcharr.Provider.Sonarr.Models
{
    /// <summary>
    ///   Representation of a Sonarr tag.
    /// </summary>
    public class SonarrTag
    {
        /// <summary>
        ///   Gets or sets the ID of the tag, within Sonarr.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///   Gets or sets the label of the tag.
        /// </summary>
        public string Label { get; set; } = string.Empty;
    }
}