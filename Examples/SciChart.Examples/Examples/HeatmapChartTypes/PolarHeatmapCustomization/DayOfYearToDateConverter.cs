// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// DayOfYearToDateConverter.cs is part of the SCICHART® Examples. Permission is hereby
// granted to modify, create derivative works, distribute and publish any part of this
// source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarHeatmapCustomization
{
    /// <summary>
    /// Converts a day of year value and year offset to a formatted date string.
    /// </summary>
    /// <remarks>
    /// This converter takes two values: a day of year (1-366) and a year offset (added to 2011 as base year),
    /// and returns a formatted date string in the format "d MMM yyyy".
    /// </remarks>
    public class DayOfYearToDateConverter : IMultiValueConverter
    {
        /// <summary>
        /// Converts day of year and year offset values to a formatted date string.
        /// </summary>
        /// <param name="values">An array containing two double values: [0] day of year (1-366), [1] year offset from 2011.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A formatted date string in "d MMM yyyy" format, or an empty string if conversion fails.</returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var valuesAsDoubles = values?.OfType<double>().ToList();
            if (valuesAsDoubles == null || valuesAsDoubles.Count < 2) return string.Empty;

            // Example data starts at year 2011
            var startYear = 2011;
            // Use a leap year for date conversion
            // because the example data assumes every year has 366 days
            var leapYear = 2012;

            var dayValue = valuesAsDoubles[0];
            var yearValue = valuesAsDoubles[1];
            var dayOfYear = (int)(dayValue + 1);
            var year = (int)yearValue + startYear;
            if (dayOfYear < 1 || dayOfYear > 366) return string.Empty;

            // Use a leap year for date conversion
            // because the example data assumes every year has 366 days
            var date = new DateTime(leapYear, 1, 1).AddDays(dayOfYear - 1);
            return date.ToString($"d MMM {year}", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Converts a formatted date string back to day of year and year offset values.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetTypes">The array of types to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Not implemented.</returns>
        /// <exception cref="NotImplementedException">This method is not implemented.</exception>
        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
