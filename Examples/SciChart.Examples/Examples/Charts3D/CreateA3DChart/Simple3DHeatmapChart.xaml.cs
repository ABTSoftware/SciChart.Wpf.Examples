// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Simple3DHeatmapChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Controls;
using SciChart.Charting3D.Model;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    /// <summary>
    /// Interaction logic for OrthogonalHeatmap3DChart.xaml
    /// </summary>
    public partial class Simple3DHeatmapChart : UserControl
    {
        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        private readonly Random _random = new Random();
        private const int XSize = 200;
        private const int ZSize = 200;
        private const double Angle = Math.PI * 2 * 20 / 30;
        private const int Cx = 150;
        private const int Cy = 100;

        public Simple3DHeatmapChart()
        {
            InitializeComponent();

            var meshDataSeries = new UniformGridDataSeries3D<double>(XSize, ZSize) { StepX = 1, StepZ = 1 };

            for (var x = 0; x < XSize; x++)
            {
                for (var z = 0; z < ZSize; z++)
                {
                    var v = (1 + Math.Sin(x * 0.04 + Angle)) * 50 + (1 + Math.Sin(z * 0.1 + Angle)) * 50 * (1 + Math.Sin(Angle * 2));

                    var r = Math.Sqrt((x - Cx) * (x - Cx) + (z - Cy) * (z - Cy));
                    var exp = Math.Max(0, 1 - r * 0.008);
                    meshDataSeries[z, x] = (v * exp + _random.NextDouble() * 50);
                }
            }
            //Trace.WriteLine(meshDataSeries.To2DArray().ToStringArray2D());
            surfaceMesh.DataSeries = meshDataSeries;
        }
    }
}