// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// QuadLeftRightAxes.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    public partial class QuadLeftRightAxes : UserControl
    {
        // Used to generate Random Walk
        private Random _random = new Random(251916);
        const int Count = 2000;

        public QuadLeftRightAxes()
        {
            InitializeComponent();
        }

        private void QuadLeftRightAxes_Loaded(object sender, RoutedEventArgs e)
        {
            // Batch updates with one redraw
            using (sciChart.SuspendUpdates())
            {
                // Add four data-series
                var dataSeries0 = new XyDataSeries<DateTime, double>();
                var dataSeries1 = new XyDataSeries<DateTime, double>();
                var dataSeries2 = new XyDataSeries<DateTime, double>();
                var dataSeries3 = new XyDataSeries<DateTime, double>();
                var dataSeries4 = new XyDataSeries<DateTime, double>();
                var dataSeries5 = new XyDataSeries<DateTime, double>();
                var dataSeries6 = new XyDataSeries<DateTime, double>();
                var dataSeries7 = new XyDataSeries<DateTime, double>();


                // Fill each data-series with 2,000 pts of X,Y values (Date,Double) and assign to RenderableSeries
                sciChart.RenderableSeries[0].DataSeries = FillData(dataSeries0, "Red Line");
                sciChart.RenderableSeries[1].DataSeries = FillData(dataSeries1, "Grey Line");
                sciChart.RenderableSeries[2].DataSeries = FillData(dataSeries2, "Orange Line");
                sciChart.RenderableSeries[3].DataSeries = FillData(dataSeries3, "Blue Line");
                sciChart.RenderableSeries[4].DataSeries = FillData(dataSeries4, "Another Blue");
                sciChart.RenderableSeries[5].DataSeries = FillData(dataSeries5, "Green Line");
                sciChart.RenderableSeries[6].DataSeries = FillData(dataSeries6, "Another Red");
                sciChart.RenderableSeries[7].DataSeries = FillData(dataSeries7, "Another Grey");
            }

            sciChart.ZoomExtents();
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
