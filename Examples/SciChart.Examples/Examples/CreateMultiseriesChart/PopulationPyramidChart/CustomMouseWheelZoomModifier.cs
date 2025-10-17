// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
//
// CustomMouseWheelZoomModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.PopulationPyramidChart
{
    public class CustomMouseWheelZoomModifier : MouseWheelZoomModifier
    {
        protected override void OverrideKeyboardAction(MouseModifier modifier)
        {
            // Overrides the base implementation to allow panning in X-only direction
            if (ExecuteWhen == MouseModifier.None && modifier == MouseModifier.Ctrl)
            {
                SetCurrentValue(ActionTypeProperty, ActionType.Pan);
            }
        }
    }
}