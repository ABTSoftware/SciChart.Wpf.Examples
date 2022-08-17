// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// NonUniformHeatmap.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;

namespace SciChart.Examples.Examples.HeatmapChartTypes.NonUniformHeatmap
{
    public partial class NonUniformHeatmap : UserControl
    {
        public NonUniformHeatmap()
        {
            InitializeComponent();

            heatmapSeries.DataSeries = CreateSeries();
        }

        private IDataSeries CreateSeries()
        {
            int w = 7, h = 4;
            var data = new double[h, w];

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    data[y, x] = 3.5 * ((h - y) * (w - x));
                }
            }

            var xRangeMapping = new int[] { 0, 10, 20, 26, 36, 60, 72, 84 };
            var yRangeMapping = new int[] { 100, 250, 390, 410, 600 };

            return new NonUniformHeatmapDataSeries<int, int, double>(data, i => xRangeMapping[i], i => yRangeMapping[i]);
        }
    }
}