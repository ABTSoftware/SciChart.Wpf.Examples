// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// PointCountToLabelConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
{
    public class PointCountToLabelConverter : IValueConverter
    {
        private const double _1B = 1_000_000_000;
        private const double _1M = 1_000_000;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            long pointCount = value is int intvalue ? (long)intvalue : (long)value;

            if (pointCount >= _1B)
            {
                return string.Format("{0:#0.##}B", pointCount / _1B);
            }
            return string.Format("{0:#0.##}M", pointCount / _1M);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}