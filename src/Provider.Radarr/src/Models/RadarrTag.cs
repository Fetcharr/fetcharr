namespace Fetcharr.Provider.Radarr.Models
{
    /// <summary>
    ///   Representation of a Radarr tag.
    /// </summary>
    public class RadarrTag
    {
        /// <summary>
        ///   Gets or sets the ID of the tag, within Radarr.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///   Gets or sets the label of the tag.
        /// </summary>
        public string Label { get; set; } = string.Empty;
    }
}