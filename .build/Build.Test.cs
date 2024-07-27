using Nuke.Common;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;

partial class Build : NukeBuild
{
    IEnumerable<Project> TestProjects => SolutionModelTasks
        .ReadSolution(SolutionFilePath)
        .GetAllProjects("*.Tests");

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
            DotNetTasks.DotNetTest(c => c
                .SetNoBuild(true)
                .SetNoRestore(true)
                .SetConfiguration(Configuration.Debug)
                .CombineWith(TestProjects, (_, project) => _
                    .SetProjectFile(project))));
}