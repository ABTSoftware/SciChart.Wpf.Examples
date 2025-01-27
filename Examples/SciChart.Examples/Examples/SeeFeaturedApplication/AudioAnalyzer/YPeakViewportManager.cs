// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// YPeakViewportManager.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Services;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;
using System;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AudioAnalyzer
{
    public class YPeakViewportManager : DefaultViewportManager
    {
        private IComparable _min;
        private IComparable _max;

        protected override IRange OnCalculateNewYRange(IAxis yAxis, RenderPassInfo renderPassInfo)
        {
            var range = base.OnCalculateNewYRange(yAxis, renderPassInfo);

            var min = FindMin(renderPassInfo.DataSeries);
            var max = FindMax(renderPassInfo.DataSeries);

            if(min == null || max == null) return range;

            if (IsNullOrInfinity(_min) || _min.CompareTo(min) > 0)
            {
                _min = min;
            }

            if (IsNullOrInfinity(_max) || _max.CompareTo(max) < 0)
            {
                _max = max;
            }

            range.Min = _min;
            range.Max = _max;

            return range;
        }

        private bool IsNullOrInfinity(IComparable value)
        {
            if(value is double d)
            {
                return double.IsInfinity(d);
            }

            return value == null;
        }

        private IComparable FindMin(IDataSeries[] series)
        {
            if (series.Length == 0) return null;
            var min = series[0].YMin;
            for (int i = 1; i < series.Length; i++)
            {
                var curr = series[i].YMin;
                if (min.CompareTo(curr) > 0)
                {
                    min = curr;
                }
            }
            return min;
        }

        private IComparable FindMax(IDataSeries[] series)
        {
            if (series.Length == 0) return null;
            var max = series[0].YMax;
            for (int i = 1; i < series.Length; i++)
            {
                var curr = series[i].YMax;
                if (max.CompareTo(curr) < 0)
                {
                    max = curr;
                }
            }
            return max;
        }
    }
}
