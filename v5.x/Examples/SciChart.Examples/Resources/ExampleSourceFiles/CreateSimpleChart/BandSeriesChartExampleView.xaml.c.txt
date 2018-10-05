// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BandSeriesChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    /// <summary>
    /// Interaction logic for BandSeriesChartExampleView.xaml
    /// </summary>
    public partial class BandSeriesChartExampleView : UserControl
    {
        public BandSeriesChartExampleView()
        {
            InitializeComponent();
        }

        private void BandSeriesChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Set a DataSeries of type x=DateTime, y0=Double, y1=double on the RenderableSeries declared in XAML
            var series = new XyyDataSeries<double, double>();
            sciChart.RenderableSeries[0].DataSeries = series;

            // Get some data for the upper and lower band
            var data = DataManager.Instance.GetDampedSinewave(1.0, 0.01, 1000);
            var moreData = DataManager.Instance.GetDampedSinewave(1.0, 0.005, 1000, 12);

            // Append data to series. SciChart automatically redraws
            series.Append(
                data.XData,
                data.YData,
                moreData.YData);
        }
    }
}
