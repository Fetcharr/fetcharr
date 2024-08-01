namespace Fetcharr.Testing.Containers.Radarr
{
    /// <inheritdoc cref="DockerContainer" />
    /// <summary>
    ///   Initializes a new instance of the <see cref="RadarrContainer" /> class.
    /// </summary>
    /// <param name="configuration">The container configuration.</param>
    public sealed class RadarrContainer(
        RadarrConfiguration configuration)
        : DockerContainer(configuration)
    {
        public readonly RadarrConfiguration Configuration = configuration;
    }
}