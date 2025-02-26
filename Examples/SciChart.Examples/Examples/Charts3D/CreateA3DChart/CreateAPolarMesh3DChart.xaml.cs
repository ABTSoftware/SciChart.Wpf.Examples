// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CreateAPolarMesh3DChart.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public partial class CreateAPolarMesh3DChart : UserControl
    {
        public CreateAPolarMesh3DChart()
        {
            InitializeComponent();

            int countU = 30;
            int countV = 10;
            var meshDataSeries = new PolarDataSeries3D<double>(countU, countV, 0D, Math.PI * 1.75)
            {
                SeriesName = "Polar 3D Mesh",
                A = 1,
                B = 5
            };

            var random = new Random(0);
            for (var u = 0; u < countU; u++)
            {
                var weightU = 1.0f - Math.Abs(u / (float)countU * 2.0f - 1.0f);
                for (int v = 0; v < countV; v++)
                {
                    var weightV = 1.0f - Math.Abs(v / (float)countV * 2.0f - 1.0f);
                    var offset = (float)random.NextDouble();
                    meshDataSeries[v, u] = offset * weightU * weightV;
                }
            }

            polarMeshRenderableSeries.DataSeries = meshDataSeries;
        }

        private void PaletteModeComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (polarMeshRenderableSeries == null)
                return;

            switch (((ContentControl)e.AddedItems[0]).Content.ToString())
            {
                case "Radial":
                    polarMeshRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Relative;
                    polarMeshRenderableSeries.PaletteMinimum = new Vector3(0.0f, 0.0f, 0.0f);
                    polarMeshRenderableSeries.PaletteMaximum = new Vector3(0.0f, 0.5f, 0.0f);
                    polarMeshRenderableSeries.PaletteRadialFactor = 1.0;
                    polarMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    polarMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    polarMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Axial":
                    polarMeshRenderableSeries.PaletteMinMaxMode = FreeSurfacePaletteMinMaxMode.Absolute;
                    polarMeshRenderableSeries.PaletteMinimum = new Vector3(-5.0f, 0.0f, -5.0f);
                    polarMeshRenderableSeries.PaletteMaximum = new Vector3(5.0f, 0.0f, 5.0f);
                    polarMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    polarMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.5f, 0.0f, 0.5f);
                    polarMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    polarMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Azimuthal":
                    polarMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    polarMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    polarMeshRenderableSeries.PaletteAzimuthalFactor = 1.0;
                    polarMeshRenderableSeries.PalettePolarFactor = 0.0;
                    break;
                case "Polar":
                    polarMeshRenderableSeries.PaletteRadialFactor = 0.0;
                    polarMeshRenderableSeries.PaletteAxialFactor = new Vector3(0.0f, 0.0f, 0.0f);
                    polarMeshRenderableSeries.PaletteAzimuthalFactor = 0.0;
                    polarMeshRenderableSeries.PalettePolarFactor = 1.0;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
