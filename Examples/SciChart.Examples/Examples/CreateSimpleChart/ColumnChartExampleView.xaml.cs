// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ColumnChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    public partial class ColumnChartExampleView : UserControl
    {
        public ColumnChartExampleView()
        {
            InitializeComponent();
        }

        private void ColumnChartExampleViewOnLoaded(object sender, RoutedEventArgs e)
        {
            var dataSeries = new UniformXyDataSeries<double> { SeriesName = "Histogram" };

            var yValues = new[] { 0.1, 0.2, 0.4, 0.8, 1.1, 1.5, 2.4, 4.6, 8.1, 11.7, 14.4, 16.0, 13.7, 10.1, 6.4, 3.5, 2.5, 1.4, 0.4, 0.1};

            using (sciChart.SuspendUpdates())
            {
                for (int i = 0; i < yValues.Length; i++)
                {
                    // DataSeries for appending data
                    dataSeries.Append(yValues[i]);
                }

                columnSeries.DataSeries = dataSeries;
            }

            sciChart.ZoomExtents();
        }
    }
}