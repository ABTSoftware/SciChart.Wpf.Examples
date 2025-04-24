using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;
using System;

namespace SciChart.Examples.Examples.CreateStockCharts.MarketProfileTrading
{
    class HistogramBarViewportManager : DefaultViewportManager
    {
        private IAxis _priceYAxis;

        public override void AttachSciChartSurface(ISciChartSurface scs)
        {
            base.AttachSciChartSurface(scs);
            ParentSurface = scs;

            // Set Autorange behavior for the Price Y Axis
            _priceYAxis = scs.YAxes.GetAxisById("PriceYAxis");
            _priceYAxis.AutoRange = AutoRange.Always;
        }

        public override void DetachSciChartSurface()
        {
            // Reset Autorange behavior for the Price Y Axis
            _priceYAxis.AutoRange = AutoRange.Once;
            _priceYAxis = null;
            ParentSurface = null;

            base.DetachSciChartSurface();
        }

        public ISciChartSurface ParentSurface { get; private set; }

        protected override IRange OnCalculateNewXRange(IAxis xAxis)
        {
            var currentVisibleRange = xAxis.VisibleRange;
            // Getting X-Range of sorted data is quick,
            // so it can be done every frame
            var dataXRange = xAxis.DataRange;

            return new DateRange((DateTime)currentVisibleRange.Min, (DateTime)dataXRange.Max);
        }
    }
}