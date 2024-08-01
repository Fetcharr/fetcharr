
using Fetcharr.Testing.Templates.Extensions;

namespace Fetcharr.Testing.Templates.Testcontainers.Radarr
{
    /// <inheritdoc cref="ContainerBuilder{TBuilderEntity, TContainerEntity, TConfigurationEntity}" />
    public sealed class RadarrBuilder : ContainerBuilder<RadarrBuilder, RadarrContainer, RadarrConfiguration>
    {
        public const string RadarrImage = "lscr.io/linuxserver/radarr:5.8.3";

        public const ushort RadarrPort = 7878;

        public const string DefaultRadarrApiKey = "1b502b0fd9754b38b1b6570a8acdf6ad";

        /// <summary>
        ///   Initializes a new instance of the <see cref="RadarrBuilder" /> class.
        /// </summary>
        public RadarrBuilder()
            : this(new RadarrConfiguration())
        {
            this.DockerResourceConfiguration = this.Init().DockerResourceConfiguration;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="RadarrBuilder" /> class.
        /// </summary>
        /// <param name="resourceConfiguration">The Docker resource configuration.</param>
        private RadarrBuilder(RadarrConfiguration resourceConfiguration)
            : base(resourceConfiguration)
        {
            this.DockerResourceConfiguration = resourceConfiguration;
        }

        /// <inheritdoc />
        protected override RadarrConfiguration DockerResourceConfiguration { get; }

        /// <summary>
        ///   Sets the Radarr API key.
        /// </summary>
        /// <param name="apiKey">The Radarr API key.</param>
        /// <returns>A configured instance of <see cref="RadarrBuilder" />.</returns>
        public RadarrBuilder WithApiKey(string apiKey)
        {
            return this.Merge(this.DockerResourceConfiguration, new RadarrConfiguration(apiKey: apiKey))
                .WithEnvironment("Radarr__Auth__ApiKey", apiKey);
        }

        /// <inheritdoc />
        public override RadarrContainer Build()
        {
            this.Validate();
            return new RadarrContainer(this.DockerResourceConfiguration);
        }

        /// <inheritdoc />
        protected override RadarrBuilder Init() =>
            base.Init()
                .WithImage(RadarrImage)
                .WithPortBinding(RadarrPort, assignRandomHostPort: true)
                .WithApiKey(DefaultRadarrApiKey)
                .WithWaitStrategy(Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(request =>
                    request
                        .ForPath("/api/v3/health")
                        .ForPort(RadarrPort)
                        .WithHeader("X-Api-Key", this.DockerResourceConfiguration.ApiKey)));

        /// <inheritdoc />
        protected override void Validate()
        {
            base.Validate();

            _ = Guard.Argument(this.DockerResourceConfiguration.ApiKey, nameof(this.DockerResourceConfiguration.ApiKey))
                .NotNull()
                .NotEmpty()
                .HasLength(32);
        }

        /// <inheritdoc />
        protected override RadarrBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
            => this.Merge(this.DockerResourceConfiguration, new RadarrConfiguration(resourceConfiguration));

        /// <inheritdoc />
        protected override RadarrBuilder Clone(IContainerConfiguration resourceConfiguration)
            => this.Merge(this.DockerResourceConfiguration, new RadarrConfiguration(resourceConfiguration));

        /// <inheritdoc />
        protected override RadarrBuilder Merge(RadarrConfiguration oldValue, RadarrConfiguration newValue)
            => new(new RadarrConfiguration(oldValue, newValue));
    }
}