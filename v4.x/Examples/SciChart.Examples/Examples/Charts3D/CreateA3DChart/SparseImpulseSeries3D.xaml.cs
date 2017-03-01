// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SparseImpulseSeries3D.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting3D.PointMarkers;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    public partial class SparseImpulseSeries3D : UserControl
    {
        private readonly Random _random = new Random();
        private const int Count = 15;

        public SparseImpulseSeries3D()
        {
            InitializeComponent();
            Loaded += OnLoaded;

            PointMarkerCombo.Items.Add(typeof(SpherePointMarker3D));
            PointMarkerCombo.Items.Add(typeof(CubePointMarker3D));
            PointMarkerCombo.Items.Add(typeof(PyramidPointMarker3D));
            PointMarkerCombo.Items.Add(typeof(CylinderPointMarker3D));
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            for (var i = 1; i < Count; i++)
            {
                for (var j = 1; j <= Count; j++)
                {
                    if (i != j && i %3 == 0 && j%3 ==0)
                    {
                        var y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);

                        var randomColor = Color.FromArgb(0xFF, (byte) _random.Next(0, 255), (byte) _random.Next(0, 255), (byte) _random.Next(0, 255));

                        xyzDataSeries3D.Append(i, y, j, new PointMetadata3D(randomColor));
                    }
                }
            }

            ImpulseSeries3D.DataSeries = xyzDataSeries3D;

            PointMarkerCombo.SelectedIndex = 0;
        }

        private void PointMarkerCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImpulseSeries3D != null && OpacitySlider != null && SizeSlider != null)
            {
                ImpulseSeries3D.PointMarker = (BasePointMarker3D)Activator.CreateInstance((Type)((ComboBox)sender).SelectedItem);
                ImpulseSeries3D.PointMarker.Fill = ImpulseSeries3D.Stroke;
                ImpulseSeries3D.PointMarker.Size = (float)SizeSlider.Value;
                ImpulseSeries3D.PointMarker.Opacity = OpacitySlider.Value;
            }
        }

        private void SizeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ImpulseSeries3D != null && ImpulseSeries3D.PointMarker != null)
                ImpulseSeries3D.PointMarker.Size = (float)((Slider)sender).Value;
        }

        private void OpacitySlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ImpulseSeries3D != null && ImpulseSeries3D.PointMarker != null)
                ImpulseSeries3D.PointMarker.Opacity = ((Slider) sender).Value;
        }
    }
}