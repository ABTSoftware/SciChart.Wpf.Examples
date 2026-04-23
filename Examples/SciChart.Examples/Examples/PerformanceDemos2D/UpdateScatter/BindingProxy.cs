// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// BindingProxy.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;

namespace SciChart.Examples.Examples.PerformanceDemos2D.UpdateScatter
{
    internal class BindingProxy : FrameworkElement
    {
        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingSourceProperty =
            DependencyProperty.Register(nameof(BindingSource), typeof(object), typeof(BindingProxy), new PropertyMetadata(null, OnBindingSourceChanged));

        // Using a DependencyProperty as the backing store for RenderSurfaceTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingTargetProperty =
            DependencyProperty.Register(nameof(BindingTarget), typeof(object), typeof(BindingProxy), new PropertyMetadata(null));

        public object BindingTarget
        {
            get { return (object)GetValue(BindingTargetProperty); }
            set { SetValue(BindingTargetProperty, value); }
        }

        public object BindingSource
        {
            get { return (object)GetValue(BindingSourceProperty); }
            set { SetValue(BindingSourceProperty, value); }
        }

        private static void OnBindingSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is BindingProxy proxy)
            {
                proxy.BindingTarget = e.NewValue;
            }
        }
    }
}
