namespace Fetcharr.Testing.Containers.Sonarr
{
    /// <inheritdoc cref="ContainerConfiguration" />
    public sealed class SonarrConfiguration : ContainerConfiguration
    {
        /// <summary>
        ///   Gets the Sonarr API key.
        /// </summary>
        public string ApiKey { get; } = null!;

        /// <summary>
        ///   Initializes a new instance of the <see cref="SonarrConfiguration" /> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public SonarrConfiguration(string apiKey = null!)
        {
            this.ApiKey = apiKey;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SonarrConfiguration" /> class.
        /// </summary>
        /// <param name="resourceConfiguration">The Docker resource configuration.</param>
        public SonarrConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
            : base(resourceConfiguration)
        {

        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SonarrConfiguration" /> class.
        /// </summary>
        /// <param name="resourceConfiguration">The Docker resource configuration.</param>
        public SonarrConfiguration(IContainerConfiguration resourceConfiguration)
            : base(resourceConfiguration)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SonarrConfiguration" /> class.
        /// </summary>
        /// <param name="resourceConfiguration">The Docker resource configuration.</param>
        public SonarrConfiguration(SonarrConfiguration resourceConfiguration)
            : this(new SonarrConfiguration(), resourceConfiguration)
        {
            // Passes the configuration upwards to the base implementations to create an updated immutable copy.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SonarrConfiguration" /> class.
        /// </summary>
        /// <param name="oldValue">The old Docker resource configuration.</param>
        /// <param name="newValue">The new Docker resource configuration.</param>
        public SonarrConfiguration(SonarrConfiguration oldValue, SonarrConfiguration newValue)
            : base(oldValue, newValue)
        {
            this.ApiKey = BuildConfiguration.Combine(oldValue.ApiKey, newValue.ApiKey);
        }
    }
}