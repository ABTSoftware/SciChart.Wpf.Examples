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