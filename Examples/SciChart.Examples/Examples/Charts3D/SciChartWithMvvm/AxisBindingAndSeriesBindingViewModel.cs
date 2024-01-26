// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SelectScatterPoint3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Media;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.Charts3D.SciChartWithMvvm
{
    public class AxisBindingAndSeriesBindingViewModel : BaseViewModel
    {
        public AxisBase3DViewModel XAxis { get; }

        public AxisBase3DViewModel YAxis { get; }

        public AxisBase3DViewModel ZAxis { get; }

        public AxisBindingAndSeriesBindingViewModel()
        {
            YAxis = new TimeSpanAxis3DViewModel
            {
                StyleKey = "CustomTimeSpan3DStyle",
                FontFamily = new FontFamily("Broadway"),
                VisibleRange = new DateRange(DateTime.MinValue, DateTime.MinValue.AddSeconds(10))
            };

            XAxis = new DateTimeAxis3DViewModel
            {
                StyleKey = "CustomDateTime3DStyle",
                FontFamily = new FontFamily("Comic Sans MS")
            };

            ZAxis = new NumericAxis3DViewModel
            {
                StyleKey = "CustomNumeric3DStyle"
            };
        }
    }
}