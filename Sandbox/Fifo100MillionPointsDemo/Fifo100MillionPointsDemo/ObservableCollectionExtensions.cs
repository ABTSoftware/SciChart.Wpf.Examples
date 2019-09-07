using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fifo100MillionPointsDemo
{
    public static class ObservableCollectionExtensions
    {
        public static void AddRange<T>(this ObservableCollection<T> coll, IEnumerable<T> enumerable)
        {
            foreach (var item in enumerable)
            {
                coll.Add(item);
            }
        }
    }
}