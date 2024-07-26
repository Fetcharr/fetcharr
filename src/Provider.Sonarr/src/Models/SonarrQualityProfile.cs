namespace Fetcharr.Provider.Sonarr.Models
{
    /// <summary>
    ///   Representation of a Sonarr quality profile.
    /// </summary>
    public class SonarrQualityProfile
    {
        /// <summary>
        ///   Gets or sets the unique ID of the quality profile.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        ///   Gets or sets the name of the quality profile
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}