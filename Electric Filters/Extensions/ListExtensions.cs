using System.Collections.Generic;

namespace Electric_Filters.Extensions
{
    public static class ListExtensions
    {
        public static IEnumerable<T> Closest<T>(this IEnumerable<T> source, decimal target, int count)
        {
            var result = source;

            return source;
        }
    }
}
