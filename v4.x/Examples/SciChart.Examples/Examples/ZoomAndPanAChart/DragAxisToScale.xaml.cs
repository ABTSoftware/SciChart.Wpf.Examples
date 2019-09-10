// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DragAxisToScale.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public partial class DragAxisToScale : UserControl
    {
        private bool _initialized;

        public DragAxisToScale()
        {
            InitializeComponent();
            _initialized = true;
        }

        private void DragAxisToScale_Loaded(object sender, RoutedEventArgs e)
        {
            // Performing multiple updates in a SuspendUpdates block is efficient as only one redraw is performed
            using (sciChart.SuspendUpdates())
            {
                // Create a dataset of type X=double, Y=double
                var dataSeries0 = new XyDataSeries<double, double>();
                var dataSeries1 = new XyDataSeries<double, double>();

                var data2 = DataManager.Instance.GetFourierSeries(1.0, 0.1);
                var data1 = DataManager.Instance.GetDampedSinewave(1500, 3.0, 0.0, 0.005, data2.Count);

                // Append data to series.
                dataSeries0.Append(data1.XData, data1.YData);
                dataSeries1.Append(data2.XData, data2.YData);

                // Assign data series to RenderableSeries
                // Note: you can also data-bind them in MVVM
                mountainSeries.DataSeries = dataSeries1;
                lineSeries.DataSeries = dataSeries0;

                // Set initial zoom
                sciChart.XAxis.VisibleRange = new DoubleRange(3, 6);
                sciChart.ZoomExtentsY();
                dragModes.ItemsSource = Enum.GetNames(typeof(AxisDragModes));
                dragXYDirection.ItemsSource = Enum.GetNames(typeof(XyDirection));
            }
        }

        private void DragModes_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_initialized) return;

            var mode = (AxisDragModes)Enum.Parse(typeof(AxisDragModes), (string)dragModes.SelectedValue, true);
            yAxisLeftDragmodifier.DragMode = mode;
            yAxisRightDragmodifier.DragMode = mode;
            xAxisDragModifier.DragMode = mode;
        }

        private void DragXYDirection_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!_initialized) return;

            switch ((XyDirection)Enum.Parse(typeof(XyDirection), (string)dragXYDirection.SelectedValue, true))
            {
                case XyDirection.XDirection:
                    yAxisLeftDragmodifier.IsEnabled = false;
                    yAxisRightDragmodifier.IsEnabled = false;
                    xAxisDragModifier.IsEnabled = true;
                    break;
                case XyDirection.YDirection:
                    yAxisLeftDragmodifier.IsEnabled = true;
                    yAxisRightDragmodifier.IsEnabled = true;
                    xAxisDragModifier.IsEnabled = false;
                    break;
                case XyDirection.XYDirection:
                    yAxisLeftDragmodifier.IsEnabled = true;
                    yAxisRightDragmodifier.IsEnabled = true;
                    xAxisDragModifier.IsEnabled = true;
                    break;
            }
        }
    }
}
