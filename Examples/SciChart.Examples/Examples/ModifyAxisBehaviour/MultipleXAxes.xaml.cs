// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MultipleXAxes.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    /// <summary>
    /// Interaction logic for MultipleXAxes.xaml
    /// </summary>
    public partial class MultipleXAxes : UserControl
    {
        // Used to generate Random Walk
        private Random _random = new Random(251916);
        const int Count = 2000;

        public MultipleXAxes()
        {
            InitializeComponent();
        }

        private void MultipleXAxes_Loaded(object sender, RoutedEventArgs e)
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
                blueLine.DataSeries = FillData(dataSeries0, "Blue Line");
                rosyLine.DataSeries = FillData(dataSeries1, "Rosy Line");
                aquaLine.DataSeries = FillData(dataSeries2, "Aqua Line");
                violetLine.DataSeries = FillData(dataSeries3, "Violet Line");
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
