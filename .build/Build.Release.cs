using Nuke.Common;
using Nuke.Common.Tools.GitHub;

using Octokit;

partial class Build : NukeBuild
{
    [Parameter("Whether the release is a pre-release or not.")]
    readonly bool PreRelease;

    Target Release => _ => _
        .Description("Creates and pushes a new release to GitHub")
        .DependsOn(BuildImage)
        .Requires(() => this.GithubToken)
        .Executes(async () =>
        {
            ProductHeaderValue productInformation = new("fetcharr");
            GitHubTasks.GitHubClient = new GitHubClient(productInformation)
            {
                Credentials = new Credentials(this.GithubToken)
            };

            NewRelease release = new(GitVersion.MajorMinorPatch)
            {
                Name = GitVersion.MajorMinorPatch,
                Prerelease = this.PreRelease,
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