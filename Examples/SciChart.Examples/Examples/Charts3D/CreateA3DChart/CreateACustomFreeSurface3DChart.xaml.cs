// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateACustomFreeSurface3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows.Controls;
using SciChart.Charting3D;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.RenderableSeries;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    public partial class CreateACustomFreeSurface3DChart : UserControl
    {
        public CreateACustomFreeSurface3DChart()
        {
            InitializeComponent();

            int countU = 30;
            int countV = 30;
            var meshDataSeries = new CustomFreeSurfaceDataSeries3D<double>(countU, countV,
                (u, v) => 5.0 + Math.Sin(5 * (u + v)),
                (u, v) => u,
                (u, v) => v,
                (r, theta, phi) => r * Math.Sin(theta) * Math.Cos(phi),
                (r, theta, phi) => r * Math.Cos(theta),
                (r, theta, phi) => r * Math.Sin(theta) * Math.Sin(phi))
            {
                SeriesName = "Custom Free Surface"
            };

            customFreeSurfaceRenderableSeries.DataSeries = meshDataSeries;
        }

        private void PaletteModeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (customFreeSurfaceRenderableSeries == null)
                return;

            switch (((ContentControl)e.AddedItems[0]).Content.ToString())
            {
                case "Radial":
                    customFreeSurfaceRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Relative;
                    customFreeSurfaceRenderableSeries.PaletteMinimum = new Vector3(0.0f, 5.0f, 0.0f);
                    customFreeSurfaceRenderableSeries.PaletteMaximum = new Vector3(0.0f, 7.0f, 0.0f);
                    customFreeSurfaceRenderableSeries.PaletteRadialFactor = 1.0;
                    customFreeSurfaceRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    customFreeSurfaceRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    customFreeSurfaceRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Axial":
                    customFreeSurfaceRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Absolute;
                    customFreeSurfaceRenderableSeries.PaletteMinimum = new Vector3(0.0f, -2.0f, 0.0f);
                    customFreeSurfaceRenderableSeries.PaletteMaximum = new Vector3(0.0f, 2.0f, 0.0f);
                    customFreeSurfaceRenderableSeries.PaletteRadialFactor = 0.0;
                    customFreeSurfaceRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 1.0f, 0.0f);
                    customFreeSurfaceRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    customFreeSurfaceRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Azimuthal":
                    customFreeSurfaceRenderableSeries.PaletteRadialFactor = 0.0;
                    customFreeSurfaceRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    customFreeSurfaceRenderableSeries.PaletteAzimuthalFactor = 1.0;
                    customFreeSurfaceRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Polar":
                    customFreeSurfaceRenderableSeries.PaletteRadialFactor = 0.0;
                    customFreeSurfaceRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    customFreeSurfaceRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    customFreeSurfaceRenderableSeries.PalettePolarFactor = 1.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
