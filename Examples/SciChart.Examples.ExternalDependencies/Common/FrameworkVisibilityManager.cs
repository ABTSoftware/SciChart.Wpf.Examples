// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// FrameworkVisibilityManager.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;

namespace SciChart.Examples.ExternalDependencies.Common
{
    public enum FrameworkVisibility
    {
        All,
        Wpf, 
        Silverlight
    }

    /// <summary>
    /// Used to show or hide UIElements based on framework (WPF, Silverlight)
    /// </summary>
    public class FrameworkVisibilityManager : FrameworkElement
    {
        public static readonly DependencyProperty VisibleInProperty =
            DependencyProperty.RegisterAttached("VisibleIn", typeof(FrameworkVisibility), typeof(FrameworkVisibilityManager), new PropertyMetadata(FrameworkVisibility.All, OnVisibleInPropertyChanged));        

        public static void SetVisibleIn(DependencyObject element, FrameworkVisibility visibleIn)
        {
            element.SetValue(VisibleInProperty, visibleIn);
        }

        public static FrameworkVisibility GetVisibleIn(DependencyObject element)
        {
            return (FrameworkVisibility)element.GetValue(VisibleInProperty);
        }

        private static void OnVisibleInPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
#if SILVERLIGHT
            var visibility = ((FrameworkVisibility) e.NewValue) == FrameworkVisibility.Wpf ? Visibility.Collapsed : Visibility.Visible;
#else
            var visibility = ((FrameworkVisibility) e.NewValue) == FrameworkVisibility.Silverlight ? Visibility.Collapsed : Visibility.Visible;
#endif

            (d as UIElement).Visibility = visibility;
        }
    }
}
