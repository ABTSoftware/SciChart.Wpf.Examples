// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// FrameworkVisibilityManager.cs is part of SCICHART®, High Performance Scientific Charts
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
