// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// TextBoxHelper.cs is part of SCICHART®, High Performance Scientific Charts
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
