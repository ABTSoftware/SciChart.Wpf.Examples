// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
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

            var chinaDataSeries = new UniformXyDataSeries<double> { SeriesName = "China" };
            var indiaDataSeries = new UniformXyDataSeries<double> { SeriesName = "India" };
            var usaDataSeries = new UniformXyDataSeries<double> { SeriesName = "USA" };
            var indoneziaDataSeries = new UniformXyDataSeries<double> { SeriesName = "Indonesia" };
            var brazilDataSeries = new UniformXyDataSeries<double> { SeriesName = "Brazil" };
            var pakistanDataSeries = new UniformXyDataSeries<double> { SeriesName = "Pakistan" };
            var nigeriaDataSeries = new UniformXyDataSeries<double> { SeriesName = "Nigeria" };
            var bangladeshDataSeries = new UniformXyDataSeries<double> { SeriesName = "Bangladesh" };
            var russiaDataSeries = new UniformXyDataSeries<double> { SeriesName = "Russia" };
            var japanDataSeries = new UniformXyDataSeries<double> { SeriesName = "Japan" };
            var restOfTheWorldDataSeries = new UniformXyDataSeries<double> { SeriesName = "Rest Of The World" };
            var totalDataSeries = new UniformXyDataSeries<double> { SeriesName = "Total" };

            for (int i = 0; i < 4; i++)
            {
                chinaDataSeries.Append(china[i]);
                if (i != 2)
                {
                    indiaDataSeries.Append(india[i]);
                    usaDataSeries.Append(usa[i]);
                    indoneziaDataSeries.Append(indonesia[i]);
                    brazilDataSeries.Append(brazil[i]);
                }
                else
                {
                    indiaDataSeries.Append(double.NaN);
                    usaDataSeries.Append(double.NaN);
                    indoneziaDataSeries.Append(double.NaN);
                    brazilDataSeries.Append(double.NaN);
                }
                pakistanDataSeries.Append(pakistan[i]);
                nigeriaDataSeries.Append(nigeria[i]);
                bangladeshDataSeries.Append(bangladesh[i]);
                russiaDataSeries.Append(russia[i]);
                japanDataSeries.Append(japan[i]);
                restOfTheWorldDataSeries.Append(restOfTheWorld[i]);
                totalDataSeries.Append(china[i] + india[i] + usa[i] + indonesia[i] + brazil[i] + pakistan[i] +
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