using System;
using SciChart.Charting.ViewportManagers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace SciChart.Mvvm.Tutorial
{
    public class AutoRangeViewportManager : DefaultViewportManager
    {
        public ISciChartSurface ParentSurface { get; private set; }

        public override void AttachSciChartSurface(ISciChartSurface scs)
        {
            base.AttachSciChartSurface(scs); 
            ParentSurface = scs;
        }

        protected override IRange OnCalculateNewXRange(IAxis xAxis)
        {
            // The current XAxis VisibleRange
            var currentVisibleRange = xAxis.VisibleRange.AsDoubleRange();

            if (ParentSurface.ZoomState == ZoomStates.UserZooming)
                return currentVisibleRange; // Stop auto-ranging if user is zooming

            // The MaxXRange is the VisibleRange on the XAxis if we were to zoom to fit all data
            var maxXRange = xAxis.GetMaximumRange().AsDoubleRange();
            var xMax = Math.Max(maxXRange.Max, currentVisibleRange.Max);

            // Auto-ranging on the XAxis 
            return new DoubleRange(0.0, xMax);
        }
    }
}