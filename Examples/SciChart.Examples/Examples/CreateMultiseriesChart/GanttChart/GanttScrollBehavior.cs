using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.GanttChart
{
    public class GanttScrollBehavior : Behavior<ScrollViewer>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
        }
  
        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                AssociatedObject.ScrollToVerticalOffset(AssociatedObject.VerticalOffset - e.Delta);

                e.Handled = true;
            }
        }
    }
}