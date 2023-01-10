// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
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
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
        public SeriesVerticalSlicesExample()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataSeries0 = new UniformXyDataSeries<double>(0d, 0.01) { SeriesName = "Curve A" };
            var dataSeries1 = new UniformXyDataSeries<double>(0d, 0.01) { SeriesName = "Curve B" };
            var dataSeries2 = new UniformXyDataSeries<double>(0d, 0.01) { SeriesName = "Curve C" };

            var data1 = DataManager.Instance.GetSinewaveYData(0.8, 0.0, 1000, 3);
            var data2 = DataManager.Instance.GetSinewaveYData(0.5, -1.0, 1000, 5);
            var data3 = DataManager.Instance.GetSinewaveYData(0.7, 0.75, 1000, 7);

            dataSeries0.Append(data1);
            dataSeries1.Append(data2);
            dataSeries2.Append(data3);

            using (sciChart.SuspendUpdates())
            {
                sciChart.RenderableSeries[0].DataSeries = dataSeries0;
                sciChart.RenderableSeries[1].DataSeries = dataSeries1;
                sciChart.RenderableSeries[2].DataSeries = dataSeries2;
            }

            sciChart.ZoomExtents();
        }

        private void OnCreateSliceClick(object sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton toggleButton)
            {
                if (toggleButton.IsChecked == true)
                {
                    MouseLeftButtonUp += OnMouseLeftButtonUp;
                }
                else
                {
                    MouseLeftButtonUp -= OnMouseLeftButtonUp;
                }
            }
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MouseLeftButtonUp -= OnMouseLeftButtonUp;

            var mousePoint = e.GetPosition((UIElement)sciChart.GridLinesPanel).X;

            var slice = new VerticalLineAnnotation()
            {
                X1 = sciChart.XAxis.GetDataValue(mousePoint),
                Style = (Style)Resources["sliceStyle"]
            };

            sliceModifier.VerticalLines.Add(slice);
            addVerticalSliceBtn.IsChecked = false;
        }

        private void OnDeleteSelectedSliceClick(object sender, RoutedEventArgs e)
        {
            var selectedSlices = sliceModifier.VerticalLines.Where(annotation => annotation.IsSelected).ToList();

            foreach (var slice in selectedSlices)
            {
                sliceModifier.VerticalLines.Remove(slice);
            }
        }
    }
}
