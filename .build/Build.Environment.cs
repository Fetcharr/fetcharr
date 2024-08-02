using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitVersion;

partial class Build : NukeBuild
{
    private const string RepositoryUrl = "https://github.com/fetcharr/fetcharr";

    private const string RepositoryDescription = "Automatically sync Plex watchlist to your Sonarr and Radarr instances.";

    private const string RepositoryLicense = "MIT";

    private const string DockerImage = "ghcr.io/fetcharr/fetcharr";

    private AbsolutePath SourceDirectory => RootDirectory / "src";

    private AbsolutePath SolutionFilePath => SourceDirectory / "Fetcharr.sln";

    [GitVersion(Framework = "net8.0", NoFetch = true)]
    readonly GitVersion GitVersion;

    [GitRepository]
    readonly GitRepository Repository;

    GitHubActions GitHubActions => GitHubActions.Instance;

    /// <summary>
    ///   Gets whether NUKE is building a release build or not.
    /// </summary>
    private bool IsReleaseBuild => GitVersion.BranchName.Equals("main", StringComparison.InvariantCultureIgnoreCase);

    /// <summary>
    ///   Gets the version tag for the current build, with release version numbering.
    /// </summary>
    private string ReleaseVersionTag => GitVersion.MajorMinorPatch;

    /// <summary>
    ///   Gets the version tag for the current build, with development version numbering.
    /// </summary>
    private string DevelopmentVersionTag => $"develop-{GitVersion.MajorMinorPatch}.{GitVersion.PreReleaseNumber}";

    /// <summary>
    ///   Gets the primary version tag for the current version.
    /// </summary>
    private string VersionTag => this.IsReleaseBuild
        ? ReleaseVersionTag
        : DevelopmentVersionTag;

    /// <summary>
    ///   Gets the version tags for the current version.
    /// </summary>
    private string[] VersionTags => this.IsReleaseBuild
        ? ["latest", $"{GitVersion.Major}", $"{GitVersion.Major}.{GitVersion.Minor}", ReleaseVersionTag]
        : ["develop", DevelopmentVersionTag];
}