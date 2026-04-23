// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// IsEmptyStringConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class IsEmptyStringConverter : IValueConverter
    {
        private const string InvertionFlag = "INVERSE";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var stringParam = parameter as string;
            var inverse = string.Equals(stringParam, InvertionFlag, StringComparison.InvariantCultureIgnoreCase);

            var onTrue= inverse ? Visibility.Visible : Visibility.Collapsed;
            var onFalse = inverse ? Visibility.Collapsed : Visibility.Visible;

            return string.IsNullOrEmpty(value as string) ? onTrue : onFalse;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}