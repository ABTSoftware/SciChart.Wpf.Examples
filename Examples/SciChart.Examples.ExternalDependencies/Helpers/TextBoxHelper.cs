// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// TextBoxHelper.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;

namespace SciChart.Examples.ExternalDependencies.Helpers
{
    public class TextBoxHelper
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached
            ("Watermark", typeof(string), typeof(TextBoxHelper), new PropertyMetadata(null));       

        public static void SetWatermark(DependencyObject element, string value)
        {
            element.SetValue(WatermarkProperty, value);
        }

        public static string GetWatermark(DependencyObject element)
        {
            return (string) element.GetValue(WatermarkProperty);
        }
        
        public static readonly DependencyProperty WatermarkStyleProperty = DependencyProperty.RegisterAttached
            ("WatermarkStyle", typeof(Style), typeof(TextBoxHelper), new PropertyMetadata(null));       

        public static void SetWatermarkStyle(DependencyObject element, Style value)
        {
            element.SetValue(WatermarkStyleProperty, value);
        }

        public static Style GetWatermarkStyle(DependencyObject element)
        {
            return (Style) element.GetValue(WatermarkStyleProperty);
        }
    }
}
