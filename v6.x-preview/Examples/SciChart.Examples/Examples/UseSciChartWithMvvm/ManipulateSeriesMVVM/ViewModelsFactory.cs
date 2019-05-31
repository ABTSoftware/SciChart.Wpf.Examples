// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ViewModelsFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.ManipulateSeriesMVVM
{
    public static class ViewModelsFactory
    {
        private static readonly Random Random = new Random(251916);
        private const int Count = 50;

        public static IRenderableSeriesViewModel New(Type type, double valueShift, IDataSeries dataSeries = null)
        {
            var xyDataSeries = dataSeries ?? GetXyDataSeries(GetRanromWalk(valueShift));

            if (type == typeof (ColumnRenderableSeriesViewModel))
            {
                return new ColumnRenderableSeriesViewModel {DataSeries = xyDataSeries, StyleKey = "ColumnStyle"};
            }

            if (type == typeof (ImpulseRenderableSeriesViewModel))
            {
                return new ImpulseRenderableSeriesViewModel {DataSeries = xyDataSeries, StyleKey = "ImpulseStyle"};
            }

            if (type == typeof (LineRenderableSeriesViewModel))
            {
                return new LineRenderableSeriesViewModel {DataSeries = xyDataSeries, StyleKey = "LineStyle"};
            }

            if (type == typeof (MountainRenderableSeriesViewModel))
            {
                return new MountainRenderableSeriesViewModel {DataSeries = xyDataSeries, StyleKey = "MountainStyle"};
            }

            if (type == typeof (XyScatterRenderableSeriesViewModel))
            {
                return new XyScatterRenderableSeriesViewModel {DataSeries = xyDataSeries, StyleKey = "ScatterStyle"};
            }

            throw new NotImplementedException("Unsupported Series Type");
        }

// ReSharper disable PossibleMultipleEnumeration
        private static double[] GetRanromWalk(double valueShift)
        {
            var randomWalk = 1d;

            var yBuffer = new double[Count];
            for (var i = 0; i < Count; i++)
            {
                randomWalk += (Random.NextDouble() - 0.498);
                yBuffer[i] = randomWalk + valueShift;
            }

            return yBuffer;
        }

        private static IEnumerable<double> GetXValues(double[] data)
        {
            return Enumerable.Range(0, data.Length).Select(x => (double) x);
        }

        private static IXyDataSeries<double, double> GetXyDataSeries(double[] data)
        {
            var dataSeries = new XyDataSeries<double, double>();
            dataSeries.Append(GetXValues(data), data);

            return dataSeries;
        }
// ReSharper restore PossibleMultipleEnumeration
    }
}