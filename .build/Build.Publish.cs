using System.IO.Compression;

using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.DotNet;

partial class Build : NukeBuild
{
    Target Publish => _ => _
        .Description("Publishes the .NET projects and builds them as single-file applications\n")
        .DependsOn(Compile)
        .Executes(() => {
            string runtimeIdentifier = "win-x64";

            if(OperatingSystem.IsLinux()) {
                runtimeIdentifier = "linux-x64";
            }

            DotNetTasks.DotNetPublish(c => c
                .SetConfiguration(Configuration)
                .SetProject(ApiProjectDirectory)
                .SetRuntime(runtimeIdentifier)
                .SetOutput(PublishOutputDirectory)
                .SetPublishSingleFile(true));

            PublishOutputDirectory.ZipTo(
                AssetsDirectory / $"fetcharr-{VersionTag}-{runtimeIdentifier}.zip",
                filter: x => !x.HasExtension(".pdb"),
                compressionLevel: CompressionLevel.SmallestSize,
                fileMode: FileMode.Create
            );
        });
}