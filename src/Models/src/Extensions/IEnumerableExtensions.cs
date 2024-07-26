namespace Fetcharr.Models.Extensions
{
    public static class IEnumerableExtensions
    {
        public static bool ContainsAny<TSource>(
            this IEnumerable<TSource> source,
            IEnumerable<TSource> value,
            IEqualityComparer<TSource>? comparer)
            => value.Any(v => source.Contains(v, comparer));
    }
}