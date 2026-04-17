// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MonthNameAngularAxisLabelProvider.cs is part of the SCICHART® Examples. Permission is hereby
// granted to modify, create derivative works, distribute and publish any part of this
// source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarHeatmapCustomization
{
    /// <summary>
    /// Provides custom axis labels for day-of-year values, replacing the default labels with month names.
    /// </summary>
    public class MonthNameAngularAxisLabelProvider : NumericLabelProvider
    {
        /// <summary>
        /// Array of month names used for axis labels.
        /// </summary>
        private readonly string[] Months =
            {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };

        /// <summary>
        /// Converts a numeric tick value of an Angular Axis to a month name label. Values 0-365 are considered days of a year, others are skipped.
        /// </summary>
        /// <returns>A month name if the day is between 1-366 and tick is in the middle of the month, otherwise an empty string.</returns>
        public override string FormatLabel(IComparable dataValue)
        {
            var formattedLabel = string.Empty;

            // Skip all axis ticks but 0-366
            var tickValue = Convert.ToDouble(dataValue);
            if (tickValue < 0 || tickValue > 366) return formattedLabel;

            // Take only ticks 15,45,75,..
            var axisDelta = 15;
            if (tickValue % axisDelta == 0 &&
                tickValue % 2 != 0)
            {
                var dayOfYear = tickValue;
                var daysInMonth = 30;
                var monthIndex = (int)(dayOfYear / daysInMonth);
                formattedLabel = Months[monthIndex];
            }

            return formattedLabel;
        }
    }
}