using Nuke.Common;
using Nuke.Common.Execution;

[UnsetVisualStudioEnvironmentVariables]
partial class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild
        ? Configuration.Debug
        : Configuration.Release;

    [Parameter]
    [Secret]
    readonly string GithubToken;

    protected override void OnBuildInitialized()
    {
        Serilog.Log.Information("🔥 Build process started");

        base.OnBuildInitialized();
    }
}