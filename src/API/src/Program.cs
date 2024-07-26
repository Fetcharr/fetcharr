using Fetcharr.API.Configuration;
using Fetcharr.API.Extensions;
using Fetcharr.Cache.Core.Extensions;
using Fetcharr.Cache.Hybrid.Extensions;
using Fetcharr.Cache.InMemory.Extensions;
using Fetcharr.Models.Configuration;
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

            builder.Services.AddSingleton<FetcharrConfiguration>(_
                => new ConfigurationParser().ReadConfig("config.yaml"));

            builder.Services.AddCaching(opts => opts
                .UseHybrid("metadata", opts => opts.SQLite.DatabasePath = "db/metadata.sqlite")
                .UseInMemory("watchlist"));

            builder.Services
                .AddPlexServices()
                .AddSonarrServices()
                .AddRadarrServices()
                .AddPingingServices()
                .AddFlurlErrorHandler();

            builder.Services.AddControllers();

            WebApplication app = builder.Build();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}