// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ListBoxScrollToTopBehavior.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.Demo.Behaviors
{
    /* NOTE:
     * We use Microsoft.Xaml.Behaviors.Behavior as a base class for this behaviour. We have embedded the source for
     * MS Behaviours in our SciChart.Examples.ExternalDependencies DLL for example purposes only and for compatibility with 
     * WPF and .NET Core
     *
     * What you should do is reference either System.Windows.Interactivity or Microsoft.Xaml.Behaviors.Wpf from NuGet
     * as it is not recommended to reference SciChart.Examples.ExternalDependencies in your applications 
     */
    public class ListBoxScrollToTopBehavior : Behavior<ListBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Loaded += AssociatedObjectOnLoaded;
            AssociatedObject.Unloaded+= AssociatedObjectOnUnloaded;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Loaded -= AssociatedObjectOnLoaded;
            AssociatedObject.Unloaded -= AssociatedObjectOnUnloaded;
        }

        private void AssociatedObjectOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ((MainWindowViewModel)AssociatedObject.DataContext).SearchResults.CollectionChanged += OnCollectionChanged;
        }

        private void AssociatedObjectOnUnloaded(object sender, RoutedEventArgs routedEventArgs)
        {
            ((MainWindowViewModel)AssociatedObject.DataContext).SearchResults.CollectionChanged -= OnCollectionChanged;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs notifyCollectionChangedEventArgs)
        {
            if (AssociatedObject.Items.Count > 0)
            {
                AssociatedObject.ScrollIntoView(AssociatedObject.Items[0]);
            }
        }
    }
}