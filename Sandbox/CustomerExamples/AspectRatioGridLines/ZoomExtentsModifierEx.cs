using System;
using SciChart.Charting.ChartModifiers;
using SciChart.Drawing.Common;

namespace AspectRatioGridLines
{
    public class ZoomExtentsModifierEx : ZoomExtentsModifier
    {
        protected override void PerformZoom()
        {
            if (ParentSurface == null) return;

            ParentSurface.ChartModifier?.ResetInertia();

            var xDataRange = ParentSurface.XAxis.GetMaximumRange();
            var yDataRange = ParentSurface.YAxis.GetMaximumRange();

            var duration = IsAnimated
                ? TimeSpan.FromMilliseconds(500)
                : TimeSpan.Zero;

            if (xDataRange.Diff.CompareTo(yDataRange.Diff) > 0 &&
                ParentSurface.RenderSurface is IRenderSurface surface)
            {
                yDataRange = VisibleRangeHelper.GetYRangeByXRange
                    (surface.ActualHeight, surface.ActualWidth, xDataRange);             
            }

            if (yDataRange?.IsDefined == true)
            {
                ParentSurface.YAxis.AnimateVisibleRangeTo(yDataRange, duration);
            }
        }
    }
}