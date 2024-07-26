namespace Fetcharr.Provider.Radarr.Models
{
    /// <summary>
    ///   Representation of a Radarr quality profile.
    /// </summary>
    public class RadarrQualityProfile
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