using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitVersion;

partial class Build : NukeBuild
{
    private AbsolutePath SourceDirectory => RootDirectory / "src";

    private AbsolutePath SolutionFilePath => SourceDirectory / "Fetcharr.sln";

    [GitVersion(Framework = "net8.0", NoFetch = true)]
    readonly GitVersion GitVersion;

    [GitRepository]
    readonly GitRepository Repository;
}