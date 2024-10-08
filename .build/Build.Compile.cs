using Nuke.Common;
using Nuke.Common.Tools.DotNet;

partial class Build : NukeBuild
{
    Target Restore => _ => _
        .Description("Downloads and install .NET packages.\n")
        .Executes(() =>
            DotNetTasks.DotNetRestore(c => c.SetProjectFile(SolutionFilePath)));

    Target Compile => _ => _
        .Description("Compiles the entire build tree.\n")
        .DependsOn(Restore)
        .Executes(() =>
            DotNetTasks.DotNetBuild(c => c
                .SetProjectFile(SolutionFilePath)
                .SetNoRestore(true)
                .SetConfiguration(Configuration)));
}