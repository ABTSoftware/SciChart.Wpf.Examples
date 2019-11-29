// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateAPointLine3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Windows.Media;
using SciChart.Charting3D.Model;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    public partial class CreateAPointLine3DChart : UserControl
    {
        private const int Count = 100;

        public CreateAPointLine3DChart()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            var random = new Random(0);

            for (var i = 0; i < Count; i++)
            {
                var x = 5*Math.Sin(i);
                var y = i;
                var z = 5*Math.Cos(i);

                Color? randomColor = Color.FromArgb(0xFF, (byte) random.Next(50, 255), (byte) random.Next(50, 255), (byte) random.Next(50, 255));
                var scale = (float) ((random.NextDouble() + 0.5)*3.0);

                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(randomColor, scale));
            }

            PointLineSeries3D.DataSeries = xyzDataSeries3D;
        }
    }
}