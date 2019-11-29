// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LeftRightYAxes.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    public partial class LeftRightYAxes : UserControl
    {
        public LeftRightYAxes()
        {
            InitializeComponent();
        }

        private void LeftRightAxesView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Add two data series. These are bound 1:1 to renderableSeries
            var leftDataSeries = new XyDataSeries<double, double>();
            var rightDataSeries = new XyDataSeries<double, double>();

            // Get some data
            var data1 = DataManager.Instance.GetFourierSeries(1.0, 0.1);
            var data2 = DataManager.Instance.GetDampedSinewave(3.0, 0.005, data1.Count);

            // Append data to series
            leftDataSeries.Append(data1.XData, data1.YData);
            rightDataSeries.Append(data2.XData, data2.YData);

            // Assign DataSeries to RenderableSeries
            // Note, you can also bind these as RenderableSeries.DataSeries is a DependencyProperty
            lineSeriesLeft.DataSeries = leftDataSeries;
            lineSeriesRight.DataSeries = rightDataSeries;

            sciChart.ZoomExtents();
        }
    }
}
