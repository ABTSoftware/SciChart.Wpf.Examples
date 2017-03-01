using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SciChart.Charting.Numerics.CoordinateProviders;
using SciChart.Charting.Themes;
using SciChart.Charting.Visuals.Axes;

namespace SciChart.Examples.Examples.CreateRadarChart
{
    public class CustomRadarAxis : RadarAxis
    {
        protected override void DrawTickLabels(AxisCanvas canvas, TickCoordinates tickCoords, float offset)
        {
            if (!tickCoords.IsEmpty)
            {
                tickCoords = new TickCoordinates(tickCoords.MinorTicks.Skip(1).ToArray(),
                    tickCoords.MajorTicks.Skip(1).ToArray(),
                    tickCoords.MinorTickCoordinates.Skip(1).ToArray(),
                    tickCoords.MajorTickCoordinates.Skip(1).ToArray());

            }

            base.DrawTickLabels(canvas, tickCoords, offset);
        }

        protected override void OnDrawAxis(TickCoordinates tickCoords)
        {
            if (!tickCoords.IsEmpty)
            {
                tickCoords = new TickCoordinates(tickCoords.MinorTicks.Skip(1).ToArray(),
                    tickCoords.MajorTicks.Skip(1).ToArray(),
                    tickCoords.MinorTickCoordinates.Skip(1).ToArray(),
                    tickCoords.MajorTickCoordinates.Skip(1).ToArray());

            }

            base.OnDrawAxis(tickCoords);

        }
    }
}
