// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateAPointCloud3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public partial class CreateAPointCloud3DChart : UserControl
    {
        public CreateAPointCloud3DChart()
        {
            InitializeComponent();

            PointMarkerCombo.Items.Add(typeof(EllipsePointMarker3D));
            PointMarkerCombo.Items.Add(typeof(QuadPointMarker3D));            
            PointMarkerCombo.Items.Add(typeof(TrianglePointMarker3D));
            PointMarkerCombo.Items.Add(typeof(PixelPointMarker3D));     
            PointMarkerCombo.Items.Add(typeof(CustomPointMarker3D));            

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            const int count = 10000;

            for (int i = 0; i < count; i++)
            {
                var x = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                var y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                var z = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);

                xyzDataSeries3D.Append(x, y, z);
            }

            ScatterSeries3D.DataSeries = xyzDataSeries3D;

            PointMarkerCombo.SelectedIndex = 0;
        }

        private void PointMarkerCombo_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {            
            if (ScatterSeries3D != null && OpacitySlider != null && SizeSlider != null)
            {
                var pmType = (Type) ((ComboBox) sender).SelectedItem;
                
                // Special case. CustomPointMarker is defined in the XAML as it contains a custom brush 
                if (pmType == typeof(CustomPointMarker3D))
                {
                    var pointMarker = (CustomPointMarker3D)this.TryFindResource("CustomPointMarkerResource");
                    ScatterSeries3D.PointMarker = pointMarker;
                }
                else
                {
                    // Create an instance of the pointmarker we want to draw 
                    var pointMarker = (BasePointMarker3D)Activator.CreateInstance(pmType);
                    ScatterSeries3D.PointMarker = pointMarker;
                }

                ScatterSeries3D.PointMarker.Fill = Color.FromArgb(0x77, 0xAD, 0xFF, 0x2F);
                ScatterSeries3D.PointMarker.Size = (float)SizeSlider.Value;
                ScatterSeries3D.PointMarker.Opacity = OpacitySlider.Value;
            }
        }

        private void SizeSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ScatterSeries3D != null && ScatterSeries3D.PointMarker != null)
                ScatterSeries3D.PointMarker.Size = (float)((Slider)sender).Value;
        }

        private void OpacitySlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ScatterSeries3D != null && ScatterSeries3D.PointMarker != null)
                ScatterSeries3D.PointMarker.Opacity = ((Slider)sender).Value;
        }
    }   
}