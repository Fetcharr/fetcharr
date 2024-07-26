using System.Text.Json.Serialization;

namespace Fetcharr.Provider.Sonarr.Models
{
    /// <summary>
    ///   Enumeration of available Sonarr series statuses.
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SonarrSeriesStatus
    {
        /// <summary>
        ///   The series has been partly been released, with more to come.
        /// </summary>
        Continuing,

        /// <summary>
        ///   The series has ended.
        /// </summary>
        Ended,

        /// <summary>
        ///   The series has not yet made it's debut.
        /// </summary>
        Upcoming,
    }
}