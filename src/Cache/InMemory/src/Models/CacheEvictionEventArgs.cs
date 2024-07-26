namespace Fetcharr.Cache.InMemory.Models
{
    public class CacheEvictionEventArgs(string key, object? value) : EventArgs
    {
        public readonly string Key = key;

        public readonly object? Value = value;
    }
}