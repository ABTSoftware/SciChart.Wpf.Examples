// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CircularBuffer.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;

namespace SciChart.Examples.Examples.CreateRealtimeChart
{
    /// <summary>
    ///     Implements a simple FIFO buffer. This is used to store DataSeries in the RealTimeGhostedTraces example
    ///     e.g.
    ///     When Capacity = 5 and appending items 0, 1, 2, 3, 4 then indexes 0,1,2,3,4 resolve the 5 items
    ///     When Capacity = 5 and appending items 0, 1, 2, 3, 4, 5, 6 then indexes 0,1,2,3,4 resolve items 2, 3, 4, 5, 6 
    ///         i.e. the first two items, 0 and 1, have been lost when the capacity is exceeded
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class CircularBuffer<T>
    {
        private readonly T[] _innerList;
        private int _startIndex = -1;

        private readonly int _size;
        private int _usedSize;

        public int Size { get { return _size; } }
        public int Count { get { return _usedSize; } internal set { _usedSize = value; } }

        public CircularBuffer(int size)
        {
            _size = size;
            _innerList = new T[size];
        }

        public T Add(T item)
        {
            var index = NextIndex();

            _innerList[index] = item;
            _usedSize = Math.Min(_usedSize + 1, _size);

            return item;
        }

        public IList<T> InnerList { get { return _innerList; } }

        public T this[int index]
        {
            get { return GetItemAt(index); }
        }

        private int NextIndex()
        {
            if (_startIndex >= 0 || _usedSize == Size) //Now rotating
            {
                _startIndex = (_startIndex + 1) % Size;
                if (_startIndex > _usedSize)
                    _startIndex = _usedSize;

                return _startIndex;
            }
            else
            {
                return _usedSize;
            }
        }

        public T GetItemAt(int index)
        {
            ValidateIndex(index);
            int i = ResolveIndex(index);
            return _innerList[i];
        }

        private void ValidateIndex(int index)
        {
            if (index < 0 || index >= _usedSize)
                throw new IndexOutOfRangeException();
        }

        internal int ResolveIndex(int index)
        {
            if (_startIndex < 0)
                return index;
            else
                return (_startIndex + 1 + index) % _usedSize;
        }
    }
}

