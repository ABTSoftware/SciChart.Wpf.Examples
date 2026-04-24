// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ScrollViewerExtensions.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo.Behaviors
{
    public class ScrollViewerExtensions
    {
        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached
            ("CornerRadius", typeof(CornerRadius), typeof(ScrollViewerExtensions), new PropertyMetadata(new CornerRadius(0)));  

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius) element.GetValue(CornerRadiusProperty);
        }

        public static readonly DependencyProperty CanContentScrollProperty = DependencyProperty.RegisterAttached
            ("CanContentScroll", typeof(bool), typeof(ScrollViewerExtensions), new PropertyMetadata(true, OnCanContentScrollChanged));        

        public static void SetCanContentScroll(DependencyObject element, bool value)
        {
            element.SetValue(CanContentScrollProperty, value);
        }

        public static bool GetCanContentScroll(DependencyObject element)
        {
            return (bool) element.GetValue(CanContentScrollProperty);
        }

        private static void OnCanContentScrollChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ScrollViewer.SetCanContentScroll(d, (bool)e.NewValue);
        }
    }
}