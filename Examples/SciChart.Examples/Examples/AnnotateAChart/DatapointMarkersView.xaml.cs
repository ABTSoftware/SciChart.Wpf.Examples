// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DatapointMarkersView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.AnnotateAChart
{
    public partial class DatapointMarkersView : UserControl
    {
        public DatapointMarkersView()
        {
            InitializeComponent();
            Loaded += DatapointMarkersViewLoaded;
        }

        private void DatapointMarkersViewLoaded(object sender, RoutedEventArgs e)
        {
            var seriesA = new UniformXyDataSeries<double>(0d, 1d);
            var seriesB = new UniformXyDataSeries<double>(0d, 1d);

            seriesA.SeriesName = "Sinewave A";
            seriesB.SeriesName = "Sinewave B";

            double count = 100.0;
            double k = 2 * Math.PI / 30.0;
            for (int i = 0; i < (int)count; i++)
            {
                var phi = k * i;
                seriesA.Append((1.0 + i / count) * Math.Sin(phi));
                seriesB.Append((0.5 + i / count) * Math.Sin(phi));
            }

            sciChart.RenderableSeries[0].DataSeries = seriesA;
            sciChart.RenderableSeries[1].DataSeries = seriesB;
        }
    }
}
