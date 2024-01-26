// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UsePointMarkers.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.StyleAChart
{
    public partial class UsePointMarkers : UserControl
    {
        public UsePointMarkers()
        {
            InitializeComponent();
        }
        private void UsePointMarkers_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataSeries1 = new UniformXyDataSeries<double> { SeriesName = "Ellipse Marker" };
            var dataSeries2 = new UniformXyDataSeries<double> { SeriesName = "Square Marker" };
            var dataSeries3 = new UniformXyDataSeries<double> { SeriesName = "Triangle Marker" };
            var dataSeries4 = new UniformXyDataSeries<double> { SeriesName = "Cross Marker" };
            var dataSeries5 = new UniformXyDataSeries<double> { SeriesName = "Sprite Marker" };

            const int dataSize = 30;
            var rnd = new Random(0);

            for (int i = 0; i < dataSize; i++) dataSeries1.Append(rnd.NextDouble());
            for (int i = 0; i < dataSize; i++) dataSeries2.Append(1 + rnd.NextDouble());
            for (int i = 0; i < dataSize; i++) dataSeries3.Append(1.8 + rnd.NextDouble());
            for (int i = 0; i < dataSize; i++) dataSeries4.Append(2.5 + rnd.NextDouble());
            for (int i = 0; i < dataSize; i++) dataSeries5.Append(3.5 + rnd.NextDouble());

            // insert a break into the line - we do this to test double.NaN for the point marker types 
            dataSeries1.Update(15, double.NaN);
            dataSeries2.Update(15, double.NaN);
            dataSeries3.Update(15, double.NaN);
            dataSeries4.Update(15, double.NaN);
            dataSeries5.Update(15, double.NaN);

            using (sciChart.SuspendUpdates())
            {
                lineSeries1.DataSeries = dataSeries1;
                lineSeries2.DataSeries = dataSeries2;
                lineSeries3.DataSeries = dataSeries3;
                lineSeries4.DataSeries = dataSeries4;
                lineSeries5.DataSeries = dataSeries5;
            }

            sciChart.ZoomExtents();
        }
    }
}