// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// PolarUniformHeatmap.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using System;
using System.Windows.Controls;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarUniformHeatmap
{
    public partial class PolarUniformHeatmap : UserControl
    {
        private readonly Random _random = new Random(0);

        public PolarUniformHeatmap()
        {
            InitializeComponent();

            // Generate 2D scalar field composed of sinusoidal spatial waves with added noise
            var data = GenerateHeatmapData(300, 200);

            // Create a UniformHeatmapDataSeries and assign it to PolarUniformHeatmapRenderableSeries defined in XAML
            var dataSeries = new UniformHeatmapDataSeries<int, int, double>(data, 0, 1, 0, 1) { SeriesName = "PolarUniformHeatmap" };
            HeatmapRenderSeries.DataSeries = dataSeries;
        }

        private double[,] GenerateHeatmapData(int heatmapWidth, int heatmapHeight)
        {
            double angle = Math.Round(Math.PI * 2 * 1 / 30, 3);
            var data = new double[heatmapHeight, heatmapWidth];

            // Generate 2D scalar field composed of sinusoidal spatial waves with added noise
            for (int x = 0; x < heatmapWidth; x++)
            {
                for (int y = 0; y < heatmapHeight; y++)
                {
                    var v = (1 + Math.Round(Math.Sin(x * 0.04 + angle), 3)) * 50 + (1 + Math.Round(Math.Sin(y * 0.1 + angle), 3)) * 50 * (1 + Math.Round(Math.Sin(angle * 2), 3));
                    var cx = 150; var cy = 100;
                    var r = Math.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                    var exp = Math.Max(0, 1 - r * 0.008);
                    data[y, x] = (v * exp + _random.NextDouble() * 10);
                }
            }

            return data;
        }
    }
}
