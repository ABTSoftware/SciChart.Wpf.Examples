using Microsoft.Xaml.Behaviors;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SciChart_DigitalAnalyzerPerformanceDemo
{
    public class DigitalAnalyzerScrollBehavior : Behavior<ScrollViewer>
    {
        public static readonly DependencyProperty ChangeChannelHeightCommandProperty = DependencyProperty.Register
            (nameof(ChangeChannelHeightCommand), typeof(ICommand), typeof(DigitalAnalyzerScrollBehavior), new PropertyMetadata(null));

        public ICommand ChangeChannelHeightCommand
        {
            get => (ICommand)GetValue(ChangeChannelHeightCommandProperty);
            set => SetValue(ChangeChannelHeightCommandProperty, value);
        }

        public double ChannelHeightDelta { get; set; }

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.PreviewMouseWheel += ScrollViewer_OnPreviewMouseWheel;
        }
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.PreviewMouseWheel -= ScrollViewer_OnPreviewMouseWheel;
        }

        private void ScrollViewer_OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
            {
                if (ChangeChannelHeightCommand?.CanExecute(null) != true) return;
                ChangeChannelHeightCommand.Execute(e.Delta > 0 ? ChannelHeightDelta : -ChannelHeightDelta);
                e.Handled = true;
            }
            else if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                if (!(sender is ScrollViewer scroll)) return;
                scroll.ScrollToVerticalOffset(scroll.VerticalOffset - e.Delta);
                e.Handled = true;
            }
        }
    }
}