// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
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
using SciChart.Charting3D;
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
                ScatterSeries3D.PointMarker = (BasePointMarker3D)Activator.CreateInstance((Type)((ComboBox)sender).SelectedItem);
                ScatterSeries3D.PointMarker.Fill = Colors.LimeGreen;
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

    /// <summary>
    /// Defines a Custom texture Point-marker for used with 3D RenderableSeries
    /// </summary>
    public class CustomPointMarker3D : BaseTexturePointMarker3D
    {
        private Texture2D _texture;

        /// <summary>
        /// Initializes the instance of <see cref="TrianglePointMarker3D"/>.
        /// </summary>
        public CustomPointMarker3D()
        {
            DefaultStyleKey = typeof(CustomPointMarker3D);
            _texture = new Texture2D(128, 128, TextureFormat.TEXTUREFORMAT_A8B8G8R8);
            uint[] pixelData = new uint[128 * 128];

            for (uint i = 0; i < 128; i++)
            {
                for (uint j = 0; j < 128; j++)
                {
                    uint i8 = 0;
                    if (i < 52 || i > 76)
                    {
                        if (j > 52 && j < 76)
                        {
                            i8 = 0xff;
                        }
                    }
                    else
                    {
                        i8 = 0xff;
                    }

                    pixelData[i + j * 128] = i8 | i8 << 8 | i8 << 16 | i8 << 24;
                }
            }
            _texture.WritePixels(pixelData);
        }

        /// <summary>
        /// Gets the <see cref="Texture2D" /> instance which is repeated across data-points
        /// </summary>
        public override Texture2D PointTexture
        {
            get { return _texture; }
        }       
    }
}