using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MVVM.Base
{
    public static class ICollectionExtension
    {
        public static void AddRange(this ItemCollection source, IEnumerable items)
        {
            foreach (var item in items)
            {
                source.Add(item);
            }
        }

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
