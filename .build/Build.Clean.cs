using JetBrains.Annotations;

using Nuke.Common;
using Nuke.Common.Tools.DotNet;

partial class Build : NukeBuild
{
    Target Clean => _ => _
        .Description("Cleans the build tree")
        .Before(Restore)
        .Executes(() =>
            DotNetTasks.DotNetClean(c => c.SetProject(SolutionFilePath)));
}