// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateAUniformMesh3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Sandbox;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    [TestCase("3D Chart DLL Loading Test")]
    public partial class CreateAUniformMesh3DChart : UserControl
    {
        public CreateAUniformMesh3DChart()
        {
            InitializeComponent();

            int xSize = 25;
            int zSize = 25;
            var meshDataSeries = new UniformGridDataSeries3D<double>(xSize, zSize)
            {
                StepX = 1,
                StepZ = 1,
                SeriesName = "Uniform Surface Mesh",
            };

            for (int x = 0; x < xSize; x++)
            {
                for (int z = 0; z < zSize; z++)
                {
                    double y = Math.Sin(x * 0.2) / ((z+1) * 2);
                    meshDataSeries[z, x] = y;
                }
            }

            surfaceMeshRenderableSeries.DataSeries = meshDataSeries;
        }
    }
}
