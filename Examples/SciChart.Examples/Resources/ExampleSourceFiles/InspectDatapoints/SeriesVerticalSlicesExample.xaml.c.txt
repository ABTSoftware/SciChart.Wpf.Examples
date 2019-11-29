// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SeriesVerticalSlicesExample.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Annotations;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.InspectDatapoints
{
    /// <summary>
    /// Interaction logic for SeriesVerticalSlicesExample.xaml
    /// </summary>
    public partial class SeriesVerticalSlicesExample : UserControl
    {
        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        private readonly Random _random = new Random();

        public SeriesVerticalSlicesExample()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataSeries0 = new XyDataSeries<double, double> { SeriesName = "Curve A" };
            var dataSeries1 = new XyDataSeries<double, double> { SeriesName = "Curve B" };
            var dataSeries2 = new XyDataSeries<double, double> { SeriesName = "Curve C" };

            var data1 = DataManager.Instance.GetSinewave(0.8, 0.0, 1000, 3);
            var data2 = DataManager.Instance.GetSinewave(0.5, -1.0, 1000, 5);
            var data3 = DataManager.Instance.GetSinewave(0.7, 0.75, 1000, 7);

            dataSeries0.Append(data1.XData, data1.YData);
            dataSeries1.Append(data2.XData, data2.YData);
            dataSeries2.Append(data3.XData, data3.YData);

            using (this.sciChart.SuspendUpdates())
            {
                sciChart.RenderableSeries[0].DataSeries = dataSeries0;
                sciChart.RenderableSeries[1].DataSeries = dataSeries1;
                sciChart.RenderableSeries[2].DataSeries = dataSeries2;
            }

            sciChart.ZoomExtents();
        }

        private void OnCreateSliceClick(object sender, RoutedEventArgs e)
        {
            MouseButtonEventHandler mouseClick = null;
            mouseClick = (s, arg) =>
                 {
                     this.MouseLeftButtonUp -= mouseClick;
                     var mousePoint = arg.GetPosition((UIElement)this.sciChart.GridLinesPanel).X;

                     var slice = new VerticalLineAnnotation()
                     {
                         X1 = this.sciChart.XAxis.GetDataValue(mousePoint),
                         Style = (Style)Resources["sliceStyle"]
                     };

                     sliceModifier.VerticalLines.Add(slice);
                 };

            this.MouseLeftButtonUp += mouseClick;
        }

        private void OnDeleteSelectedSliceClick(object sender, RoutedEventArgs e)
        {
            var selectedSlices = sliceModifier.VerticalLines.Where(annotation => annotation.IsSelected).ToList();

            foreach (var slice in selectedSlices)
            {
                sciChart.Annotations.Remove(slice);
            }
        }
    }
}
