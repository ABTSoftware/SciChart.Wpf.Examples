// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ScrollBars.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.ZoomAndPanAChart
{
    /// <summary>
    /// Interaction logic for ScrollBars.xaml
    /// </summary>
    public partial class ScrollBars : UserControl
    {
        // Used to generate Random Walk
        private Random _random = new Random(251916);
        const int Count = 2000;

        public ScrollBars()
        {
            InitializeComponent();
        }

        private void ScrollBarsLoaded(object sender, RoutedEventArgs e)
        {
            // Batch updates with one redraw
            using (sciChart.SuspendUpdates())
            {
                // Add four data-series
                var dataSeries0 = new XyDataSeries<DateTime, double>();
                var dataSeries1 = new XyDataSeries<DateTime, double>();
                var dataSeries2 = new XyDataSeries<DateTime, double>();
                var dataSeries3 = new XyDataSeries<DateTime, double>();

                // Fill each dataset with 2,000 pts of X,Y values (Date,Double)
                // and ensure RenderableSeries has its datasource
                bottomLeftLine.DataSeries = FillData(dataSeries0, "Line #1");
                bottomLeftCurve.DataSeries = FillData(dataSeries1, "Curve #1");
                topRightLine.DataSeries = FillData(dataSeries2, "Line #2");
                topRightCurve.DataSeries = FillData(dataSeries3, "Curve #2");

                // Set Visible ranges to force scrollBars
                sciChart.YAxes[0].VisibleRange = new DoubleRange(12, 28);
                sciChart.YAxes[1].VisibleRange = new DoubleRange(-2, 8);
                sciChart.XAxes[0].VisibleRange = new DateRange(new DateTime(2012, 1, 5), new DateTime(2012, 1, 10));
                sciChart.XAxes[1].VisibleRange = new DateRange(new DateTime(2012, 1, 5), new DateTime(2012, 1, 10));
            }
        }

        private IDataSeries FillData(IXyDataSeries<DateTime, double> dataSeries, string name)
        {
            double randomWalk = 10.0;
            var startDate = new DateTime(2012, 01, 01);

            // Generate the X,Y data with sequential dates on the X-Axis and slightly positively biased random walk on the Y-Axis
            var xBuffer = new DateTime[Count];
            var yBuffer = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                randomWalk += (_random.NextDouble() - 0.498);
                yBuffer[i] = randomWalk;
                xBuffer[i] = startDate.AddMinutes(i * 10);
            }

            // Buffer above and append all in one go to avoid multiple recalculations of series range
            dataSeries.Append(xBuffer, yBuffer);
            dataSeries.SeriesName = name;

            return dataSeries;
        }
    }
}