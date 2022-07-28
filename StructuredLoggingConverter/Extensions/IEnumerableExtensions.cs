namespace Krip.Logging.Extensions
{
    public static class IEnumerableExtensions
    {
        // Standard implememention of Distinct() also keeps order,
        // but documentation doesn't guarantee it,
        // so it's safer to create custom version
        public static IEnumerable<T> DistinctOrdered<T>(this IEnumerable<T> items)
        {
            HashSet<T> result = new HashSet<T>();
            foreach (var item in items)
            {
                if (result.Add(item))
                {
                    yield return item;
                }
            }
        }
    }
}
