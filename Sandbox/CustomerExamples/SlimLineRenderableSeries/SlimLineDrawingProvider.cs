using SciChart.Charting.Visuals.RenderableSeries.DrawingProviders;
using SciChart.Data.Model;

namespace SlimLineRenderableSeriesExample
{
    public class SlimLineDrawingProvider : LineSeriesDrawingProvider<Point2DSeries>
    {
        public SlimLineDrawingProvider(SlimLineRenderableSeries renderSeries) : base(renderSeries)
        {
        }
    }
}