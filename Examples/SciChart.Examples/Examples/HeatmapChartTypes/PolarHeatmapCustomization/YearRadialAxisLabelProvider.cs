// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// YearRadialAxisLabelProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Globalization;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarHeatmapCustomization
{
    /// <summary>
    /// Provides custom axis labels for years, converting year offsets to actual year values starting from 2011.
    /// </summary>
    public class YearRadialAxisLabelProvider : NumericLabelProvider
    {
        /// <summary>
        /// Formats a numeric data value as a year label.
        /// </summary>
        /// <param name="dataValue">The numeric value representing a year offset from 2011.</param>
        /// <returns>A string representing the actual year (offset + 2011), or an empty string for negative values.</returns>
        public override string FormatLabel(IComparable dataValue)
        {
            var year = Convert.ToDouble(dataValue);

            if (year < 0)
                return string.Empty;

            // Example data starts at year 2011
            var startYear = 2011;

            return (year + startYear).ToString(CultureInfo.InvariantCulture);
        }
    }
}