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