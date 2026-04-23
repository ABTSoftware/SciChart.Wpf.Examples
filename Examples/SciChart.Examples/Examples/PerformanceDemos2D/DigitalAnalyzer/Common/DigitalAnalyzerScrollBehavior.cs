// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// DigitalAnalyzerScrollBehavior.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
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

        public double ChannelHeightDelta { get; set;}

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