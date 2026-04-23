// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// HyperlinkButtonBehavior.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.ExternalDependencies.Behaviors
{
    class HyperlinkButtonBehavior : Behavior<Button>
    {
        public static readonly DependencyProperty UriProperty = DependencyProperty.Register("Uri", typeof(string), typeof(HyperlinkButtonBehavior));

        public string Uri
        {
            get { return (string) GetValue(UriProperty); }
            set { SetValue(UriProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Click += OnHyperlinkClick;
            AssociatedObject.Cursor = Cursors.Hand;
        }

        private void OnHyperlinkClick(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrWhiteSpace(Uri))
            {
                var procStartInfo = new ProcessStartInfo(Uri) { UseShellExecute = true };
                Process.Start(procStartInfo);
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Click -= OnHyperlinkClick;
            AssociatedObject.Cursor = Cursors.Arrow;
        }
    }
}
