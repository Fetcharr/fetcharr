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

    [Secret]
    [Parameter("GitHub Token for pushing Docker images to GHCR")]
    readonly string GithubToken;

    protected override void OnBuildInitialized()
    {
        Serilog.Log.Information("🔥 Build process started");
        Serilog.Log.Information("   Repository: {Repository}", this.Repository.HttpsUrl);
        Serilog.Log.Information("   Version:    {Version}", this.VersionTag);
        Serilog.Log.Information("   Tags:       {VersionTags}", this.VersionTags);
        Serilog.Log.Information("   IsRelease:  {IsReleaseBuild}", this.IsReleaseBuild);

        if(this.GitHubActions is not null)
        {
            Serilog.Log.Information("   Branch:     {BranchName}", this.GitHubActions.Ref);
            Serilog.Log.Information("   Commit:     {CommitSha}", this.GitHubActions.Sha);
        }

        base.OnBuildInitialized();
    }
}