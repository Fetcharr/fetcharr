namespace Fetcharr.Testing.Templates.Testcontainers.Radarr
{
    /// <inheritdoc cref="ContainerConfiguration" />
    public sealed class RadarrConfiguration : ContainerConfiguration
    {
        /// <summary>
        ///   Gets the Radarr API key.
        /// </summary>
        public string ApiKey { get; } = null!;

        /// <summary>
        ///   Initializes a new instance of the <see cref="RadarrConfiguration" /> class.
        /// </summary>
        /// <param name="apiKey">The API key.</param>
        public RadarrConfiguration(string apiKey = null!)
        {
            this.ApiKey = apiKey;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RadarrConfiguration" /> class.
        /// </summary>
        /// <param name="resourceConfiguration">The Docker resource configuration.</param>
        public RadarrConfiguration(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
            : base(resourceConfiguration)
        {

        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RadarrConfiguration" /> class.
        /// </summary>
        /// <param name="resourceConfiguration">The Docker resource configuration.</param>
        public RadarrConfiguration(IContainerConfiguration resourceConfiguration)
            : base(resourceConfiguration)
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadarrConfiguration" /> class.
        /// </summary>
        /// <param name="resourceConfiguration">The Docker resource configuration.</param>
        public RadarrConfiguration(RadarrConfiguration resourceConfiguration)
            : this(new RadarrConfiguration(), resourceConfiguration)
        {
            // Passes the configuration upwards to the base implementations to create an updated immutable copy.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RadarrConfiguration" /> class.
        /// </summary>
        /// <param name="oldValue">The old Docker resource configuration.</param>
        /// <param name="newValue">The new Docker resource configuration.</param>
        public RadarrConfiguration(RadarrConfiguration oldValue, RadarrConfiguration newValue)
            : base(oldValue, newValue)
        {
            this.ApiKey = BuildConfiguration.Combine(oldValue.ApiKey, newValue.ApiKey);
        }
    }
}