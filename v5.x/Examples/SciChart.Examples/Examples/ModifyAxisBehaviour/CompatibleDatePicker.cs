// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CompatibleDatePicker.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    public class CompatibleDatePicker: DatePicker
    {
        public CompatibleDatePicker()
        {
            this.Loaded+=CompatibleDatePicker_Loaded;
        }

        private void CompatibleDatePicker_Loaded(object sender, RoutedEventArgs e)
        {
            //Getting text block with watermark
            var textBlock = GetChildOfType<TextBlock>(this);

            if (textBlock != null)
            {
                textBlock.Text = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
            }
        }

        private T GetChildOfType<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj == null) return null;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                var child = VisualTreeHelper.GetChild(depObj, i);

                var result = (child as T) ?? GetChildOfType<T>(child);
                if (result != null) return result;
            }

            return null;
        }
    }
}
