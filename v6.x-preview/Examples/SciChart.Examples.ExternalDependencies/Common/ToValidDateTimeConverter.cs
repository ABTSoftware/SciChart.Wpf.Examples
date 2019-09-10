// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ToValidDateTimeConverter.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
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
            if (XVisibleRange == null || value == null) return value;

            return ValidateDate(value, parameter);
        }

        private object ValidateDate(object value, object parameter)
        {
            var newDate = (DateTime) value;
            switch (parameter.ToString())
            {
                case "Max":
                    if (XVisibleRange.Min >= newDate)
                    {
                        newDate = XVisibleRange.Max;
                    }
                    break;
                case "Min":
                    if (XVisibleRange.Max <= newDate)
                    {
                        newDate = XVisibleRange.Min;
                    }
                    break;
            }
            return newDate;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (XVisibleRange == null || value == null) return value;

            return ValidateDate(value, parameter);
        }
    }
}