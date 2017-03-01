using System;
using System.Windows;
using SciChart.Charting.ChartModifiers;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.WaterfallChart
{
    public class CustomWaterfallRubberBandXyZoomModifier : RubberBandXyZoomModifier
    {
        public Point startPoint { get; set; }
        public Point endPoint { get; set; }

        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);
            startPoint = GetPointRelativeTo(e.MousePoint, ModifierSurface);
        }

        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            ModifierSurface.ReleaseMouseCapture();
            endPoint = GetPointRelativeTo(e.MousePoint, ModifierSurface);
            OnAttached();

            if (Math.Abs(startPoint.X - endPoint.X) > 10)
            {
                int i = 0;
                foreach (CustomWaterfallNumericAxis YAxis in ParentSurface.XAxes)
                {

                    YAxis.Zoom(startPoint.X + i * 2, endPoint.X + i * 2, TimeSpan.FromMilliseconds(1000));
                    i++;
                }
            }
        }
    }

}
