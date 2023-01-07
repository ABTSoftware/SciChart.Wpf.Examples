// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SplineChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.CreateACustomChart.SplineLineSeries
{
    /// <summary>
    /// Interaction logic for CustomChartExampleView.xaml
    /// </summary>
    public partial class SplineChartExampleView : UserControl
    {
        public SplineChartExampleView()
        {
            InitializeComponent();
        }

        private void SplineChartExampleView_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a DataSeries of type X=double, Y=double
            var originalData = new XyDataSeries<double, double>() {SeriesName = "Original"};
            var splineData = new XyDataSeries<double, double>() { SeriesName = "Spline" };

            LineRenderSeries.DataSeries = originalData;
            SplineRenderSeries.DataSeries = splineData;

            var data = DataManager.Instance.GetSinewave(1.0, 0.0, 100, 25);

            // Append data to series. SciChart automatically redraws
            originalData.Append(data.XData, data.YData);
            splineData.Append(data.XData, data.YData);

            sciChart.ZoomExtents();
        }
    }
}
