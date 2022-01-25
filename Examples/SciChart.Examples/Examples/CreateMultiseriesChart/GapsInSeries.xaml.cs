// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// GapsInSeries.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    /// <summary>
    /// Interaction logic for GapsInSeries.xaml
    /// </summary>
    public partial class GapsInSeries : UserControl
    {
        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        //private readonly RandomWalkGenerator _random = new RandomWalkGenerator();

        public GapsInSeries()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var dataSeries = CreateDataSeries();

            renderableColumnSeries.DataSeries = dataSeries;
            renderableLineSeries.DataSeries = dataSeries;
            renderableMountainSeries.DataSeries = dataSeries;

            sciChart.ZoomExtents();
            sciChart.YAxis.VisibleRange = new DoubleRange(-1, 1);
        }

        private IXyDataSeries<double, double> CreateDataSeries()
        {
            var _random = new RandomWalkGenerator(seed: 0);

            var dataSeries = new XyDataSeries<double, double>();
            dataSeries.SeriesName = "Random Series";

            int i = 0;
            // Samples 0,1,2 append double.NaN
            for (; i < 3; i++)
            {
                dataSeries.Append(i, double.NaN);
            }

            // Samples 3,4,5,6 append values
            for (; i < 7; i++)
            {
                dataSeries.Append(i, _random.GetRandomDouble());
            }

            // Samples 7,8,9 append double.NaN
            for (; i < 10; i++)
            {
                dataSeries.Append(i, double.NaN);
            }

            // Samples 10,11,12,13 append values
            for (; i < 14; i++)
            {
                dataSeries.Append(i, -_random.GetRandomDouble());
            }

            // Samples 14,15,16 append double.NaN
            for (; i < 16; i++)
            {
                dataSeries.Append(i, double.NaN);
            }

            return dataSeries;
        }

        private void GapsCheckbox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (gapsCheckbox != null && closedLinesCheckbox != null)
            {
                if (gapsCheckbox.IsChecked == true)
                {
                    foreach (var rSeries in sciChart.RenderableSeries)
                    {
                        rSeries.DrawNaNAs = LineDrawMode.Gaps;
                    }
                    closedLinesCheckbox.IsChecked = false;
                }

                if (gapsCheckbox.IsChecked == false && closedLinesCheckbox.IsChecked == false)
                {
                    gapsCheckbox.IsChecked = true;
                }
            }
        }

        private void ClosedCheckbox_CheckedChanged(object sender, RoutedEventArgs e)
        {
            if (closedLinesCheckbox != null && gapsCheckbox != null)
            {
                if (closedLinesCheckbox.IsChecked == true)
                {
                    foreach (var rSeries in sciChart.RenderableSeries)
                    {
                        rSeries.DrawNaNAs = LineDrawMode.ClosedLines;
                    }
                    gapsCheckbox.IsChecked = false;
                }

                if (gapsCheckbox.IsChecked == false && closedLinesCheckbox.IsChecked == false)
                {
                    closedLinesCheckbox.IsChecked = true;
                }
            }
        }
    }
}
