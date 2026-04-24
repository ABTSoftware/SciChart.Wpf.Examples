// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// LinqExtension.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;

namespace SciChart.Examples.Demo.Helpers
{
    public static class LinqExtension
    {
        private static readonly Random _rnd = new(Environment.TickCount);

        public static List<T> ShuffleToList<T>(this IEnumerable<T> source)
        {
            if (source?.Any() != true)
            {
                throw new InvalidOperationException("Cannot shuffle null or empty set");
            }

            var list = source.ToList();
            var i = list.Count;

            while (i > 1)
            {
                i--;
                var j = _rnd.Next(i + 1);

#pragma warning disable IDE0180

                T value = list[j];
                list[j] = list[i];
                list[i] = value;

#pragma warning restore IDE0180
            }
            return list;
        }
    }
}