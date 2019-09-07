using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fifo100MillionPointsDemo.HelperClasses
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