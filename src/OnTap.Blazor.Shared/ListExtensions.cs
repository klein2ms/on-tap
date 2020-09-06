using System.Collections.Generic;

namespace OnTap.Blazor.Shared
{
    public static class ListExtensions
    {
        public static List<T> Append<T>(
            this List<T> list,
            IEnumerable<T> range)
        {
            list.AddRange(range);
            return list;
        }
    }
}