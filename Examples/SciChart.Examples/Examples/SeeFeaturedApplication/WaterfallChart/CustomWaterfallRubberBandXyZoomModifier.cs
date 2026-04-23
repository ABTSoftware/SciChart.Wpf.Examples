// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomWaterfallRubberBandXyZoomModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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
