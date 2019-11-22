using System.Collections.Generic;
using System.Windows;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Drawing.Common;

namespace CustomPointMarkerExample
{
    /// <summary>
    /// Renders a diamond shape PointMarker, demonstrating custom BasePointMarker API
    /// </summary>
    public class DiamondPointMarker : BasePointMarker
    {
        public override void Draw(IRenderContext2D context, IEnumerable<Point> centers)
        {
            var fill = context.CreateBrush(Fill);
            var stroke = context.CreatePen(Stroke, AntiAliasing, (float)StrokeThickness);

            float width2 = (float)(Width * 0.5);
            float height2 = (float)(Height * 0.5);

            foreach (var center in centers)
            {
                double top = center.Y - height2;
                double bottom = center.Y + height2;
                double left = center.X - width2;
                double right = center.X + width2;

                var diamondPoints = new[]
                {
                    // Points drawn like this:
                    // 
                    //      x0      (x4 in same location as x0)
                    // 
                    // x3        x1
                    //   
                    //      x2
  
                    new Point(center.X, top), // x0
                    new Point(right, center.Y), // x1
                    new Point(center.X, bottom), // x2
                    new Point(left, center.Y), // x3
                    new Point(center.X, top), // x4 == x0
                };

                context.FillPolygon(fill, diamondPoints);
                context.DrawLines(stroke, diamondPoints);
            }
        }
    }
}
