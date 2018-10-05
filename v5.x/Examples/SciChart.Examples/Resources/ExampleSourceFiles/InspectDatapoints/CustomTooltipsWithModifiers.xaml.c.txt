// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomTooltipsWithModifiers.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.InspectDatapoints
{
    /// <summary>
    /// Interaction logic for CustomTooltipsWithModifiers.xaml
    /// </summary>
    public partial class CustomTooltipsWithModifiers : UserControl
    {
        private readonly Random _random = new Random(251916);
        const int Count = 2000;

        public CustomTooltipsWithModifiers()
        {
            Loaded += CustomTooltipsLoaded;
            InitializeComponent();
        }

        private void CustomTooltipsLoaded(object sender, RoutedEventArgs e)
        {
            // Batch updates with one redraw
            using (SciChart.SuspendUpdates())
            {
                // Add four data-series
                var dataSeries0 = new XyDataSeries<double, double>();
                var dataSeries1 = new XyDataSeries<double, double>();

                SciChart.RenderableSeries[0].DataSeries = FillData(dataSeries0, "Series #1");
                SciChart.RenderableSeries[1].DataSeries = FillData(dataSeries1, "Series #2");
            }
        }

        private IDataSeries FillData(IXyDataSeries<double, double> dataSeries, string name)
        {
            var randomWalk = 10.0;

            // Generate the X,Y data with sequential dates on the X-Axis and slightly positively biased random walk on the Y-Axis
            var xBuffer = new double[Count];
            var yBuffer = new double[Count];
            for (int i = 0; i < Count; i++)
            {
                randomWalk += (_random.NextDouble() - 0.498);
                yBuffer[i] = randomWalk;
                xBuffer[i] = i;
            }

            // Buffer above and append all in one go to avoid multiple recalculations of series range
            dataSeries.Append(xBuffer, yBuffer);
            dataSeries.SeriesName = name;

            return dataSeries;
        }
    }
}