// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
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
            var dataSeries1 = new XyDataSeries<double, double>() { SeriesName = "Ellipse Marker" };
            var dataSeries2 = new XyDataSeries<double, double>() { SeriesName = "Square Marker" };
            var dataSeries3 = new XyDataSeries<double, double>() { SeriesName = "Triangle Marker" };
            var dataSeries4 = new XyDataSeries<double, double>() { SeriesName = "Cross Marker" };
            var dataSeries5 = new XyDataSeries<double, double>() { SeriesName = "Sprite Marker" };

            const int dataSize = 30;
            var rnd = new Random();
            for (int i = 0; i < dataSize; i++) dataSeries1.Append(i, rnd.NextDouble());
            for (int i = 0; i < dataSize; i++) dataSeries2.Append(i, 1 + rnd.NextDouble());
            for (int i = 0; i < dataSize; i++) dataSeries3.Append(i, 1.8 + rnd.NextDouble());
            for (int i = 0; i < dataSize; i++) dataSeries4.Append(i, 2.5 + rnd.NextDouble());
            for (int i = 0; i < dataSize; i++) dataSeries5.Append(i, 3.5 + rnd.NextDouble());

            // insert a break into the line - we do this to test double.NaN for the point marker types 
            dataSeries1.Update(dataSeries1.XValues[15], double.NaN);
            dataSeries2.Update(dataSeries1.XValues[15], double.NaN);
            dataSeries3.Update(dataSeries1.XValues[15], double.NaN);
            dataSeries4.Update(dataSeries1.XValues[15], double.NaN);
            dataSeries5.Update(dataSeries1.XValues[15], double.NaN);

            using (this.sciChart.SuspendUpdates())
            {
                this.lineSeries1.DataSeries = dataSeries1;
                this.lineSeries2.DataSeries = dataSeries2;
                this.lineSeries3.DataSeries = dataSeries3;
                this.lineSeries4.DataSeries = dataSeries4;
                this.lineSeries5.DataSeries = dataSeries5;
            }

            this.sciChart.ZoomExtents();
        }
    }
}
