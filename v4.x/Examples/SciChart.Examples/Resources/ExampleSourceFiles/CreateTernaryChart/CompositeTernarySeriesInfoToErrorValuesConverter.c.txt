using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SciChart.Charting.Model.ChartData;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.CreateTernaryChart
{
    /// <summary>
    /// Converts from a SeriesInfoCollection to Error value.
    /// </summary>
    public class CompositeTernarySeriesInfoToErrorValuesConverter : IValueConverter
    {
        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string formattedString = string.Empty;

            var seriesInfoCollection = (IEnumerable<SeriesInfo>) value;
            var errorSeries = seriesInfoCollection.LastOrDefault(x => x.RenderableSeries is TernaryErrorBarRenderableSeries);

            if (errorSeries != null)
            {
                var series = (TernaryErrorBarRenderableSeries)errorSeries.RenderableSeries;

                var isLowError = (string) parameter == "LowError";
                var error = isLowError ? series.LowError : series.HighError;
                var formatString = isLowError ? "Low Error: {0}" : "High Error: {0}";

                formattedString = GetFormattedErrorValue(error, series.ErrorType, formatString);
            }

            return formattedString;
        }

        private string GetFormattedErrorValue(double errorValue, ErrorType errorType, string formatString)
        {
            if (errorType == ErrorType.Relative)
            {
                errorValue *= 100;
                formatString += "%";
            }

            return string.Format(formatString, errorValue);
        }

        /// <summary>
        /// Converts a value.
        /// </summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
