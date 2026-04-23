// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ValidateDateFormatStringConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class ValidateDateFormatStringConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ValidateFormatString(value);
        }

        private static object ValidateFormatString(object value)
        {
            try
            {
                var testDate = new DateTime(2013, 03, 2);

                testDate.ToString((string) value);
            }
            catch 
            {
                return null;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ValidateFormatString(value);
        }
    }
}
