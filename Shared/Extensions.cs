using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DominosCutScreen.Shared
{
    public static class Extensions
    {
        /// https://codereview.stackexchange.com/a/6625
        public static IEnumerable<IGrouping<TKey, TElement>> GroupBySequential<TSource, TKey, TElement>(
            this IEnumerable<TSource> Source,
            Func<TSource, TKey> KeySelector,
            Func<TSource, TElement> ElementSelector,
            Func<TKey, TKey, bool> Comparer)
        {
            if (!Source.Any())
                yield break;

            var currentKey = KeySelector(Source.First());
            var foundItems = new List<TElement>();
            foreach (var item in Source)
            {
                var key = KeySelector(item);
                bool passedCheck = Comparer(currentKey, key);
                if (!passedCheck)
                {
                    yield return new Grouping<TKey, TElement>(currentKey, foundItems);
                    currentKey = key;
                    foundItems = new();
                }

                foundItems.Add(ElementSelector(item));
            }

            if (foundItems.Count > 0)
                yield return new Grouping<TKey, TElement>(currentKey, foundItems);
        }
    }
}
