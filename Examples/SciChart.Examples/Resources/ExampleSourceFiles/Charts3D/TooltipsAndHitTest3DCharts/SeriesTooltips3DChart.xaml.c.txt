// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SeriesTooltips3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Windows.Media.Media3D;
using SciChart.Charting3D.Model;

namespace SciChart.Examples.Examples.Charts3D.TooltipsAndHitTest3DCharts
{
    /// <summary>
    /// Interaction logic for SeriesTooltips3DChart.xaml
    /// </summary>
    public partial class SeriesTooltips3DChart : UserControl
    {
        private const int SegmentsCount = 25;
        private const double YAngle = -65;

        private readonly Color _blueColor = Color.FromArgb(0xFF, 0x00, 0x84, 0xCF);
        private readonly Color _redColor = Color.FromArgb(0xFF, 0xEE, 0x11, 0x10);
        private readonly double _rotationAngle;

        public SeriesTooltips3DChart()
        {
            InitializeComponent();

            _rotationAngle = 360 / 45;
            
            Loaded += (sender, args) => Initialize();
        }

        private void Initialize()
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            var currentAngle = 0d;
            for (var i = -SegmentsCount; i < SegmentsCount + 1; i++)
            {
                AddSegment(xyzDataSeries3D, i, currentAngle);
                currentAngle = (currentAngle + _rotationAngle)%360;
            }

            SciChart.RenderableSeries[0].DataSeries = xyzDataSeries3D;
        }

        private void AddSegment(IXyzDataSeries3D<double, double, double> xyzDataSeries3D, double y, double currentAngle)
        {
            var temp = RotatePoint(new Point(-4, 0), DegreeToRadian(currentAngle));
            var point = RotateAroundZ(new Point3D(temp.X, y, temp.Y), DegreeToRadian(YAngle));
            xyzDataSeries3D.Append(point.X, point.Y, point.Z, new PointMetadata3D(_blueColor));

            temp = RotatePoint(new Point(4, 0), DegreeToRadian(currentAngle));
            point = RotateAroundZ(new Point3D(temp.X, y, temp.Y), DegreeToRadian(YAngle));
            xyzDataSeries3D.Append(point.X, point.Y, point.Z, new PointMetadata3D(_redColor));
        }

        private Point RotatePoint(Point point, double angle)
        {
            var x = point.X*Math.Cos(angle) - point.Y*Math.Sin(angle);
            var y = point.X*Math.Sin(angle) + point.Y*Math.Cos(angle);

            return new Point(x, y);
        }

        private Point3D RotateAroundZ(Point3D point, double angle)
        {
            var x = point.X * Math.Cos(angle) - point.Y * Math.Sin(angle);
            var y = point.X * Math.Sin(angle) + point.Y * Math.Cos(angle);

            return new Point3D(x, y, point.Z);
        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }
    }
}