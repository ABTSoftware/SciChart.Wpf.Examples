using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Core.Framework;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Sandbox.Examples.CustomModifiersSandbox
{
    // using technique from https://www.scichart.com/questions/wpf/zoomextends-on-either-yaxis-or-chart to create a custom chartmodifier
    // to report clicks on chart parts 
    public class DetectClicksOnChartPartsModifier : ChartModifierBase
    {
        public DetectClicksOnChartPartsModifier()
        {
            // Process events even if handled 
            ReceiveHandledEvents = true;
        }

        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);

            IEnumerable<IHitTestable> chartElements = this.YAxes.Cast<IHitTestable>()
                            .Concat(this.XAxes)
                            .Concat(new[] {this.ModifierSurface});

            var clickedElement = chartElements.FirstOrDefault(ce => IsPointWithinBounds(e.MousePoint, ce));

            if (clickedElement != null)
            {
                Debug.WriteLine("The following element was clicked: " + WhatElement(clickedElement));
            }
        }

        private string WhatElement(IHitTestable clickedElement)
        {
            if (clickedElement is AxisBase axis)
            {
                return (axis.IsXAxis ? "X" : "Y") + $"Axis, Type={axis.GetType().Name}, Id={axis.Id}, Alignment={axis.AxisAlignment}";
            }
            else if (clickedElement is IChartModifierSurface)
            {
                return "The inner chart surface";
            }

            return null;
        }
    }
}
