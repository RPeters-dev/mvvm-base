using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MVVM.Base
{
    public static class ICollectionExtension
    {
        public static void Update<T>(this ICollection<T> source, IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            var newItems = items.Except(source, comparer).ToArray();
            var oldItems = source.Except(items, comparer).ToArray();

            foreach (var item in newItems)
            {
                source.Add(item);
            }

            foreach (var item in oldItems)
            {
                source.Remove(item);
            }
        }
    }
}
