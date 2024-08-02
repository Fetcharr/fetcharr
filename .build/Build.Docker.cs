using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;

using Serilog;

using static Nuke.Common.Tools.Docker.DockerTasks;

partial class Build : NukeBuild
{
    [Parameter("Whether to push the built Docker image to GHCR")]
    readonly bool PushImage = false;

    [Parameter("List of platforms to build the Docker image for")]
    readonly string[] ImagePlatforms = ["linux/amd64", "linux/arm", "linux/arm64"];

    Target AssertDockerPush => _ => _
        .Unlisted()
        .Description("Asserts whether the built Docker image can be pushed.\n")
        .Before(Restore)
        .Executes(() =>
        {
            if(this.PushImage && string.IsNullOrEmpty(this.GithubToken))
            {
                Assert.Fail("Cannot push Docker image, when GitHub token is not set.");
            }
        });

    Target BuildImage => _ => _
        .Description("Builds the Docker image of Fetcharr, and optionally pushes it to GHCR.\n")
        .DependsOn(AssertDockerPush)
        .DependsOn(Test)
        .DependsOn(Format)
        .Executes(() =>
            DockerBuildxBuild(x => x
                .SetPath(".")
                .SetFile("Dockerfile")
                .SetTag(this.VersionTags.Select(version => $"{DockerImage}:{version}").ToArray())
                .SetPlatform(string.Join(",", this.ImagePlatforms))
                .SetPush(this.PushImage && this.GithubToken is not null)
                .AddCacheFrom("type=gha")
                .AddCacheTo("type=gha,mode=max")
                .AddLabel($"org.opencontainers.image.source={RepositoryUrl}")
                .AddLabel($"org.opencontainers.image.url={RepositoryUrl}")
                .AddLabel($"org.opencontainers.image.description={RepositoryDescription}")
                .AddLabel($"org.opencontainers.image.licenses={RepositoryLicense}")
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