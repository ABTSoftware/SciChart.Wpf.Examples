using System;
using System.Collections.Generic;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace State_Series_Example
{
    public class StatePaletteProvider : IPointMarkerPaletteProvider
    {
        public Color ErrorColor { get; set; }
        public Color WarningColor { get; set; }
        public Color NormalColor { get; set; }

        public StatePaletteProvider()
        {
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }

        public PointPaletteInfo? OverridePointMarker(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            State state = (State)((IList<double>)rSeries.DataSeries.YValues)[index];
            if (state == State.Error) return new PointPaletteInfo() { Fill = ErrorColor, Stroke = ErrorColor };
            if (state == State.Warning) return new PointPaletteInfo() { Fill = WarningColor, Stroke = WarningColor };
            if (state == State.Normal) return new PointPaletteInfo() { Fill = NormalColor, Stroke = NormalColor };

            throw new NotImplementedException();
        }
    }
}