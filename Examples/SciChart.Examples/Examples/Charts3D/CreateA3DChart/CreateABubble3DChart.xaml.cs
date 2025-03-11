// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateABubble3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public partial class CreateABubble3DChart : UserControl
    {
        public CreateABubble3DChart()
        {
            InitializeComponent();

            PointMarkerCombo.Items.Add(typeof(SpherePointMarker3D));
            PointMarkerCombo.Items.Add(typeof(CubePointMarker3D));
            PointMarkerCombo.Items.Add(typeof(PyramidPointMarker3D));
            PointMarkerCombo.Items.Add(typeof(CylinderPointMarker3D));

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>() {SeriesName = "Colorful Bubble!"};

            const int count = 250;

            var random = new Random(0);

            DataManager.Instance.SetRandomSeed(0); // required only by some UIAutomationTests, to have consistent results between test runs 
            for (var i = 0; i < count; i++)
            {
                var x = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                var y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                var z = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);

                // Scale is a multiplier used to increase/decrease ScatterRenderableSeries3D.ScatterPointSize
                var scale = (float) ((random.NextDouble() + 0.5)*3.0);

                // Color is applied to PointMetadata3D and overrides the default ScatterRenderableSeries.Stroke property
                Color? randomColor = Color.FromArgb(0xFF, (byte) random.Next(50, 255), (byte) random.Next(50, 255), (byte) random.Next(50, 255));
                
                // To declare scale and colour, add a VertextData class as the w (fourth) parameter. 
                // The PointMetadata3D class also has other properties defining the behaviour of the XYZ point
                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(randomColor, scale));
            }

            ScatterSeries3D.DataSeries = xyzDataSeries3D;

            PointMarkerCombo.SelectedIndex = 0;
        }

        private void PointMarkerCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ScatterSeries3D != null && OpacitySlider != null && SizeSlider != null)
            {
                ScatterSeries3D.PointMarker = (BasePointMarker3D) Activator.CreateInstance((Type) ((ComboBox) sender).SelectedItem);
                ScatterSeries3D.PointMarker.Size = (float) SizeSlider.Value;
                ScatterSeries3D.PointMarker.Opacity = OpacitySlider.Value;
            }
        }

        private void SizeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ScatterSeries3D != null && ScatterSeries3D.PointMarker != null)
                ScatterSeries3D.PointMarker.Size = (float) ((Slider) sender).Value;
        }

        private void OpacitySlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ScatterSeries3D != null && ScatterSeries3D.PointMarker != null)
                ScatterSeries3D.PointMarker.Opacity = ((Slider) sender).Value;
        }
    }
}