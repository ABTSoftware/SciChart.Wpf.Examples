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
