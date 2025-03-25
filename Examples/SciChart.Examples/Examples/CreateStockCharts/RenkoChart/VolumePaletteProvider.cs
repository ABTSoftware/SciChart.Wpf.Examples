// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// VolumePaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.CreateStockCharts.RenkoChart
{
    public class VolumePaletteProvider : IStrokePaletteProvider, IFillPaletteProvider
    {
        public Brush UpVolumeBrush { get; set; }
        public Brush DownVolumeBrush { get; set; }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var isUpBar = (metadata as VolumeMetadata)?.IsUpBar ?? false;
            return isUpBar ? UpVolumeBrush.ExtractColor() : DownVolumeBrush.ExtractColor();
        }

        public Brush OverrideFillBrush(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var isUpBar = (metadata as VolumeMetadata)?.IsUpBar ?? false;
            return isUpBar ? UpVolumeBrush : DownVolumeBrush;
        }
    }
}
