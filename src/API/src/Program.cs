using Fetcharr.API.Extensions;
using Fetcharr.API.Services;
using Fetcharr.Cache.Core.Extensions;
using Fetcharr.Cache.Hybrid.Extensions;
using Fetcharr.Cache.InMemory.Extensions;
using Fetcharr.Configuration.Extensions;
using Fetcharr.Models.Extensions;
using Fetcharr.Shared.Http.Extensions;

namespace Fetcharr.API
{
    class Program
    {
        static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Services.AddLogging(opts =>
                opts.AddSimpleConsole(options =>
                {
                    options.IncludeScopes = true;
                    options.SingleLine = true;
                    options.TimestampFormat = "HH:mm:ss ";
                }));

            builder.Services.AddCaching(opts => opts
                .UseHybrid("metadata", opts => opts.SQLite.DatabasePath = "metadata.sqlite")
                .UseInMemory("watchlist"));

            builder.Services
                .AddDefaultEnvironment()
                .AddConfiguration()
                .AddValidation()
                .AddPlexServices()
                .AddSonarrServices()
                .AddRadarrServices()
                .AddPingingServices()
                .AddFlurlErrorHandler()
                .AddHostedService<StartupInformationService>();

            builder.Services.AddControllers();

            WebApplication app = builder.Build();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}