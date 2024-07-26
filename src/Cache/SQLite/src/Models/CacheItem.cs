using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Fetcharr.Cache.SQLite.Models
{
    public class CacheItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Key { get; set; }

        public string? Value { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }

        public CacheItem(string key)
        {
            this.Key = key;
        }

        public CacheItem(string key, object? value, TimeSpan expiration)
        {
            this.Key = key;
            this.Value = value is null ? null : JsonSerializer.Serialize(value);
            this.ExpiresAt = DateTime.Now + expiration;
        }

        public async Task SetValueAsync<T>(T? value, CancellationToken cancellationToken = default)
        {
            this.Value = value is not null ? JsonSerializer.Serialize(value) : null;

            await Task.CompletedTask;
        }

        public async Task<T?> GetValueAsync<T>(CancellationToken cancellationToken = default)
        {
            if(this.Value is null)
            {
                return await Task.FromResult<T?>(default);
            }

            return JsonSerializer.Deserialize<T>(this.Value);
        }
    }
}