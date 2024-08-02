
using Fetcharr.Testing.Containers.Extensions;

namespace Fetcharr.Testing.Containers.Sonarr
{
    /// <inheritdoc cref="ContainerBuilder{TBuilderEntity, TContainerEntity, TConfigurationEntity}" />
    public sealed class SonarrBuilder : ContainerBuilder<SonarrBuilder, SonarrContainer, SonarrConfiguration>
    {
        public const string SonarrImage = "lscr.io/linuxserver/sonarr:4.0.8";

        public const ushort SonarrPort = 8989;

        public const string DefaultSonarrApiKey = "5aec487a70b5417e880d3923e4786d18";

        /// <summary>
        ///   Initializes a new instance of the <see cref="SonarrBuilder" /> class.
        /// </summary>
        public SonarrBuilder()
            : this(new SonarrConfiguration())
        {
            this.DockerResourceConfiguration = this.Init().DockerResourceConfiguration;
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref="SonarrBuilder" /> class.
        /// </summary>
        /// <param name="resourceConfiguration">The Docker resource configuration.</param>
        private SonarrBuilder(SonarrConfiguration resourceConfiguration)
            : base(resourceConfiguration)
        {
            this.DockerResourceConfiguration = resourceConfiguration;
        }

        /// <inheritdoc />
        protected override SonarrConfiguration DockerResourceConfiguration { get; }

        /// <summary>
        ///   Sets the Sonarr API key.
        /// </summary>
        /// <param name="apiKey">The Sonarr API key.</param>
        /// <returns>A configured instance of <see cref="SonarrBuilder" />.</returns>
        public SonarrBuilder WithApiKey(string apiKey)
        {
            return this.Merge(this.DockerResourceConfiguration, new SonarrConfiguration(apiKey: apiKey))
                .WithEnvironment("Sonarr__Auth__ApiKey", apiKey);
        }

        /// <inheritdoc />
        public override SonarrContainer Build()
        {
            this.Validate();
            return new SonarrContainer(this.DockerResourceConfiguration);
        }

        /// <inheritdoc />
        protected override SonarrBuilder Init() =>
            base.Init()
                .WithImage(SonarrImage)
                .WithPortBinding(SonarrPort, assignRandomHostPort: true)
                .WithApiKey(DefaultSonarrApiKey)
                .WithWaitStrategy(Wait.ForUnixContainer()
                    .UntilMessageIsLogged("Now listening on"));

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
        protected override SonarrBuilder Clone(IResourceConfiguration<CreateContainerParameters> resourceConfiguration)
            => this.Merge(this.DockerResourceConfiguration, new SonarrConfiguration(resourceConfiguration));

        /// <inheritdoc />
        protected override SonarrBuilder Clone(IContainerConfiguration resourceConfiguration)
            => this.Merge(this.DockerResourceConfiguration, new SonarrConfiguration(resourceConfiguration));

        /// <inheritdoc />
        protected override SonarrBuilder Merge(SonarrConfiguration oldValue, SonarrConfiguration newValue)
            => new(new SonarrConfiguration(oldValue, newValue));
    }
}