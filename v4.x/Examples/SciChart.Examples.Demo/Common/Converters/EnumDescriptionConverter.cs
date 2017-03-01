// **************************************************************************************
// SCICHART © Copyright SciChart Ltd. 2011-2012. All rights reserved.
// Examples Suite source code provided as-is to assist in creation of applications using SciChart. 
// At no time may this source be be copied, transferred, sold, distributed or made available
// **************************************************************************************

using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class EnumDescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Enum e = value as Enum;
            if (e == null) return null;

            var type = e.GetType();
            var memInfo = type.GetMember(e.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            var description = ((DescriptionAttribute)attributes[0]).Description;
            return description;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}