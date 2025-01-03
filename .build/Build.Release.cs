using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;

using Octokit;

partial class Build : NukeBuild
{
    Target Release => _ => _
        .Description("Creates and pushes a new release to GitHub.\n")
        .DependsOn(BuildImage)
        .DependsOn(Publish)
        .Requires(() => this.GithubToken)
        .Executes(async () =>
        {
            ProductHeaderValue productInformation = new("fetcharr");
            GitHubTasks.GitHubClient = new GitHubClient(productInformation)
            {
                Credentials = new Credentials(this.GithubToken)
            };

            NewRelease newRelease = new(this.VersionTag)
            {
                Name = this.VersionTag,
                Prerelease = !this.IsReleaseBuild,
                Draft = false,
                GenerateReleaseNotes = true,
                MakeLatest = MakeLatestQualifier.True,
            };

            Release release;

            try
            {
                release = await GitHubTasks.GitHubClient.Repository.Release.Get(
                    this.Repository.GetGitHubOwner(),
                    this.Repository.GetGitHubName(),
                    this.VersionTag);
            }
            catch
            {
                release = await GitHubTasks.GitHubClient.Repository.Release.Create(
                    this.Repository.GetGitHubOwner(),
                    this.Repository.GetGitHubName(),
                    newRelease);
            }

            foreach(AbsolutePath asset in AssetsDirectory.GlobFiles($"fetcharr-{VersionTag}-*.zip"))
            {
                Serilog.Log.Information("Uploading asset '{Asset}' to release...", asset.Name);

                using FileStream assetStream = File.OpenRead(asset);

                ReleaseAssetUpload assetUpload = new()
                {
                    FileName = asset.Name,
                    ContentType = "application/zip",
                    RawData = assetStream
                };

                await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(
                    release,
                    assetUpload
                );
            }
        });
}