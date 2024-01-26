// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UniformHeatmapPeakDetection.xaml.cs is part of the SCICHART® Examples. Permission
// is hereby granted to modify, create derivative works, distribute and publish any part
// of this source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.HeatmapChartTypes.UniformHeatmapPeakDetection
{
    public partial class UniformHeatmapPeakDetection : UserControl
    {
        public UniformHeatmapPeakDetection()
        {
            InitializeComponent();

            var buffer = GetSpectrogramBuffer();
            HeatmapRenderableSeries.DataSeries = new UniformHeatmapDataSeries<int, int, double>(buffer, 0, 1, 0, 1);
        }

        private double[,] GetSpectrogramBuffer()
        {
            const int yCount = 100;
            const int xCount = 4_096;

            var spectrogramBuffer = new double[yCount, xCount];

            var random = new Random();
            var transform = new FFT2();

            transform.init(12);

            var re = new double[xCount];
            var im = new double[xCount];

            for (int y = 0; y < yCount; y++)
            {
                for (int i = 0; i < xCount; i++)
                {
                    re[i] = 2d * Math.Sin(2 * Math.PI * i / 20) +
                            5 * Math.Sin(2 * Math.PI * i / 10) -
                            2d * random.NextDouble();
                    im[i] = -10;
                }

                transform.run(re, im);

                for (int i = 0; i < xCount; i++)
                {
                    var mag = Math.Sqrt(re[i] * re[i] + im[i] * im[i]);
                    im[i] = i;

                    spectrogramBuffer[y, i] = 20 * Math.Log10(mag / xCount);
                }
            }

            return spectrogramBuffer;
        }
    }
}