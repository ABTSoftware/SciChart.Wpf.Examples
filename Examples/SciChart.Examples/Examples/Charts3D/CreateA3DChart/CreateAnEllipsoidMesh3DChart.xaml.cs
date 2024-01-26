// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateAnEllipsoidMesh3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public partial class CreateAnEllipsoidMesh3DChart : UserControl
    {
        public CreateAnEllipsoidMesh3DChart()
        {
            InitializeComponent();

            InitializeComponent();

            int countU = 40;
            int countV = 20;
            var meshDataSeries = new EllipsoidDataSeries3D<double>(countU, countV)
            {
                SeriesName = "Ellipsoid Mesh",
                A = 6,
                B = 6,
                C = 6
            };

            var random = new Random(0);
            for (var u = 0; u < countU; u++)
            {
                for (int v = 0; v < countV; v++)
                {
                    var weight = 1.0f - Math.Abs(v / (float)countV * 2.0f - 1.0f);
                    var offset = (float)random.NextDouble();
                    meshDataSeries[v, u] = offset * weight;
                }
            }

            ellipsoidMeshRenderableSeries.DataSeries = meshDataSeries;
        }

        private void PaletteModeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ellipsoidMeshRenderableSeries == null)
                return;

            switch (((ContentControl)e.AddedItems[0]).Content.ToString())
            {
                case "Radial":
                    ellipsoidMeshRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Relative;
                    ellipsoidMeshRenderableSeries.PaletteMinimum = new Vector3(0.0f, 6.0f, 0.0f);
                    ellipsoidMeshRenderableSeries.PaletteMaximum = new Vector3(0.0f, 7.0f, 0.0f);
                    ellipsoidMeshRenderableSeries.PaletteRadialFactor = 1.0;
                    ellipsoidMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    ellipsoidMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    ellipsoidMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Axial":
                    ellipsoidMeshRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Absolute;
                    ellipsoidMeshRenderableSeries.PaletteMinimum = new Vector3(0.0f, -4.0f, 0.0f);
                    ellipsoidMeshRenderableSeries.PaletteMaximum = new Vector3(0.0f, 4.0f, 0.0f);
                    ellipsoidMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    ellipsoidMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 1.0f, 0.0f);
                    ellipsoidMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    ellipsoidMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Azimuthal":
                    ellipsoidMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    ellipsoidMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    ellipsoidMeshRenderableSeries.PaletteAzimuthalFactor = 1.0;
                    ellipsoidMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Polar":
                    ellipsoidMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    ellipsoidMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    ellipsoidMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    ellipsoidMeshRenderableSeries.PalettePolarFactor = 1.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
