﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
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
using SciChart.Charting.Model.Filters;
using SciChart.Charting3D.Model;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.TenorCurves3DChart
{
    /// <summary>
    /// Interaction logic for TenorCurves3DChart.xaml
    /// </summary>
    public partial class TenorCurves3DChart : UserControl
    {
        public TenorCurves3DChart()
        {
            InitializeComponent();

            var random = new Random();

            int xSize = 25;
            int zSize = 25;

            var meshDataSeries = new UniformGridDataSeries3D<double,double,DateTime>(xSize, zSize);
            var lineDataSeries = new XyDataSeries<double>();
            var mountainDataSeries = new XyDataSeries<double, double>();

            meshDataSeries.SeriesName = "Tenor Curves";
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
                    // Compute a slope function with some noise
                    var y = (z != 0) ? Math.Pow(z + random.NextDouble(), step) : Math.Pow((double)z + 1, 0.3);

                    // Compute a 3d parabola function 
                    var nX = x - xSize / 2;
                    var nZ = z - zSize / 2;
                    var parabola = (nX * nX) + (nZ * nZ);
                    
                    // Set the data
                    meshDataSeries[z, x] = y * (parabola + 50) * 0.1;
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

                lineDataSeries.Append(x, average / 25);
                mountainDataSeries.Append(x, meshDataSeries[12, x]);
            }

            surfaceMeshRenderableSeries.DataSeries = meshDataSeries;
            MountainSeries0.DataSeries = lineDataSeries.ToSpline(10);
            ScatterSeries0.DataSeries = lineDataSeries;
            MountainSeries1.DataSeries = mountainDataSeries.ToSpline(10);
            ScatterSeries1.DataSeries = mountainDataSeries;

            surfaceMeshRenderableSeries.Maximum = (double)meshDataSeries.YRange.Max;
            surfaceMeshRenderableSeries.Minimum = (double)meshDataSeries.YRange.Min;
        }
    }
}