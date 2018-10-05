// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// StackedColumnChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    /// <summary>
    /// Interaction logic for StackedColumnChartExampleView.xaml
    /// </summary>
    public partial class StackedColumnChartExampleView
    {
        public StackedColumnChartExampleView()
        {
            InitializeComponent();
        }

        private void StackedColumnChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var porkData = new double[] { 10, 13, 7, 16, 4, 6, 20, 14, 16, 10, 24, 11 };
            var vealData = new double[] { 12, 17, 21, 15, 19, 18, 13, 21, 22, 20, 5, 10 };
            var tomatoesData = new double[] { 7, 30, 27, 24, 21, 15, 17, 26, 22, 28, 21, 22 };
            var cucumberData = new double[] { 16, 10, 9, 8, 22, 14, 12, 27, 25, 23, 17, 17 };
            var pepperData = new double[] { 7, 24, 21, 11, 19, 17, 14, 27, 26, 22, 28, 16 };

            var dataSeries1 = new XyDataSeries<double, double> { SeriesName = "Pork" };
            var dataSeries2 = new XyDataSeries<double, double> { SeriesName = "Veal" };
            var dataSeries3 = new XyDataSeries<double, double> { SeriesName = "Tomato" };
            var dataSeries4 = new XyDataSeries<double, double> { SeriesName = "Cucumber" };
            var dataSeries5 = new XyDataSeries<double, double> { SeriesName = "Pepper" };

            const int data = 1992;
            for (int i = 0; i < porkData.Length; i++)
            {
                dataSeries1.Append(data + i, porkData[i]);
                dataSeries2.Append(data + i, vealData[i]);
                dataSeries3.Append(data + i, tomatoesData[i]);
                dataSeries4.Append(data + i, cucumberData[i]);
                dataSeries5.Append(data + i, pepperData[i]);
            }

            using (SciChart.SuspendUpdates())
            {
                SciChart.RenderableSeries[0].DataSeries = dataSeries1;
                SciChart.RenderableSeries[1].DataSeries = dataSeries2;
                SciChart.RenderableSeries[2].DataSeries = dataSeries3;
                SciChart.RenderableSeries[3].DataSeries = dataSeries4;
                SciChart.RenderableSeries[4].DataSeries = dataSeries5;
            }

            SciChart.ZoomExtents();
        }
    }
}