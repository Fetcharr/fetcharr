using Nuke.Common;
using Nuke.Common.Tools.GitHub;

using Octokit;

partial class Build : NukeBuild
{
    Target Release => _ => _
        .Description("Creates and pushes a new release to GitHub.\n")
        .DependsOn(BuildImage)
        .Requires(() => this.GithubToken)
        .Executes(async () =>
        {
            ProductHeaderValue productInformation = new("fetcharr");
            GitHubTasks.GitHubClient = new GitHubClient(productInformation)
            {
                Credentials = new Credentials(this.GithubToken)
            };

            NewRelease release = new(this.VersionTag)
            {
                Name = this.VersionTag,
                Prerelease = !this.IsReleaseBuild,
                Draft = false,
                GenerateReleaseNotes = true,
                MakeLatest = MakeLatestQualifier.True,
            };

            await GitHubTasks.GitHubClient.Repository.Release.Create(
                this.Repository.GetGitHubOwner(),
                this.Repository.GetGitHubName(),
                release);
        });
}