// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// MinPopulationLabelConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.SeeFeaturedApplication.PopulationMap
{
    /// <summary>
    /// Formats the population-filter slider value: 0 reads as "All cities", otherwise a "≥ 250 K" / "≥ 1.5 M" threshold.
    /// </summary>
    public class MinPopulationLabelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var population = System.Convert.ToDouble(value, culture);

            if (population < 1000)
                return "All cities";

            return population >= 1_000_000
                ? $"≥ {population / 1_000_000.0:0.#} M"
                : $"≥ {population / 1_000.0:0} K";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
