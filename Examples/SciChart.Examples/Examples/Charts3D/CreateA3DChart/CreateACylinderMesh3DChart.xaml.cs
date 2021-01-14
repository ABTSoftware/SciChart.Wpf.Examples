// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateACylinderMesh3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.Charts3D.CreateASurfaceMeshChart
{
    public partial class CreateACylinderMesh3DChart : UserControl
    {
        public CreateACylinderMesh3DChart()
        {
            InitializeComponent();

            int countU = 40;
            int countV = 30;
            var meshDataSeries = new CylindroidDataSeries3D<double>(countU, countV)
            {
                SeriesName = "Cylindroid Mesh",
                A = 3,
                B = 3,
                H = 7
            };

            var random = new Random(0);
            for (var u = 0; u < countU; u++)
            {
                for (int v = 0; v < countV; v++)
                {
                    var weight = 1.0f - Math.Abs(v / (float) countV * 2.0f - 1.0f);
                    var offset = (float)random.NextDouble();
                    meshDataSeries[v, u] = offset * weight;
                }
            }

            cylinderMeshRenderableSeries.DataSeries = meshDataSeries;
            cylinderMeshRenderableSeries.DrawBackSide = true;
        }

        private void PaletteModeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cylinderMeshRenderableSeries == null)
                return;

            switch (((ContentControl)e.AddedItems[0]).Content.ToString())
            {
                case "Radial":
                    cylinderMeshRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Relative;
                    cylinderMeshRenderableSeries.PaletteMinimum = new Vector3(0.0f, 5.0f, 0.0f);
                    cylinderMeshRenderableSeries.PaletteMaximum = new Vector3(0.0f, 7.0f, 0.0f);
                    cylinderMeshRenderableSeries.PaletteRadialFactor = 1.0;
                    cylinderMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    cylinderMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    cylinderMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Axial":
                    cylinderMeshRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Absolute;
                    cylinderMeshRenderableSeries.PaletteMinimum = new Vector3(0.0f, -4.0f, 0.0f);
                    cylinderMeshRenderableSeries.PaletteMaximum = new Vector3(0.0f, 4.0f, 0.0f);
                    cylinderMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    cylinderMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 1.0f, 0.0f);
                    cylinderMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    cylinderMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Azimuthal":
                    cylinderMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    cylinderMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    cylinderMeshRenderableSeries.PaletteAzimuthalFactor = 1.0;
                    cylinderMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Polar":
                    cylinderMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    cylinderMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    cylinderMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    cylinderMeshRenderableSeries.PalettePolarFactor = 1.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
