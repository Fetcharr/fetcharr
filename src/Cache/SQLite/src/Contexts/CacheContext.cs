using Fetcharr.Cache.SQLite.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.SQLite.Contexts
{
    public class CacheContext(
        IOptions<SQLiteCachingProviderOptions> options)
        : DbContext
    {
        public DbSet<CacheItem> Items { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
            => builder.UseSqlite($"Data Source={options.Value.DatabasePath}");
    }
}