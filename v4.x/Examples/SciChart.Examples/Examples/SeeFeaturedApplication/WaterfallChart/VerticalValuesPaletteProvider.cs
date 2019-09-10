using SciChart.Charting.Visuals.PaletteProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.WaterfallChart
{
    public class VerticalValuesPaletteProvider : DependencyObject, IStrokePaletteProvider
    {
        public int SelectedIndex { get; set; }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            if (rSeries.IsSelected)
            {
                return Colors.Aqua;
            }

            if (index == SelectedIndex)
            {
                return Colors.Goldenrod;
            }

            return null;
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }
    }
}
