// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// AudioAnalyzerPaletteProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using System.Windows.Media;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AudioAnalyzer
{
    public class AudioAnalyzerPaletteProvider : IFillPaletteProvider, IPointMarkerPaletteProvider, IStrokePaletteProvider
    {
        public HeatmapColorPalette Palette { get; set; }

        public double FillOpacity { get; set; } = 1.0;
        public double PointOpacity { get; set; } = 1.0;

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            if (Palette == null) return null;
            var color = GetColorInternal(rSeries, index);
            return color;
        }

        public PointPaletteInfo? OverridePointMarker(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            if (Palette == null) return null;
            var color = GetColorInternal(rSeries, index);
            color = Color.FromArgb((byte)(PointOpacity * 255), color.R, color.G, color.B);
            return new PointPaletteInfo() { Fill = color, Stroke = color };
        }


        public Brush OverrideFillBrush(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            if (Palette == null) return Brushes.Transparent;
            var color = GetColorInternal(rSeries, index);
            color = Color.FromArgb((byte)(FillOpacity * 255), color.R, color.G, color.B);
            return new SolidColorBrush(color);
        }

        private Color GetColorInternal(IRenderableSeries rSeries, int index)
        {
            var value = Convert.ToDouble(rSeries.DataSeries.XValues[index]);
            return Palette.GetColor(value).ToColor();
        }
    }
}
