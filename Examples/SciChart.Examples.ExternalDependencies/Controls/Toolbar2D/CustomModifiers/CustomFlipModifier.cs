// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomFlipModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Windows.Input;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model;
using SciChart.Core.Extensions;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers
{
    public class CustomFlipModifier : ChartModifierBase
    {
        public ICommand FlipXAxis { get; }

        public ICommand FlipYAxis { get; }

        public CustomFlipModifier()
        {
            FlipXAxis = new ActionCommand(() =>
            {
                if (ParentSurface != null)
                {
                    FlipAxes(ParentSurface.XAxes);
                }
            });

            FlipYAxis = new ActionCommand(() =>
            {
                if (ParentSurface != null)
                {
                    FlipAxes(ParentSurface.YAxes);
                }
            });
        }

        public void FlipAxes(AxisCollection axes)
        {
            using (ParentSurface.SuspendUpdates())
            {
                axes.ForEachDo(axis => axis.FlipCoordinates = !axis.FlipCoordinates);
            }
        }
    }
}