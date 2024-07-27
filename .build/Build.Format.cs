using Nuke.Common;
using Nuke.Common.Tools.DotNet;

using static Nuke.Common.Tools.DotNet.DotNetTasks;

partial class Build : NukeBuild
{
    Target Format => _ => _
        .DependsOn(Restore)
        .Executes(() =>
            DotNetFormat(c => c
                .SetProject(SolutionFilePath)
                .SetNoRestore(true)
                .SetSeverity(DotNetFormatSeverity.error)
                .AddExclude("src/Cache/SQLite/src/Migrations/*.cs")
                .SetVerifyNoChanges(true)));
}