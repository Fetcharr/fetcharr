using Nuke.Common;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    Target Format => _ => _
        .Description("Performs linting on the build tree")
        .DependsOn(Restore)
        .Executes(() =>
            DotNetFormat(c => c
                .SetProject(SolutionFilePath)
                .SetNoRestore(true)
                .SetSeverity(DotNetFormatSeverity.error)
                .SetVerifyNoChanges(true)));
}