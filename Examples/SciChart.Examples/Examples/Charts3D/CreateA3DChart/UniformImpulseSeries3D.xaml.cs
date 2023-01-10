// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UniformImpulseSeries3D.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using SciChart.Charting3D.Model;
using SciChart.Charting3D.PointMarkers;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    public partial class UniformImpulseSeries3D : UserControl
    {
        private const int CountX = 17;
        private const int CountZ = 30;
        public UniformImpulseSeries3D()
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
            var uniformDataSeries = new UniformGridDataSeries3D<double>(CountX, CountZ)
            {
                StepX = 7,
                StepZ = 130,
                StartX = 9,
                StartZ = 295,
                SeriesName = "Impulse Series 3D",
            };

            for (var x = 0; x < CountX; x++)
            {
                for (var z = 0; z < CountZ; z++)
                {
                    var y = Math.Sin(x * 0.25) / ((z + 1) * 2);
                    uniformDataSeries[z, x] = y;
                }
            }

            //Trace.WriteLine(uniformDataSeries.To2DArray().ToStringArray2D());
            ImpulseSeries3D.DataSeries = uniformDataSeries;

            PointMarkerCombo.SelectedIndex = 0;
        }

        private void PointMarkerCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ImpulseSeries3D != null && OpacitySlider != null && SizeSlider != null)
            {
                ImpulseSeries3D.PointMarker = (BaseMeshPointMarker3D)Activator.CreateInstance((Type)((ComboBox)sender).SelectedItem);
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
                ImpulseSeries3D.PointMarker.Opacity = ((Slider)sender).Value;
        }
    }
}