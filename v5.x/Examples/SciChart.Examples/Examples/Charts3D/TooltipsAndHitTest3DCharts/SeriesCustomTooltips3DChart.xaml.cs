// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SeriesCustomTooltips3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting3D.Model;

namespace SciChart.Examples.Examples.Charts3D.TooltipsAndHitTest3DCharts
{
    /// <summary>
    /// Interaction logic for SeriesCustomTooltips3DChart.xaml
    /// </summary>
    public partial class SeriesCustomTooltips3DChart : UserControl
    {
        private const int Count = 500;

        private readonly Random _random = new Random();

        public SeriesCustomTooltips3DChart()
        {
            InitializeComponent();
            Loaded += (sender, args) =>
            {
                Initialize();
                SciChart.ZoomExtents();
            };
        }

        private void Initialize()
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            for (int i = 0, pointIndex = 0; i < Count; i++)
            {
                var m1 = _random.Next(2) == 0 ? -1 : 1;
                var m2 = _random.Next(2) == 0 ? -1 : 1;
                var x1 = _random.NextDouble() * m1;
                var x2 = _random.NextDouble() * m2;

                if (x1 * x1 + x2 * x2 > 1) continue;

                var x = 2 * x1 * Math.Sqrt(1 - x1 * x1 - x2 * x2);
                var y = 2 * x2 * Math.Sqrt(1 - x1 * x1 - x2 * x2);
                var z = 1 - 2 * (x1 * x1 + x2 * x2);

                // Append an XYZ Point with random color
                // Set the PointMetadata.Tag which we bind to in the View 
                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(GetRandomColor(), 3.0f, false, 
                    string.Format("PointMetadata Index {0}", ++pointIndex)));
            }

            SciChart.RenderableSeries[0].DataSeries = xyzDataSeries3D;
        }

        private Color GetRandomColor()
        {
            return Color.FromArgb(255, (byte)_random.Next(0, 255), (byte)_random.Next(0, 255),
                (byte)_random.Next(0, 255));
        }
    }    
}