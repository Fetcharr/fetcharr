using Fetcharr.Models.Configuration.Radarr;

namespace Fetcharr.Provider.Radarr.Models
{
    /// <summary>
    ///   Representation of a Radarr movie.
    /// </summary>
    public class RadarrMovie
    {
        /// <summary>
        ///   Gets or sets the ID of the movie, within Radarr.
        ///   If <see langword="null" />, the movie does not yet exist in Radarr.
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        ///   Gets or sets the title of the movie.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the release year of the movie.
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        ///   Gets or sets the status of the movie.
        /// </summary>
        public RadarrMovieStatus Status { get; set; }

        /// <summary>
        ///   Gets or sets the certification of the movie.
        /// </summary>
        public string Certification { get; set; } = string.Empty;

        /// <summary>
        ///   Gets or sets the genres of the movie.
        /// </summary>
        public List<string> Genres { get; set; } = [];

        /// <summary>
        ///   Gets or sets the folder to store the movie in.
        /// </summary>
        public string Folder { get; set; } = string.Empty;

        /// <summary>
        ///   Gets whether this movie is still in production.
        /// </summary>
        public bool IsInProduction => this.Genres.Count == 0 || string.IsNullOrEmpty(this.Certification);
    }
}