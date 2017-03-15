// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HeatMapWithTextInCellsExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.HeatmapChartTypes.NonUniformHeatmap
{
    /// <summary>
    /// Interaction logic for NonUniformHeatmap.xaml
    /// </summary>
    public partial class NonUniformHeatmap : UserControl
    {
        public NonUniformHeatmap()
        {
            InitializeComponent();

            heatmapSeries.ColorMap = new HeatmapColorPalette
            {
                GradientStops = new ObservableCollection<GradientStop>
                {
                    new GradientStop(Colors.Blue, 0), 
                    new GradientStop(Colors.White, 0.3), 
                    new GradientStop(Colors.Green, 0.5),
                    new GradientStop(Colors.Yellow, 0.7),
                    new GradientStop(Colors.Red, 1.0),
                },
                Minimum = 0,
                Maximum = 100
            };

            heatmapSeries.DataSeries = CreateSeries();
        }

        private IDataSeries CreateSeries()
        {
            int w = 7, h = 4;
            var data = new double[h, w];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    data[y, x] = 3.5 * ((h - y) * (w - x));
                }

            var xSteps = new int[] { 10, 10, 6, 10, 24, 12, 10 };
            var ySteps = new int[] { 100, 200, 100, 400 };

            return new NonUniformHeatmapDataSeries<int, int, double>(data, 0, xSteps, 0, ySteps);
        }
    }
}
