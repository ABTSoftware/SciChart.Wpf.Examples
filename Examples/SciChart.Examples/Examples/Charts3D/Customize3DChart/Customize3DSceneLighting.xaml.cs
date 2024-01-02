// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Customize3DSceneLighting.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting3D;
using SciChart.Charting3D.Model;
using Viewport3D = SciChart.Charting3D.Viewport3D;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart
{
    public partial class Customize3DSceneLighting : UserControl
    {
        public Customize3DSceneLighting()
        {
            InitializeComponent();

            lightModeComboBox.ItemsSource = Enum.GetValues(typeof(MainLightMode));
            lightModeComboBox.SelectedItem = MainLightMode.CameraForward;

            int countU = 40;
            int countV = 40;

            var meshDataSeries = new EllipsoidDataSeries3D<double>(countU, countV)
            {
                SeriesName = "Sphere Mesh",
                A = 6,
                B = 6,
                C = 6
            };

            for (var u = 0; u < countU; u++)
            {
                for (int v = 0; v < countV; v++)
                {
                    meshDataSeries[v, u] = 0.5;
                }
            }

            sphereMesh.DataSeries = meshDataSeries;
        }

        private void LightSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sciChart?.Viewport3D is Viewport3D viewport)
            {
                var vector = viewport.GetMainLightDirection();

                vector.x = (float) lightSliderX.Value;
                vector.y = (float) lightSliderY.Value;
                vector.z = (float) lightSliderZ.Value;

                viewport.SetMainLightDirection(vector);
            }
        }

        private void LightMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lightModeComboBox.SelectedItem is MainLightMode lightMode)
            {
                lightSlidersPanel.IsEnabled = lightMode == MainLightMode.GlobalSpace;

                if (sciChart?.Viewport3D is Viewport3D viewport)
                {
                    viewport.SetMainLightMode(lightMode);

                    if (lightMode == MainLightMode.GlobalSpace)
                    {
                        var vector = viewport.GetMainLightDirection();

                        vector.x = (float) lightSliderX.Value;
                        vector.y = (float) lightSliderY.Value;
                        vector.z = (float) lightSliderZ.Value;

                        viewport.SetMainLightDirection(vector);
                    }
                }
            }
        }

        private void SciChart_OnRendered(object sender, EventArgs e)
        {
            if (sciChart.Viewport3D is Viewport3D viewport)
            {
                var lightMode = viewport.GetMainLightMode();
                labelLightMode.Text = lightMode.ToString();

                var cameraDirection =  viewport.CameraController.Target - viewport.CameraController.Position;
                cameraDirection.Normalize();

                labelCameraX.Text = cameraDirection.X.ToString("F2");
                labelCameraY.Text = cameraDirection.Y.ToString("F2");
                labelCameraZ.Text = cameraDirection.Z.ToString("F2");

                if (lightMode == MainLightMode.CameraForward)
                {
                    labelLightX.Text = labelCameraX.Text;
                    labelLightY.Text = labelCameraY.Text;
                    labelLightZ.Text = labelCameraZ.Text;
                }
                else if (lightMode == MainLightMode.GlobalSpace)
                {
                    var vector = viewport.GetMainLightDirection();

                    labelLightX.Text = vector.x.ToString("F2");
                    labelLightY.Text = vector.y.ToString("F2");
                    labelLightZ.Text = vector.z.ToString("F2");
                }
            }
        }
    }
}