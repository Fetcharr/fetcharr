using Fetcharr.API.Extensions;
using Fetcharr.Cache.Core.Extensions;
using Fetcharr.Cache.Hybrid.Extensions;
using Fetcharr.Cache.InMemory.Extensions;
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
                    configuration.MinimumLevel.Override("Fetcharr.Cache", LogEventLevel.Information);
                }

                configuration.WriteTo.Console();

                if(context.HostingEnvironment.IsProduction())
                {
                    configuration.WriteTo.File(
                        $"{appDataSetup.LogDirectory}/fetcharr.log",
                        rollingInterval: RollingInterval.Day);
                }
            });

            builder.Services.AddCaching(opts => opts
                .UseHybrid("metadata", opts => opts.SQLite.DatabasePath = "metadata.sqlite")
                .UseInMemory("watchlist")
                .UseInMemory("plex-graphql"));

            builder.Services.AddFetcharr();
            builder.Services.AddControllers();

            WebApplication app = builder.Build();

            app.MapControllers();

            await app.RunAsync();
        }
    }
}