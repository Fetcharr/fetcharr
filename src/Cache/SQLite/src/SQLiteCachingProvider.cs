using Fetcharr.Cache.Core;
using Fetcharr.Cache.Core.Logging;
using Fetcharr.Cache.SQLite.Contexts;
using Fetcharr.Cache.SQLite.Models;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fetcharr.Cache.SQLite
{
    /// <summary>
    ///   Provider implementation for SQLite caching.
    /// </summary>
    public class SQLiteCachingProvider(
        IOptions<SQLiteCachingProviderOptions> options,
        ILogger<SQLiteCachingProvider> logger,
        IDbContextFactory<CacheContext> contextFactory)
        : BaseCachingProvider
    {
        /// <inheritdoc />
        public override string Name => options.Value.Name;

        /// <inheritdoc />
        public override async Task InitializeAsync(CancellationToken cancellationToken = default)
        {
            using CacheContext context = contextFactory.CreateDbContext();
            await context.Database.MigrateAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task PingAsync(CancellationToken cancellationToken = default)
        {
            using CacheContext context = contextFactory.CreateDbContext();
            await context.Database.CanConnectAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task FlushAsync(CancellationToken cancellationToken = default)
            => await Task.CompletedTask;

        /// <inheritdoc />
        public override async Task EvictExpiredAsync(CancellationToken cancellationToken = default)
        {
            using CacheContext context = contextFactory.CreateDbContext();

            await context.Items
                .Where(i => i.ExpiresAt < DateTime.Now)
                .ExecuteDeleteAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key, nameof(key));

            expiration ??= options.Value.DefaultExpiration;
            if(options.Value.MaxExpirationDilation > 0)
            {
                TimeSpan dilationTime = TimeSpan.FromSeconds(Random.Shared.Next(options.Value.MaxExpirationDilation));
                expiration += dilationTime;
            }

            using CacheContext context = contextFactory.CreateDbContext();

            CacheItem? existing = await context.Items
                .Where(i => i.Key == key)
                .FirstOrDefaultAsync(cancellationToken);

            if(existing is not null)
            {
                existing.ExpiresAt = DateTime.Now + expiration.Value;
                await existing.SetValueAsync(value, cancellationToken);

                context.Items.Update(existing);
            }
            else
            {
                await context.Items.AddAsync(new CacheItem(key, value, expiration.Value), cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task<CacheValue<T>> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNullOrEmpty(key, nameof(key));

            using CacheContext context = contextFactory.CreateDbContext();

            await context.Items
                .Where(v => v.ExpiresAt < DateTime.Now && v.Key == key)
                .ExecuteDeleteAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);

            CacheItem? item = await context.Items
                .Where(v => v.Key == key)
                .FirstOrDefaultAsync(cancellationToken);

            if(item is null)
            {
                if(options.Value.EnableLogging)
                {
                    logger.CacheMiss(this.Name, key);
                }

                return CacheValue<T>.NoValue;
            }

            if(options.Value.EnableLogging)
            {
                logger.CacheHit(this.Name, key);
            }

            return new CacheValue<T>(await item.GetValueAsync<T>(cancellationToken), true);
        }

        /// <inheritdoc />
        public override async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            using CacheContext context = contextFactory.CreateDbContext();

            await context.Items
                .Where(i => i.Key == key)
                .ExecuteDeleteAsync(cancellationToken);

            await context.SaveChangesAsync(cancellationToken);
        }

        /// <inheritdoc />
        public override async Task<bool> ExistsAsync(string key, CancellationToken cancellationToken = default)
        {
            using CacheContext context = contextFactory.CreateDbContext();
            return await context.Items.Where(i => i.Key == key).AnyAsync(cancellationToken);
        }
    }
}