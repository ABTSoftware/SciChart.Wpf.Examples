using System.Windows;
using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Sandbox.ExampleSandbox.CustomModifiers
{
    /// <summary>
    /// A single X-Y axis implementation of a ZoomPanModifier, used to demonstrate the ChartModifierBase and Axis Interactivity APIs in SciChart
    /// </summary>
    public class SimpleZoomPanModifier : ChartModifierBase
    {
        private Point? _lastPoint;

        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);

            e.Handled = true;
            _lastPoint = e.MousePoint;
        }

        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            base.OnModifierMouseMove(e);
            if (_lastPoint == null) return;

            var currentPoint = e.MousePoint;
            var xDelta = currentPoint.X - _lastPoint.Value.X;
            var yDelta = _lastPoint.Value.Y - currentPoint.Y;

            using (ParentSurface.SuspendUpdates())
            {
                // Scroll the XAxis by the number of pixels since the last update
                XAxis.Scroll(XAxis.IsHorizontalAxis ? xDelta : -yDelta, ClipMode.None);

                // Scroll the YAxis by the number of pixels since the last update
                YAxis.Scroll(YAxis.IsHorizontalAxis ? -xDelta : yDelta, ClipMode.None);

                // Note.. can be extended for multiple YAxis XAxis, just iterate over all axes on the parent surface
            }

            _lastPoint = currentPoint;
        }

        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            base.OnModifierMouseUp(e);
            _lastPoint = null;
        }
    }
}