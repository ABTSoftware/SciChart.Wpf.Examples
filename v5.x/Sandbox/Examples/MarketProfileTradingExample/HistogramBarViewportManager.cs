using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace SciChart.Sandbox.Examples.MarketProfileTradingExample
{
    class HistogramBarViewportManager: DefaultViewportManager
    {
        public override void AttachSciChartSurface(ISciChartSurface scs)
        {
            base.AttachSciChartSurface(scs);
            this.ParentSurface = scs;
        }

        public ISciChartSurface ParentSurface { get; private set; }

        protected override IRange OnCalculateNewXRange(IAxis xAxis)
        {
            var currentVisibleRange = xAxis.VisibleRange;

            var maxXRange = xAxis.DataRange;

            return new IndexRange((int)currentVisibleRange.Min, (int)maxXRange.Max);
        }
    }
}
