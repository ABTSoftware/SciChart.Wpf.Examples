// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// UltrasoundPolarLabelProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarHeatmapCustomization_Ultrasound
{
    /// <summary>
    /// A custom label provider that selectively hides labels outside a specified Visible Range.
    /// </summary>
    /// <remarks>
    /// This label provider is useful for polar heatmap charts where labels should only be displayed
    /// within a specific angular range (e.g., for ultrasound sector displays). Labels outside the
    /// visible range are hidden by returning an empty string.
    /// </remarks>
    public class UltrasoundPolarLabelProvider : NumericLabelProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UltrasoundPolarLabelProvider"/> class.
        /// </summary>
        /// <param name="visibleMin">The minimum visible range in degrees. Labels below this value will be hidden.</param>
        /// <param name="visibleMax">The maximum visible angle in degrees. Labels above this value will be hidden.</param>
        public UltrasoundPolarLabelProvider(double visibleMin, double visibleMax)
        {
            VisibleMin = visibleMin;
            VisibleMax = visibleMax;
        }

        /// <summary>
        /// Gets the minimum visible angle in degrees.
        /// Labels with angles below this value will not be displayed.
        /// </summary>
        public double VisibleMin { get; }

        /// <summary>
        /// Gets the maximum visible angle in degrees.
        /// Labels with angles above this value will not be displayed.
        /// </summary>
        public double VisibleMax { get; }

        /// <summary>
        /// Formats the label for the specified data value, hiding labels outside the visible angular range.
        /// </summary>
        /// <param name="dataValue">The data value to format, representing an angle in degrees.</param>
        /// <returns>
        /// An empty string if the angle is outside the visible range (less than <see cref="VisibleMin"/> 
        /// or greater than <see cref="VisibleMax"/>); otherwise, the formatted label from the base implementation.
        /// </returns>
        /// <remarks>
        /// This method converts the data value to a double representing an angle, then checks if it falls
        /// within the visible range. Only labels within the range are formatted and displayed.
        /// </remarks>
        public override string FormatLabel(IComparable dataValue)
        {
            var angle = Convert.ToDouble(dataValue);

            if (angle < VisibleMin || angle > VisibleMax)
                return string.Empty;

            return base.FormatLabel(dataValue);
        }
    }
}
