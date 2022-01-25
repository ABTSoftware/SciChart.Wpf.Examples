// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PolarChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting.Visuals.Axes;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    /// <summary>
    /// Interaction logic for PolarChartExampleView.xaml
    /// </summary>
    public partial class PolarChartExampleView : UserControl
    {
        private readonly string[] axisAlignment = {"Bottom", "Top", "Left", "Right"};

        public PolarChartExampleView()
        {
            InitializeComponent();

            xAxisAlignment.ItemsSource = axisAlignment;
            xAxisAlignment.SelectedItem = Enum.GetName(typeof(AxisAlignment), AxisAlignment.Bottom);

            yAxisAlignment.ItemsSource = axisAlignment;
            yAxisAlignment.SelectedItem = Enum.GetName(typeof(AxisAlignment), AxisAlignment.Left);
        }

        private void PolarChartExampleView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var dataSeries = new XyDataSeries<double, double>();
            lineRenderSeries.DataSeries = dataSeries;

            var data = DataManager.Instance.GetSquirlyWave();
            dataSeries.Append(data.XData, data.YData);

            sciChart.ZoomExtents();
        }

        private void OnXAxisAlignmentChanged(object sender, SelectionChangedEventArgs e)
        {
            if (xAxis != null)
                xAxis.AxisAlignment = (AxisAlignment) Enum.Parse(typeof(AxisAlignment), (string) e.AddedItems[0], true);
        }

        private void OnYAxisAlignmentChanged(object sender, SelectionChangedEventArgs e)
        {
            if (yAxis != null)
                yAxis.AxisAlignment = (AxisAlignment) Enum.Parse(typeof(AxisAlignment), (string) e.AddedItems[0], true);
        }
    }
}