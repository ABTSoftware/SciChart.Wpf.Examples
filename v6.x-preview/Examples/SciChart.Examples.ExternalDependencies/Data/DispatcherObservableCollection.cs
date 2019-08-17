// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DispatcherObservableCollection.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public class DispatcherObservableCollection<T> : IList<T>, INotifyCollectionChanged
    {
        private readonly IList<T> _collection = new List<T>();
        private readonly Dispatcher _dispatcher;
        public event NotifyCollectionChangedEventHandler CollectionChanged;
        private readonly object _syncRoot = new object();

        internal static Dispatcher GetDispatcher()
        {
#if SILVERLIGHT
            var dispatcher = Deployment.Current.Dispatcher;

            // if we did not get the Dispatcher throw an exception
            if (dispatcher != null)
                return dispatcher;

            throw new InvalidOperationException("Unable to get the Silverlight Deployment.Current.Dispatcher");
#else
            return Dispatcher.CurrentDispatcher;
#endif
        }

        public DispatcherObservableCollection()
        {
            _dispatcher = GetDispatcher();
        }

        public void Add(T item)
        {
            lock (_syncRoot)
            {
                _collection.Add(item);
                int index = _collection.Count - 1;
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _collection.Clear();
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public bool Contains(T item)
        {
            lock (_syncRoot)
            {
                var result = _collection.Contains(item);
                return result;
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_syncRoot)
            {
                _collection.CopyTo(array, arrayIndex);
            }
        }

        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    var result = _collection.Count;
                    return result;
                }
            }
        }

        public bool IsReadOnly
        {
            get { return _collection.IsReadOnly; }
        }

        public bool Remove(T item)
        {
            if (_dispatcher.CheckAccess())
                return DoRemove(item);
            else
            {
                bool? result = null;
                _dispatcher.BeginInvoke(new Func<T, bool>(t =>
                {
                    result = DoRemove(t);
                    return result.Value;
                }), item);
                return result != null && result != false;
            }
        }

        private bool DoRemove(T item)
        {
            lock (_syncRoot)
            {
                var index = _collection.IndexOf(item);
                if (index == -1)
                {
                    return false;
                }
                var result = _collection.Remove(item);
                if (result)
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                return result;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections
            .IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public int IndexOf(T item)
        {
            lock (_syncRoot)
            {
                var result = _collection.IndexOf(item);
                return result;
            }
        }

        public void Insert(int index, T item)
        {
            lock (_syncRoot)
            {
                _collection.Insert(index, item);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item, index));
            }
        }

        public void RemoveAt(int index)
        {
            lock (_syncRoot)
            {
                if (_collection.Count == 0 || _collection.Count <= index)
                {
                    return;
                }
                _collection.RemoveAt(index);
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }

        public T this[int index]
        {
            get
            {
                lock (_syncRoot)
                {
                    var result = _collection[index];
                    return result;
                }
            }
            set
            {
                lock (_syncRoot)
                {
                    if (_collection.Count == 0 || _collection.Count <= index)
                    {
                        return;
                    }
                    _collection[index] = value;
                    OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
                }
            }
        }

        private void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            var handler = CollectionChanged;
            if (handler == null) return;

            if (_dispatcher.CheckAccess())
                handler(this, e);
            else
                _dispatcher.BeginInvoke((Action)(() => handler(this, e)));
        }
    }
}
