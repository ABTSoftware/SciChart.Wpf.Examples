// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// TenorCurves3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D.Model;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.TenorCurves3DChart
{
    /// <summary>
    /// Interaction logic for TenorCurves3DChart.xaml
    /// </summary>
    public partial class TenorCurves3DChart : UserControl
    {
        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        private Random _random;

        public TenorCurves3DChart()
        {
            InitializeComponent();

            _random = new Random();
            int xSize = 25;
            int zSize = 25;
            var meshDataSeries = new UniformGridDataSeries3D<double,double,DateTime>(xSize, zSize);
            var lineDataSeries = new XyDataSeries<double>();
            var mountainDataSeries = new XyDataSeries<double, double>();

            meshDataSeries.StartZ = new DateTime(2010,1,1);
            meshDataSeries.StepZ = new TimeSpan(1,0,0,0).ToDateTime();

            double step;

            for (int x = 0; x < xSize; x++)
            {
                switch (x)
                {
                    case 5: case 10:
                        step = 0.309;
                        break;

                    case 4: case 9:
                        step = 0.303;
                        break;

                    case 6: case 11:
                        step = 0.303;
                        break;

                    case 23:
                        step = 0.291;
                        break;

                    case 22:
                        step = 0.294;
                        break;

                    case 24:
                        step = 0.295;
                        break;

                    default:
                        step = 0.3;
                        break;
                }

                for (int z = 0; z < zSize; z++)
                {
                    var y = (z != 0) ? Math.Pow((double)z + _random.NextDouble(), step) : Math.Pow((double)z + 1, 0.3);

                    meshDataSeries[z, x] = y;
                }

            }

            double average;
            for (int x = 0; x < xSize; x++)
            {
                average = 0;
                for (int z = 0; z < zSize; z++)
                {
                    average += meshDataSeries[x, z];
                }
                var y = average / 25;
                lineDataSeries.Append(x, y);
                mountainDataSeries.Append(x, meshDataSeries[12, x]);
            }

            mountainRenderSeries.DataSeries = mountainDataSeries;
            LineRenderableSeries.DataSeries = lineDataSeries;
            surfaceMeshRenderableSeries.Maximum = (double)meshDataSeries.YRange.Max;
            surfaceMeshRenderableSeries.Minimum = (double)meshDataSeries.YRange.Min;
            surfaceMeshRenderableSeries.DataSeries = meshDataSeries;
        }
    }
}