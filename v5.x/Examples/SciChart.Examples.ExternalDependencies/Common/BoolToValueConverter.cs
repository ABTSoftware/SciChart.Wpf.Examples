// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BoolToValueConverter.cs is part of SCICHART®, High Performance Scientific Charts
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

namespace SciChart.Examples.ExternalDependencies.Common
{
    /// <summary>
    /// A ValueConverter which can convert Boolean to any value, e.g. Boolean to Visibility, Boolean to Margin
    /// </summary>
    public class BoolToValueConverter : IValueConverter
    {
        /// <summary>
        /// The value to convert to if the input boolean is true
        /// </summary>
        public object TrueValue { get; set; }
        /// <summary>
        /// The value to convert to if the input boolean is false
        /// </summary>
        public object FalseValue { get; set; }
        /// <summary>
        /// The value to convert to if the input boolean is true and in Silverlight
        /// </summary>
        public object TrueSLValue { get; set; }

        /// <summary>
        /// Modifies the source data before passing it to the target for display in the UI.
        /// </summary>
        /// <param name="value">The source data being passed to the target.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the target dependency property.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the target dependency property.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool v = false;
            if (value is bool)
            {
                v = (bool) value;
            }
            if (value is int)
            {
                v = ((int) value) >= 1;
            }

            return v ? 
#if !SILVERLIGHT
                TrueValue 
#else
                TrueSLValue ?? TrueValue
#endif
                : 
                FalseValue;
        }

        /// <summary>
        /// Modifies the target data before passing it to the source object.  This method is called only in <see cref="F:System.Windows.Data.BindingMode.TwoWay" /> bindings.
        /// </summary>
        /// <param name="value">The target data being passed to the source.</param>
        /// <param name="targetType">The <see cref="T:System.Type" /> of data expected by the source object.</param>
        /// <param name="parameter">An optional parameter to be used in the converter logic.</param>
        /// <param name="culture">The culture of the conversion.</param>
        /// <returns>
        /// The value to be passed to the source object.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }    
}
