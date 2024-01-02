// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HyperlinkButtonBehavior.cs is part of SCICHART®, High Performance Scientific Charts
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
                Process.Start(Uri);
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
