using System.Collections.Generic;

namespace Orego.Util
{
    public static class OregoHashSetUtils
    {
        public static HashSet<T> ToSet<T>(this IEnumerable<T> enumerable)
        {
            var resultSet = new HashSet<T>();
            foreach (var item in enumerable)
            {
                resultSet.Add(item);
            }

            return resultSet;
        }

        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> range)
        {
            set.UnionWith(range);
        }

        public static bool IsEmpty<T>(this HashSet<T> set)
        {
            return set.Count == 0;
        }

        public static bool IsNotEmpty<T>(this HashSet<T> set)
        {
            return set.Count > 0;
        }
    }
}