// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SparseColumn3D.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    public partial class SparseColumn3D : UserControl
    {
        private readonly Random _random = new Random();
        private const int Count = 15;

        public SparseColumn3D()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            for (var i = 1; i < Count; i++)
            {
                for (var j = 1; j <= Count; j++)
                {
                    if (i != j && i % 3 == 0 && j % 3 == 0)
                    {
                        var y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);

                        var randomColor = Color.FromArgb(0xFF, (byte)_random.Next(0, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255));

                        xyzDataSeries3D.Append(i, y, j, new PointMetadata3D(randomColor));
                    }
                }
            }

            SciChart.RenderableSeries[0].DataSeries = xyzDataSeries3D;
        }
    }
}