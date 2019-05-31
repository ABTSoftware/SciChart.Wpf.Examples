// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ChartLegendsExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    public partial class MultipleLinesView : UserControl
    {
        public MultipleLinesView()
        {
            InitializeComponent();

            cboGetLegendFor.ItemsSource = Enum.GetNames(typeof (SourceMode));
            cboGetLegendFor.SelectedIndex = 1;

            cboLegendPlacement.ItemsSource = Enum.GetNames(typeof (LegendPlacement));
            cboLegendPlacement.SelectedItem = Enum.GetName(typeof (LegendPlacement), LegendPlacement.Inside);

            cboLegendOrientation.ItemsSource = Enum.GetNames(typeof (Orientation));
            cboLegendOrientation.SelectedItem = Enum.GetName(typeof (Orientation), Orientation.Vertical);

            cboHorizontalAlignment.ItemsSource = Enum.GetNames(typeof (HorizontalAlignment));
            cboHorizontalAlignment.SelectedItem = Enum.GetName(typeof(HorizontalAlignment), System.Windows.HorizontalAlignment.Left);

            cboVerticalAlignment.ItemsSource = Enum.GetNames(typeof(VerticalAlignment));
            cboVerticalAlignment.SelectedItem = Enum.GetName(typeof(VerticalAlignment), System.Windows.VerticalAlignment.Top);
        }

        private void MultipleLinesView_OnLoaded(object sender, RoutedEventArgs e)
        {
            // Add some data series of type X=double, Y=double
            var dataSeries0 = new XyDataSeries<double, double> {SeriesName = "Curve A"};
            var dataSeries1 = new XyDataSeries<double, double> {SeriesName = "Curve B"};
            var dataSeries2 = new XyDataSeries<double, double> {SeriesName = "Curve C"};
            var dataSeries3 = new XyDataSeries<double, double> { SeriesName = "Curve D" };

            var data1 = DataManager.Instance.GetStraightLine(1000, 1.0, 10);
            var data2 = DataManager.Instance.GetStraightLine(2000, 1.0, 10);
            var data3 = DataManager.Instance.GetStraightLine(3000, 1.0, 10);
            var data4 = DataManager.Instance.GetStraightLine(4000, 1.0, 10);

            // Append data to series.
            dataSeries0.Append(data1.XData, data1.YData);
            dataSeries1.Append(data2.XData, data2.YData);
            dataSeries2.Append(data3.XData, data3.YData);
            dataSeries3.Append(data4.XData, data4.YData);

            sciChart.RenderableSeries[0].DataSeries = dataSeries0;
            sciChart.RenderableSeries[1].DataSeries = dataSeries1;
            sciChart.RenderableSeries[2].DataSeries = dataSeries2;
            sciChart.RenderableSeries[3].DataSeries = dataSeries3;

            // Zoom to extents of the data
            sciChart.ZoomExtents();
        }

        private void OnLegendModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (legendModifier != null)
            {
                legendModifier.GetLegendDataFor = (SourceMode)Enum.Parse(typeof(SourceMode), (string)e.AddedItems[0], true);
            }
        }

        private void OnLegendPlacementChanged(object sender, SelectionChangedEventArgs e)
        {
            if (legendModifier != null)
            {
                legendModifier.LegendPlacement = (LegendPlacement)Enum.Parse(typeof(LegendPlacement), (string)e.AddedItems[0], true);
            }
        }

        private void OnLegendOrientationChanged(object sender, SelectionChangedEventArgs e)
        {
            if (legendModifier != null)
            {
                legendModifier.Orientation = (Orientation)Enum.Parse(typeof(Orientation), (string)e.AddedItems[0], true);
            }
        }

        private void OnLegendHorizontalAlignmentChanged(object sender, SelectionChangedEventArgs e)
        {
            if (legendModifier != null)
            {
                legendModifier.HorizontalAlignment = (HorizontalAlignment)Enum.Parse(typeof(HorizontalAlignment), (string)e.AddedItems[0], true);
            }
        }

        private void OnLegendVerticalAlignmentChanged(object sender, SelectionChangedEventArgs e)
        {
            if (legendModifier != null)
            {
                legendModifier.VerticalAlignment = (VerticalAlignment)Enum.Parse(typeof(VerticalAlignment), (string)e.AddedItems[0], true);
            }
        }
    }
}
