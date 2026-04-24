// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ToValidDateTimeConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Data.Model;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public class ToValidDateTimeConverter : IValueConverter
    {
        public DateRange XVisibleRange { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (XVisibleRange != null && value is DateTime dateTime)
            {
                return ValidateDate(dateTime, parameter).ToString();
            }
            return DateTime.MinValue.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (XVisibleRange != null && value is string dateString)
            {
                return ValidateDate(DateTime.Parse(dateString), parameter);
            }
            return DateTime.MinValue;
        }

        private object ValidateDate(DateTime dateTime, object parameter)
        {
            if (parameter is string rangeProperty)
            {
                if (rangeProperty == "Max" && XVisibleRange.Min >= dateTime)
                {
                    dateTime = XVisibleRange.Max;
                }

                if (rangeProperty == "Min" && XVisibleRange.Max <= dateTime)
                {
                    dateTime = XVisibleRange.Min;
                }
            }
            return dateTime;
        }
    }
}