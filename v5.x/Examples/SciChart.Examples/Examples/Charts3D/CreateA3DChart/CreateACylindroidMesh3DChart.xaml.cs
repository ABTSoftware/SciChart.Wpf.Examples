// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateACylindroidMesh3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public partial class CreateACylindroidMesh3DChart : UserControl
    {
        public CreateACylindroidMesh3DChart()
        {
            InitializeComponent();

            int countU = 40;
            int countV = 20;
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

            cylindroidMeshRenderableSeries.DataSeries = meshDataSeries;
            cylindroidMeshRenderableSeries.DrawBackSide = true;
        }

        private void PaletteModeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cylindroidMeshRenderableSeries == null)
                return;

            switch (((ContentControl)e.AddedItems[0]).Content.ToString())
            {
                case "Radial":
                    cylindroidMeshRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Relative;
                    cylindroidMeshRenderableSeries.PaletteMinimum = new Vector3(3.0f, 0.0f, 0.0f);
                    cylindroidMeshRenderableSeries.PaletteMaximum = new Vector3(4.0f, 0.0f, 0.0f);
                    cylindroidMeshRenderableSeries.PaletteRadialFactor = 1.0;
                    cylindroidMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    cylindroidMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    cylindroidMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Axial":
                    cylindroidMeshRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Absolute;
                    cylindroidMeshRenderableSeries.PaletteMinimum = new Vector3(0.0f, -4.0f, 0.0f);
                    cylindroidMeshRenderableSeries.PaletteMaximum = new Vector3(0.0f, 4.0f, 0.0f);
                    cylindroidMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    cylindroidMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 1.0f, 0.0f);
                    cylindroidMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    cylindroidMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Azimuthal":
                    cylindroidMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    cylindroidMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    cylindroidMeshRenderableSeries.PaletteAzimuthalFactor = 1.0;
                    cylindroidMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Polar":
                    cylindroidMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    cylindroidMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    cylindroidMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    cylindroidMeshRenderableSeries.PalettePolarFactor = 1.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
