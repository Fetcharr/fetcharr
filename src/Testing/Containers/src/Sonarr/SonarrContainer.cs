namespace Fetcharr.Testing.Containers.Sonarr
{
    /// <inheritdoc cref="DockerContainer" />
    /// <summary>
    ///   Initializes a new instance of the <see cref="SonarrContainer" /> class.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    public sealed class SonarrContainer(
        SonarrConfiguration configuration)
        : DockerContainer(configuration)
    {
        public readonly SonarrConfiguration Configuration = configuration;

        /// <summary>
        ///   Gets the base URL for the Sonarr endpoint.
        /// </summary>
        public string EndpointBase => string.Format(
            "http://{0}:{1}",
            this.Hostname,
            this.GetMappedPublicPort(SonarrBuilder.SonarrPort));
    }
}