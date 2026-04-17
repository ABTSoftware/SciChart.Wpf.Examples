// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DailyMaxTemperatureHeatmapSurface.cs is part of the SCICHART® Examples. Permission
// is hereby granted to modify, create derivative works, distribute and publish any part of
// this source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System.Windows;
using SciChart.Charting.Visuals;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarHeatmapCustomization
{
    /// <summary>
    /// This calls provides an extension to the base SciChartSurface, additional functionality required for exporting the Daily Max Temperature Heatmap to an image
    /// </summary>
    public class DailyMaxTemperatureHeatmapSurface : SciChartSurface
    {
        protected override SciChartSurfaceBase CreateCloneOfSurfaceInMemory(Size newSize)
        {
            var clonedSurface = (SciChartSurface)base.CreateCloneOfSurfaceInMemory(newSize);

            // Apply custom Label Providers to the surface that is being cloned for export
            clonedSurface.XAxis.LabelProvider = new MonthNameAngularAxisLabelProvider();
            clonedSurface.YAxis.LabelProvider = new YearRadialAxisLabelProvider();

            return clonedSurface;
        }
    }
}
