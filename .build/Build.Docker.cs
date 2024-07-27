using Nuke.Common;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Docker;

using Serilog;

using static Nuke.Common.Tools.Docker.DockerTasks;

partial class Build : NukeBuild
{
    private string DockerImage => $"ghcr.io/maxnatamo/fetcharr";

    private string DockerTag => GitVersion.SemVer;

    private string DockerImageTag => $"{DockerImage}:{DockerTag}";

    Target BuildImage => _ => _
        .DependsOn(Test)
        .DependsOn(Format)
        .Executes(() =>
            DockerBuildxBuild(x => x
                .SetPath(".")
                .SetFile("Dockerfile")
                .SetTag(this.DockerImageTag)
                .AddCacheFrom("type=gha")
                .AddCacheTo("type=gha,mode=max")
                .AddLabel("org.opencontainers.image.source=https://github.com/maxnatamo/fetcharr")
                .AddLabel("org.opencontainers.image.url=https://github.com/maxnatamo/fetcharr")
                .AddLabel("org.opencontainers.image.description=Automatically sync Plex watchlist to your Sonarr and Radarr instances.")
                .AddLabel("org.opencontainers.image.licenses=MIT")
                .SetProcessLogger((outputType, output) =>
                {
                    // Workaround for all Docker messages being logged as errors.
                    // Source: https://github.com/nuke-build/nuke/issues/1201
                    if (outputType == OutputType.Std)
                    {
                        Log.Error(output);
                    }
                    else
                    {
                        Log.Information(output);
                    }
                })));

    Target PushImage => _ => _
        .DependsOn(BuildImage)
        .Requires(() => this.GithubToken)
        .Executes(() =>
            DockerImagePush(x => x
                .SetName(this.DockerImageTag)));
}