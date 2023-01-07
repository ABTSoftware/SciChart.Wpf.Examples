using System;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Events;
using SciChart.Core.Utility.Mouse;

namespace EventOnZoomExtentsCompleted
{
    public class ZoomExtentsModifierEx : ZoomExtentsModifier
    {
        public event EventHandler<EventArgs> ZoomExtentsCompleted;

        public override void OnModifierDoubleClick(ModifierMouseArgs e)
        {
            base.OnModifierDoubleClick(e);

            this.XAxis.VisibleRangeChanged -= OnVisibleRangeChanged;
            this.XAxis.VisibleRangeChanged += OnVisibleRangeChanged;
        }

        private void OnVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
        {
            if (e.IsAnimating == false)
            {
                this.XAxis.VisibleRangeChanged -= OnVisibleRangeChanged;
                ZoomExtentsCompleted?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}