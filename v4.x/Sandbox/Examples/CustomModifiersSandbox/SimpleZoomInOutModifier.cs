using System;
using System.Windows;
using System.Windows.Input;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Sandbox.ExampleSandbox.CustomModifiers
{
    /// <summary>
    /// A single X-Y axis implementation of a Zooming In / Out on KeyDown (CTRL+, CTRL-), used to demonstrate the ChartModifierBase and Axis Interactivity APIs in SciChart
    /// </summary>
    public class SimpleZoomInOutModifier : ChartModifierBase
    {
        public static readonly DependencyProperty ZoomFractionProperty = DependencyProperty.Register("ZoomFraction", typeof(double), typeof(SimpleZoomInOutModifier), new PropertyMetadata(0.1));

        public double ZoomFraction
        {
            get { return (double)GetValue(ZoomFractionProperty); }
            set { SetValue(ZoomFractionProperty, value); }
        }

        public override void OnModifierKeyDown(ModifierKeyArgs e)
        {
            base.OnModifierKeyDown(e);

            double factor = 0;

            if ((e.Key == Key.Add || e.Key == Key.OemPlus) && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                // On CTRL+, Zoom In
                factor = -ZoomFraction;
            }
            if ((e.Key == Key.Subtract || e.Key == Key.OemMinus) && Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                // On CTRL-, Zoom Out
                factor = ZoomFraction;
            }

            using (ParentSurface.SuspendUpdates())
            {
                // Zoom the XAxis by the required factor
                XAxis.ZoomBy(factor, factor, TimeSpan.FromMilliseconds(500));

                // Zoom the YAxis by the required factor
                YAxis.ZoomBy(factor, factor, TimeSpan.FromMilliseconds(500));

                // Note.. can be extended for multiple YAxis XAxis, just iterate over all axes on the parent surface
            }
        }
    }
}