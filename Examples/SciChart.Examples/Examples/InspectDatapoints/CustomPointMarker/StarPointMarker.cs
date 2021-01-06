// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// StarPointMarker.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Drawing.Common;

namespace SciChart.Examples.Examples.InspectDatapoints.CustomPointMarker
{
    public class StarPointMarker : BitmapSpriteBase 
    {
        protected override void RenderToCache(IRenderContext2D context, IPen2D strokePen, IBrush2D fillBrush)
        {
            var offset = 2d;
            var polygon = new Point[] {new Point(Width/2, 0), new Point(Width/2 + offset, Height/2 - offset), new Point(Width, Height/2), new Point(Width/2 + offset, Height/2 + offset), new Point(Width/2, Height), new Point(Width/2-offset, Height/2+offset), new Point(0, Height/2), new Point(Width/2-offset, Height/2-offset), new Point(Width/2, 0)};

            context.FillPolygon(fillBrush, polygon);
            context.DrawLines(strokePen, polygon);
        }
    }
}
