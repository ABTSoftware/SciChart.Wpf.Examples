// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// StackedMountainChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    /// <summary>
    /// Interaction logic for StackedMountainChartExampleView.xaml
    /// </summary>
    public partial class StackedMountainChartExampleView : UserControl
    {
        public StackedMountainChartExampleView()
        {
            InitializeComponent();

            var yValues1 = new[] { 4.0,  7,    5.2,  9.4,  3.8,  5.1, 7.5,  12.4, 14.6, 8.1, 11.7, 14.4, 16.0, 3.7, 5.1, 6.4, 3.5, 2.5, 12.4, 16.4, 7.1, 8.0, 9.0 };
            var yValues2 = new[] { 15.0, 10.1, 10.2, 10.4, 10.8, 1.1, 11.5, 3.4,  4.6,  0.1, 1.7, 14.4, 6.0, 13.7, 10.1, 8.4, 8.5, 12.5, 1.4, 0.4, 10.1, 5.0, 0.0 };

            var dataSeries1 = new XyDataSeries<double, double>() { SeriesName = "data1" };
            var dataSeries2 = new XyDataSeries<double, double>() { SeriesName = "data2" }; ;

            for (int i = 0; i < yValues1.Length; i++) dataSeries1.Append(i, yValues1[i]);
            for (int i = 0; i < yValues2.Length; i++) dataSeries2.Append(i, yValues2[i]);

            using (this.sciChart.SuspendUpdates())
            {
                this.mountainSeries1.DataSeries = dataSeries1;
                this.mountainSeries2.DataSeries = dataSeries2;
            }
        }
    }
}