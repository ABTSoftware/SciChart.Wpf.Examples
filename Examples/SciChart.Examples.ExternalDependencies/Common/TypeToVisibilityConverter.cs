// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// TypeToVisibilityConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class TypeToVisibilityConverter : IValueConverter
    {
        public Type ExpectedType { get; set; }

        public bool InverseVisibility { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && ExpectedType != null)
            {
                return ((value as Type) ?? value.GetType()) == ExpectedType
                    ? (InverseVisibility ? Visibility.Collapsed : Visibility.Visible)
                    : (InverseVisibility ? Visibility.Visible : Visibility.Collapsed);
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
