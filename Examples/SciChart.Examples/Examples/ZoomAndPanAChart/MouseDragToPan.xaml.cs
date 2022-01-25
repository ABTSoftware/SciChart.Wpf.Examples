// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MouseDragToPan.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ZoomAndPanAChart
{
    public partial class MouseDragToPan : UserControl
    {
        public MouseDragToPan()
        {
            InitializeComponent();
        }

        private void MouseDragToPan_Loaded(object sender, RoutedEventArgs e)
        {
            // Performing multiple updates in a SuspendUpdates block is efficient as only one redraw is performed
            using (sciChart.SuspendUpdates())
            {
                // Create 3 DataSeries of type X=double, Y=double
                var dataSeries0 = new XyDataSeries<double, double>();
                var dataSeries1 = new XyDataSeries<double, double>();
                var dataSeries2 = new XyDataSeries<double, double>();

                var data1 = DataManager.Instance.GetDampedSinewave(300, 1.0, 0.0, 0.01, 1000);
                var data2 = DataManager.Instance.GetDampedSinewave(300, 1.0, 0.0, 0.024, 1000);
                var data3 = DataManager.Instance.GetDampedSinewave(300, 1.0, 0.0, 0.049, 1000);

                // Append data to series.
                dataSeries0.Append(data1.XData, data1.YData);
                dataSeries1.Append(data2.XData, data2.YData);
                dataSeries2.Append(data3.XData, data3.YData);

                // Assign DataSeries to RenderableSeries
                sciChart.RenderableSeries[0].DataSeries = dataSeries0;
                sciChart.RenderableSeries[1].DataSeries = dataSeries1;
                sciChart.RenderableSeries[2].DataSeries = dataSeries2;

                // Set initial zoom
                sciChart.XAxis.VisibleRange = new DoubleRange(3, 6);
                sciChart.ZoomExtentsY();
            }

            panSciChartOn.ItemsSource = new List<string>
            {
                ExecuteOn.MouseLeftButton.ToString(), 
                ExecuteOn.MouseMiddleButton.ToString(),
                ExecuteOn.MouseRightButton.ToString()
            };
        }

        // Optional: demonstrates changing which mouse button ZoomPanModifier reacts to
        private void PanSciChartOn_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (zoomPanModifier != null)
                zoomPanModifier.ExecuteOn = (ExecuteOn)Enum.Parse(typeof(ExecuteOn), (string)panSciChartOn.SelectedValue);
        }
    }
}
