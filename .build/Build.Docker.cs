using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;

using Serilog;

using static Nuke.Common.Tools.Docker.DockerTasks;

partial class Build : NukeBuild
{
    [Parameter("Whether to push the built Docker image to GHCR")]
    readonly bool PushImage = false;

    private string DockerImage => $"ghcr.io/fetcharr/fetcharr";

    private string[] DockerVersionTags => GitVersion.BranchName.Equals("main", StringComparison.InvariantCultureIgnoreCase)
        ? ["latest", $"{GitVersion.Major}", $"{GitVersion.Major}.{GitVersion.Minor}", $"{GitVersion.MajorMinorPatch}"]
        : ["develop", $"develop-{GitVersion.MajorMinorPatch}.{GitVersion.PreReleaseNumber}"];

    private string[] DockerImageTags => DockerVersionTags.Select(version => $"{DockerImage}:{version}").ToArray();

    private string[] DockerImagePlatforms => ["linux/amd64", "linux/arm", "linux/arm64"];

    Target AssertDockerPush => _ => _
        .Unlisted()
        .Description("Asserts whether the built Docker image can be pushed.")
        .Before(Restore)
        .Executes(() =>
        {
            if(this.PushImage && string.IsNullOrEmpty(this.GithubToken))
            {
                Assert.Fail("Cannot push Docker image, when GitHub token is not set.");
            }
        });

    Target BuildImage => _ => _
        .Description("Builds the Docker image of Fetcharr, and optionally pushes it to GHCR.")
        .DependsOn(AssertDockerPush)
        .DependsOn(Test)
        .DependsOn(Format)
        .Executes(() =>
            DockerBuildxBuild(x => x
                .SetPath(".")
                .SetFile("Dockerfile")
                .SetTag(this.DockerImageTags)
                .SetPlatform(string.Join(",", this.DockerImagePlatforms))
                .SetPush(this.PushImage && this.GithubToken is not null)
                .AddCacheFrom("type=gha")
                .AddCacheTo("type=gha,mode=max")
                .AddLabel("org.opencontainers.image.source=https://github.com/fetcharr/fetcharr")
                .AddLabel("org.opencontainers.image.url=https://github.com/fetcharr/fetcharr")
                .AddLabel("org.opencontainers.image.description=Automatically sync Plex watchlist to your Sonarr and Radarr instances.")
                .AddLabel("org.opencontainers.image.licenses=MIT")
                .SetProcessLogger((outputType, output) =>
                {
                    // Workaround for all Docker messages being logged as errors.
                    // Source: https://github.com/nuke-build/nuke/issues/1201
                    if(outputType == OutputType.Std)
                    {
                        Log.Error(output);
                    }
                    else
                    {
                        Log.Information(output);
                    }
                })));
}