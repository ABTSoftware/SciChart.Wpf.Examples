// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AggregationPriceToVisibilityConverter.cs is part of the SCICHART® Examples. Permission is
// hereby granted to modify, create derivative works, distribute and publish any part of this
// source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AggregationFilters
{
    public class AggregationPriceToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is AggregationPriceChart && parameter is string expectedChart)
            {
                var priceChart = Enum.GetName(typeof(AggregationPriceChart), value);

                return priceChart.Equals(expectedChart) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
