using System.Text.Json.Serialization;

namespace Fetcharr.Models.Configuration.Radarr
{
    /// <summary>
    ///   Enumeration of all available movie statuses in Radarr.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum RadarrMovieStatus
    {
        /// <summary>
        ///   The movie has no official trailer or release date.
        /// </summary>
        TBA,

        /// <summary>
        ///   The movie has been teased via trailer or given a release date.
        /// </summary>
        Announced,

        /// <summary>
        ///   The movie is currently in cinemas.
        /// </summary>
        InCinemas,

        /// <summary>
        ///   The movie has been released on physical media or on digital streaming services.
        /// </summary>
        Released,

        /// <summary>
        ///   This movie is no more.
        /// </summary>
        Deleted,
    }
}