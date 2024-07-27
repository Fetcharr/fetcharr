using Fetcharr.Cache.SQLite.Models;
using Fetcharr.Models.Configuration;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.SQLite.Contexts
{
    public class CacheContext(
        IOptions<SQLiteCachingProviderOptions> options,
        IAppDataSetup appDataSetup)
        : DbContext
    {
        public DbSet<CacheItem> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            string absoluteDatabasePath = Path.Combine(appDataSetup.CacheDirectory, options.Value.DatabasePath);
            builder.UseSqlite($"Data Source={absoluteDatabasePath}");
        }
    }
}