namespace Fetcharr.Models.Configuration.Radarr
{
    /// <summary>
    ///   Enumeration of all available monitoring states for movies in Radarr.
    /// </summary>
    public enum RadarrMonitoredItems
    {
        /// <summary>
        ///   No monitoring.
        /// </summary>
        None,

        /// <summary>
        ///   Monitor only the movie.
        /// </summary>
        MovieOnly,

        /// <summary>
        ///   Monitor the entire movie collection.
        /// </summary>
        MovieAndCollection,
    }
}