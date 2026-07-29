// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
//
// MagnitudeBubbleSeries.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.SeismicActivityExplorer
{
    /// <summary>
    /// A bubble series whose rendered diameter doubles with each whole magnitude step
    /// (an energy-inspired 2^magnitude contrast), while the Z values keep the raw
    /// magnitude for tooltips, hit-testing and the data series itself.
    /// </summary>
    public class MagnitudeBubbleSeries : FastBubbleRenderableSeries
    {
        /// <summary>
        /// When set, this magnitude maps to <see cref="FastBubbleRenderableSeries.MaxBubbleSizeInPixels"/>
        /// instead of the series' own Z maximum, so multiple series (e.g. background events and
        /// highlighted mainshocks) share one consistent magnitude-to-size scale.
        /// </summary>
        public double SizeReferenceMagnitude { get; set; } = double.NaN;

        public override double GetBubbleDiameter(double zValue)
        {
            double referenceMagnitude = double.IsNaN(SizeReferenceMagnitude) ? MaxZValue : SizeReferenceMagnitude;
            double size = Math.Pow(2.0, zValue);
            double maxSize = Math.Pow(2.0, referenceMagnitude);

            double scale = ApplyAreaSizingInsteadOfRadius
                ? Math.Sqrt(size / maxSize)
                : size / maxSize;

            return MaxBubbleSizeInPixels * scale;
        }
    }
}
