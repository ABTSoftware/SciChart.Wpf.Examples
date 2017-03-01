// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UniformColumn3D.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D.Extensions;
using SciChart.Charting3D.Model;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    public partial class UniformColumn3D : UserControl
    {
        private const int Count = 15;

        public UniformColumn3D()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var uniformDataSeries = new UniformGridDataSeries3D<double>(Count, Count)
            {
                StepX = 1, 
                StepZ = 1, 
                SeriesName = "Column 3D Data",
            };

            for (var x = 0; x < Count; x++)
            {
                for (var z = 0; z < Count; z++)
                {
                    var y = Math.Sin(x * 0.25) / ((z + 1) * 2);
                    uniformDataSeries[z, x] = y;
                }
            }

            //Trace.WriteLine(uniformDataSeries.To2DArray().ToStringArray2D());
            SciChart.RenderableSeries[0].DataSeries = uniformDataSeries;
        }
    }
}