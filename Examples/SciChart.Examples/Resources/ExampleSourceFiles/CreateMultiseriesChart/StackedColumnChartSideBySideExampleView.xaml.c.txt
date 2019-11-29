// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// StackedColumnChartSideBySideExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    /// Interaction logic for StackedColumnChartSideBySideExampleView.xaml
    /// </summary>
    public partial class StackedColumnChartSideBySideExampleView
    {
        public StackedColumnChartSideBySideExampleView()
        {
            InitializeComponent();
        }

        private void StackedColumnChartSideBySideExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var china = new[] { 1.269, 1.330, 1.356, 1.304 };
            var india = new[] { 1.004, 1.173, 1.236, 1.656 };
            var usa = new[] { 0.282, 0.310, 0.319, 0.439 };
            var indonesia = new[] { 0.214, 0.243, 0.254, 0.313 };
            var brazil = new[] { 0.176, 0.201, 0.203, 0.261 };
            var pakistan = new[] { 0.146, 0.184, 0.196, 0.276 };
            var nigeria = new[] { 0.123, 0.152, 0.177, 0.264 };
            var bangladesh = new[] { 0.130, 0.156, 0.166, 0.234 };
            var russia = new[] { 0.147, 0.139, 0.142, 0.109 };
            var japan = new[] { 0.126, 0.127, 0.127, 0.094 };
            var restOfTheWorld = new[] { 2.466, 2.829, 3.005, 4.306 };

            var chinaDataSeries = new XyDataSeries<int, double> { SeriesName = "China" };
            var indiaDataSeries = new XyDataSeries<int, double> { SeriesName = "India" };
            var usaDataSeries = new XyDataSeries<int, double> { SeriesName = "USA" };
            var indoneziaDataSeries = new XyDataSeries<int, double> { SeriesName = "Indonesia" };
            var brazilDataSeries = new XyDataSeries<int, double> { SeriesName = "Brazil" };
            var pakistanDataSeries = new XyDataSeries<int, double> { SeriesName = "Pakistan" };
            var nigeriaDataSeries = new XyDataSeries<int, double> { SeriesName = "Nigeria" };
            var bangladeshDataSeries = new XyDataSeries<int, double> { SeriesName = "Bangladesh" };
            var russiaDataSeries = new XyDataSeries<int, double> { SeriesName = "Russia" };
            var japanDataSeries = new XyDataSeries<int, double> { SeriesName = "Japan" };
            var restOfTheWorldDataSeries = new XyDataSeries<int, double> { SeriesName = "Rest Of The World" };
            var totalDataSeries = new XyDataSeries<int, double> { SeriesName = "Total" };

            for (int i = 0; i < 4; i++)
            {
                chinaDataSeries.Append(i, china[i]);
                if (i != 2)
                {
                    indiaDataSeries.Append(i, india[i]);
                    usaDataSeries.Append(i, usa[i]);
                    indoneziaDataSeries.Append(i, indonesia[i]);
                    brazilDataSeries.Append(i, brazil[i]);
                }
                else
                {
                    indiaDataSeries.Append(i, double.NaN);
                    usaDataSeries.Append(i, double.NaN);
                    indoneziaDataSeries.Append(i, double.NaN);
                    brazilDataSeries.Append(i, double.NaN);
                }
                pakistanDataSeries.Append(i, pakistan[i]);
                nigeriaDataSeries.Append(i, nigeria[i]);
                bangladeshDataSeries.Append(i, bangladesh[i]);
                russiaDataSeries.Append(i, russia[i]);
                japanDataSeries.Append(i, japan[i]);
                restOfTheWorldDataSeries.Append(i, restOfTheWorld[i]);
                totalDataSeries.Append(i, china[i] + india[i] + usa[i] + indonesia[i] + brazil[i] + pakistan[i] +
                                          nigeria[i] + bangladesh[i] + russia[i] + japan[i] + restOfTheWorld[i]);
            }
            using (SciChart.SuspendUpdates())
            {
                ChinaSeries.DataSeries = chinaDataSeries;
                IndiaSeries.DataSeries = indiaDataSeries;
                USASeries.DataSeries = usaDataSeries;
                IndonesiaSeries.DataSeries = indoneziaDataSeries;
                BrazilSeries.DataSeries = brazilDataSeries;
                PakistanSeries.DataSeries = pakistanDataSeries;
                NigeriaSeries.DataSeries = nigeriaDataSeries;
                BangladeshSeries.DataSeries = bangladeshDataSeries;
                RussiaSeries.DataSeries = russiaDataSeries;
                JapanSeries.DataSeries = japanDataSeries;
                RestOfTheWorldSeries.DataSeries = restOfTheWorldDataSeries;
            }

            SciChart.ZoomExtents();
        }
    }
}