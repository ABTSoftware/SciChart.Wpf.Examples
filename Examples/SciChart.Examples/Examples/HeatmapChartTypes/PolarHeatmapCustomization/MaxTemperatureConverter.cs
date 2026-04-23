// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// MaxTemperatureConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarHeatmapCustomization
{
    /// <summary>
    /// Converts a temperature value to a formatted string with units, or displays custom text for missing data.
    /// </summary>
    public class MaxTemperatureConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the text to display when no data is available (NaN values).
        /// </summary>
        public string NoDataText { get; set; }

        /// <summary>
        /// Converts a temperature value to a formatted string.
        /// </summary>
        /// <param name="value">The temperature value as a double.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>A formatted string "Max {temperature}°C" or the NoDataText if the value is NaN.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double maxTemp)
            {
                return double.IsNaN(maxTemp)
                    ? NoDataText
                    : $"Max {maxTemp:F1}°C";
            }
            return NoDataText;
        }

        /// <summary>
        /// Converts a formatted temperature string back to a numeric value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>Not implemented.</returns>
        /// <exception cref="NotSupportedException">This method is not supported.</exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}