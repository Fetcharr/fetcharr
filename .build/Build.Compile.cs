using Nuke.Common;
using Nuke.Common.Tools.DotNet;

partial class Build : NukeBuild
{
    Target Restore => _ => _
        .Executes(() =>
            DotNetTasks.DotNetRestore(c => c.SetProjectFile(SolutionFilePath)));

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
            DotNetTasks.DotNetBuild(c => c
                .SetProjectFile(SolutionFilePath)
                .SetNoRestore(true)
                .SetConfiguration(Configuration)));
}