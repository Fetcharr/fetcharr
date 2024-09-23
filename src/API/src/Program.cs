using Cachedeer;
using Cachedeer.InMemory;
using Cachedeer.Sqlite;
using Cachedeer.Tiered;

using Fetcharr.API.Extensions;
using Fetcharr.Models.Configuration;

using Serilog;
using Serilog.Events;

namespace Fetcharr.API
{
    class Program
    {
        static async Task Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Host.UseSerilog((context, serviceProvider, configuration) =>
            {
                IAppDataSetup appDataSetup = serviceProvider.GetRequiredService<IAppDataSetup>();

                configuration.MinimumLevel.Warning();
                configuration.MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Error);
                configuration.MinimumLevel.Override("Microsoft.Hosting", LogEventLevel.Information);
                configuration.MinimumLevel.Override("Fetcharr", LogEventLevel.Information);

                if(context.HostingEnvironment.IsDevelopment())
                {
                    configuration.MinimumLevel.Information();
                    configuration.MinimumLevel.Override("Fetcharr", LogEventLevel.Verbose);
                    configuration.MinimumLevel.Override("Cachedeer", LogEventLevel.Information);
                }

                configuration.WriteTo.Console();

                if(context.HostingEnvironment.IsProduction())
                {
                    configuration.WriteTo.File(
                        $"{appDataSetup.LogDirectory}/fetcharr.log",
                        rollingInterval: RollingInterval.Day);
                }
            });

            builder.Services.AddCaching()
                .UseTiered("metadata", builder => builder
                    .UseInMemory("metadata-fast", opts =>
                    {
                        opts.DefaultExpiration = TimeSpan.FromMinutes(1);
                    })
                    .UseSqlite("metadata-slow", (services, name, opts) =>
                    {
                        IAppDataSetup appDataSetup = services.GetRequiredService<IAppDataSetup>();

                        opts.DefaultExpiration = TimeSpan.FromHours(4);
                        opts.DatabasePath = Path.Join(appDataSetup.CacheDirectory, "metadata.sqlite");
                    }))
                .UseInMemory("watchlist");

            builder.Services.AddFetcharr();
            builder.Services.AddControllers();

            WebApplication app = builder.Build();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}