using System;
using System.Collections.Generic;
using System.Linq;

namespace Ascetic.Core.Linq
{
    public static class Enumerable
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var seenKeys = new HashSet<TKey>();
            foreach (var element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static IEnumerable<T[]> BatchArray<T>(this IEnumerable<T> source, int batchSize)
        {
            using (var enumerator = source.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    yield return YieldBatchArrayElements(enumerator, batchSize);
                }
            }
        }

        private static T[] YieldBatchArrayElements<T>(IEnumerator<T> source, int batchSize)
        {
            var array = new T[batchSize];
            array[0] = source.Current;
            int i;
            for (i = 1; i < batchSize && source.MoveNext(); i++)
            {
                array[i] = source.Current;
            }

            if (i < batchSize)
            {
                return array.Take(i).ToArray();
            }
            return array;
        }
    }
}
