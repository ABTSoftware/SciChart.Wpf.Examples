// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PolarUniformHeatmapUltrasound.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarHeatmapCustomization_Ultrasound
{
    /// <summary>
    /// Interaction logic for PolarUniformHeatmapUltrasound.xaml
    /// </summary>
    public partial class PolarUniformHeatmapUltrasound : UserControl
    {
        public PolarUniformHeatmapUltrasound()
        {
            InitializeComponent();

            // Create a DataSeries and will it with the ultrasound image data
            var data = DataManager.Instance.GetUltrasoundPolarHeatmapData();
            HeatmapRenderSeries.DataSeries = new UniformHeatmapDataSeries<int, int, double>(data, 0, 1, 0, 1);

            // Applies a custom LabelProvider to the Angular Axis to hide axis labels outside the specified range [0 - 256]
            AngularAxis.LabelProvider = new UltrasoundPolarLabelProvider(0, 256);

            // Applies a custom LabelProvider to the Radial Axis to hide axis labels outside the specified range (below zero)
            RadialAxis.LabelProvider = new UltrasoundPolarLabelProvider(0, double.MaxValue);
        }
    }
}
