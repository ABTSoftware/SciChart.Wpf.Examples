// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// GanttScrollBehavior.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Xaml.Behaviors;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.GanttChart
{
    /// <summary>
    /// Attached behavior that enables vertical scrolling of the Gantt row list via Ctrl+MouseWheel.
    /// Regular (unmodified) mouse wheel is left unconsumed so that the underlying
    /// <see cref="SciChart.Charting.ChartModifiers.ZoomPanModifier"/> can use it to pan the X-axis.
    /// </summary>
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
            // Ctrl+Wheel scrolls the task list vertically.
            // Mark the event as handled to prevent the chart from panning at the same time.
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                AssociatedObject.ScrollToVerticalOffset(AssociatedObject.VerticalOffset - e.Delta);

                e.Handled = true;
            }
        }
    }
}