using Fetcharr.Models.Configuration;

namespace Fetcharr.API.Services
{
    /// <summary>
    ///   Hosted service for logging information about the environment on startup.
    /// </summary>
    public class StartupInformationService(
        IAppDataSetup appDataSetup,
        ILogger<StartupInformationService> logger)
        : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation(
                "Fetcharr v{Version} ({BranchName}-{ShortSha}):",
                GitVersionInformation.SemVer,
                GitVersionInformation.BranchName,
                GitVersionInformation.ShortSha);

            logger.LogInformation("  Base directory: {Path}", appDataSetup.BaseDirectory);
            logger.LogInformation("  Cache directory: {Path}", appDataSetup.CacheDirectory);
            logger.LogInformation("  Config directory: {Path}", appDataSetup.ConfigDirectory);

            await Task.CompletedTask;
        }
    }
}