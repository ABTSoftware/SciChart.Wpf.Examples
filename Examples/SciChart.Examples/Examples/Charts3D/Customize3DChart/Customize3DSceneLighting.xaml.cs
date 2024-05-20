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
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart
{
    public partial class Customize3DSceneLighting : UserControl
    {
        private readonly Vector3 _lightDirection = Vector3.Zero;

        public Customize3DSceneLighting()
        {
            InitializeComponent();

            Unloaded += (s, e) => _lightDirection.SafeDispose();

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

        private void SetLightDirection()
        {
            if (sciChart?.IsLoaded != true) return;

            var lightVectorX = (float)lightSliderX.Value;
            var lightVectorY = (float)lightSliderY.Value;
            var lightVectorZ = (float)lightSliderZ.Value;

            _lightDirection.Assign(lightVectorX, lightVectorY, lightVectorZ);

            sciChart.Viewport3D.LightingController.SetLightDirection(_lightDirection);
        }

        private void LightSlider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SetLightDirection();
        }

        private void LightMode_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sciChart?.IsLoaded != true) return;

            if (lightModeComboBox.SelectedItem is MainLightMode lightMode)
            {
                sciChart.Viewport3D.LightingController.LightMode = lightMode;

                lightSlidersPanel.IsEnabled = false;

                if (lightMode == MainLightMode.GlobalSpace)
                {
                    SetLightDirection();

                    lightSlidersPanel.IsEnabled = true;
                }
            }
        }

        private void SciChart_OnRendered(object sender, EventArgs e)
        {
            if (sciChart?.IsLoaded != true) return;

            var cameraTarget = sciChart.Viewport3D.CameraController.Target;
            var cameraPosition = sciChart.Viewport3D.CameraController.Position;
            var cameraDirection = cameraTarget - cameraPosition;

            cameraDirection.Normalize();

            using (cameraDirection)
            {
                labelCameraX.Text = cameraDirection.X.ToString("F2");
                labelCameraY.Text = cameraDirection.Y.ToString("F2");
                labelCameraZ.Text = cameraDirection.Z.ToString("F2");

                var lightMode = sciChart.Viewport3D.LightingController.LightMode;

                labelLightMode.Text = lightMode.ToString();

                if (lightMode == MainLightMode.CameraForward)
                {
                    labelLightX.Text = labelCameraX.Text;
                    labelLightY.Text = labelCameraY.Text;
                    labelLightZ.Text = labelCameraZ.Text;
                }
                else if (lightMode == MainLightMode.GlobalSpace)
                {
                    labelLightX.Text = _lightDirection.X.ToString("F2");
                    labelLightY.Text = _lightDirection.Y.ToString("F2");
                    labelLightZ.Text = _lightDirection.Z.ToString("F2");
                }
                else if (lightMode == MainLightMode.None)
                {
                    labelLightX.Text = "N/A";
                    labelLightY.Text = "N/A";
                    labelLightZ.Text = "N/A";
                }
            }
        }
    }
}