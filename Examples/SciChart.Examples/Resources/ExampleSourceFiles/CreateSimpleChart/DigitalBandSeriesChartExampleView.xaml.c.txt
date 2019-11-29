// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DigitalBandSeriesChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    /// <summary>
    /// Interaction logic for BandSeriesChartExampleView.xaml
    /// </summary>
    public partial class DigitalBandSeriesChartExampleView : UserControl
    {
        public DigitalBandSeriesChartExampleView()
        {
            InitializeComponent();
        }

        private void DigitalBandSeriesChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Add a data series which handles X-Y0-Y1 data
            var dataSeries = new XyyDataSeries<double, double>();

            // Get some reference data to display
            var data = DataManager.Instance.GetDampedSinewave(1.0, 0.01, 1000);
            var moreData = DataManager.Instance.GetDampedSinewave(1.0, 0.005, 1000, 12);

            // Append data to series. SciChart automatically redraws
            dataSeries.Append(
                data.XData,
                data.YData, 
                moreData.YData);

            bandRenderSeries.DataSeries = dataSeries;
        }
    }
}
