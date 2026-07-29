// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CityBubbleSeries.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.PopulationMap
{
    /// <summary>
    /// A bubble series sized by area (diameter ∝ √population) with a minimum pixel floor so the smallest cities stay visible.
    /// </summary>
    public class CityBubbleSeries : FastBubbleRenderableSeries
    {
        /// <summary>The smallest rendered bubble diameter, in pixels - the floor for the tiniest cities.</summary>
        public double MinBubbleSizeInPixels { get; set; } = 4.0;

        public override double GetBubbleDiameter(double zValue)
        {
            // MaxZValue is maintained by the base AutoZRange pass (max Z currently in view).
            return ComputeDiameter(zValue, MaxZValue, MinBubbleSizeInPixels, MaxBubbleSizeInPixels);
        }

        /// <summary>
        /// Area-proportional bubble diameter (diameter ∝ √value) with a minimum pixel floor. Shared by the
        /// series and the size legend so both use the same scale.
        /// </summary>
        public static double ComputeDiameter(double value, double maxValue, double minPx, double maxPx)
        {
            if (maxValue <= 0.0) return 0.0;

            var t = Math.Sqrt(Math.Max(0.0, value) / maxValue);
            return minPx + t * (maxPx - minPx);
        }
    }
}
