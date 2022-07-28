using System.Collections.Concurrent;

namespace StructuredLoggingConverter.Extensions
{
    public static class ConcurrentBagExtensions
    {
        public static void AddRange<T>(this ConcurrentBag<T> concurrentBag, IEnumerable<T> elements)
        {
            foreach (var element in elements)
            {
                concurrentBag.Add(element);
            }
        }
    }
}
