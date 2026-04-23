// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// FocusedChannelScrollBehavior.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
{
    public class FocusedChannelScrollBehavior : Behavior<ItemsControl>
    {
        public bool ScrollToFocusedChannel { get; set; } = true;

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.RequestBringIntoView += ItemsControl_OnRequestBringIntoView;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.RequestBringIntoView -= ItemsControl_OnRequestBringIntoView;
        }

        private void ItemsControl_OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = !ScrollToFocusedChannel;
        }
    }
}