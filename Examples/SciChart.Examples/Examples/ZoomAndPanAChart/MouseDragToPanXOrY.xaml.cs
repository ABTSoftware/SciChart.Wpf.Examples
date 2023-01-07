// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MouseDragToPanXOrY.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ZoomAndPanAChart
{
    public partial class MouseDragToPanXOrY : UserControl
    {
        public MouseDragToPanXOrY()
        {
            InitializeComponent();
        }

        private void MouseDragToPanXOrY_Loaded(object sender, RoutedEventArgs e)
        {
            // Performing multiple updates in a SuspendUpdates block is efficient as only one redraw is performed
            using (sciChart.SuspendUpdates())
            {
                // Add some data series
                var dataSeries0 = new XyDataSeries<double, double>();
                var dataSeries1 = new XyDataSeries<double, double>();

                var data2 = DataManager.Instance.GetFourierSeries(1.0, 0.1);
                var data1 = DataManager.Instance.GetDampedSinewave(1500, 3.0, 0.0, 0.005, data2.Count);

                // Append data to series.
                dataSeries0.Append(data1.XData, data1.YData);
                dataSeries1.Append(data2.XData, data2.YData);

                // Assign data-series to renderable series
                sciChart.RenderableSeries[0].DataSeries = dataSeries0;
                sciChart.RenderableSeries[1].DataSeries = dataSeries1;

                // Set initial zoom
                sciChart.XAxis.VisibleRange = new DoubleRange(3, 6);
                sciChart.ZoomExtentsY();
            }

            panXYDirection.ItemsSource = Enum.GetNames(typeof(XyDirection));
        }

        // Optional: demonstrates changing which direction the ZoomPanModifier operates in
        private void ZoomExtentsY_OnStateChanged(object sender, RoutedEventArgs e)
        {
            if (zoomPanModifier != null)
            {
                zoomPanModifier.ZoomExtentsY = chkZoomExtentsY.IsChecked == true;
            }
        }

        private void PanXYDirection_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (zoomPanModifier != null)
            {
                zoomPanModifier.XyDirection = (XyDirection) Enum.Parse(typeof(XyDirection), (string) panXYDirection.SelectedValue);

                if (zoomExtentsPanel != null)
                {
                    zoomExtentsPanel.Visibility = zoomPanModifier.XyDirection == XyDirection.XDirection
                        ? Visibility.Visible
                        : Visibility.Collapsed;
                }
            }
        }
    }
}