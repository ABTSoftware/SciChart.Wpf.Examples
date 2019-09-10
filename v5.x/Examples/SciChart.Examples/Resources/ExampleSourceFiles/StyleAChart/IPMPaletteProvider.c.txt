using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.StyleAChart
{
    public class PMPaletteProvider:IPointMarkerPaletteProvider
    {
        PointPaletteInfo _palette = new PointPaletteInfo{Stroke = Colors.Green, Fill = Colors.YellowGreen};

        public PointPaletteInfo? OverridePointMarker(IRenderableSeries series, int index, double xValue, double yValue,
            IPointMetadata metadata)
        {
            if (index%2 == 0)
            {
                return _palette;
            }
            return null;
        }
    }
}
