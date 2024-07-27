using Nuke.Common;
using Nuke.Common.Tools.Docker;

using static Nuke.Common.Tools.Docker.DockerTasks;

partial class Build : NukeBuild
{
    private string DockerTag => $"ghcr.io/maxnatamo/fetcharr:{GitVersion.SemVer}";

    Target BuildImage => _ => _
        .DependsOn(Test)
        .DependsOn(Format)
        .Executes(() =>
            DockerBuild(x => x
                .SetPath(".")
                .SetFile("Dockerfile")
                .SetTag(this.DockerTag)
                .AddLabel("org.opencontainers.image.source=https://github.com/maxnatamo/fetcharr")
                .AddLabel("org.opencontainers.image.url=https://github.com/maxnatamo/fetcharr")
                .AddLabel("org.opencontainers.image.description=Automatically sync Plex watchlist to your Sonarr and Radarr instances.")
                .AddLabel("org.opencontainers.image.licenses=MIT")));

    Target PushImage => _ => _
        .DependsOn(BuildImage)
        .Requires(() => this.GithubToken)
        .Executes(() =>
            DockerImagePush(x => x
                .SetName(this.DockerTag)));
}