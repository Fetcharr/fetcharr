using Nuke.Common;
using Nuke.Common.Tools.DotNet;

partial class Build : NukeBuild
{
    [Parameter("Whether to include integration tests (default: false).")]
    readonly bool IncludeIntegrationTests = false;

    Target Test => _ => _
        .Description("Runs test suites within the build tree.\n")
        .DependsOn(Compile)
        .Executes(() =>
            DotNetTasks.DotNetTest(c => c
                .SetProjectFile(SolutionFilePath)
                .SetNoRestore(true)
                .SetFilter(this.IncludeIntegrationTests ? "Test" : "Category!=IntegrationTest")
                .SetConfiguration(Configuration.Debug)));
}