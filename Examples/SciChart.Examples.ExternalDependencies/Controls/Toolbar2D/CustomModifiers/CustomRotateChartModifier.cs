// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomRotateChartModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Visuals.Axes;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers
{
    public class CustomRotateChartModifier : ChartModifierBase
    {
        // Defines the IsRotationEnabled attached property
        public static readonly DependencyProperty IsRotationEnabledProperty =
            DependencyProperty.RegisterAttached("IsRotationEnabled", typeof(bool), typeof(CustomRotateChartModifier), new PropertyMetadata(true));

        public static bool GetIsRotationEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsRotationEnabledProperty);
        }

        public static void SetIsRotationEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsRotationEnabledProperty, value);
        }

        public ICommand RotateChartCommand
        {
            get
            {
                return new ActionCommand(() =>
                {
                    if (ParentSurface != null)
                    {

                        using (ParentSurface.SuspendUpdates())
                        {
                            foreach (var xAxis in ParentSurface.XAxes)
                            {
                                RotateClockwise(xAxis);
                            }

                            foreach (var yAxis in ParentSurface.YAxes)
                            {
                                RotateClockwise(yAxis);
                            }
                        }
                    }
                });
            }
        }

        private static void RotateClockwise(IAxis axis)
        {
            switch (axis.AxisAlignment)
            {
                case AxisAlignment.Right:
                    axis.AxisAlignment = AxisAlignment.Bottom;
                    break;
                case AxisAlignment.Bottom:
                    axis.AxisAlignment = AxisAlignment.Left;
                    break;
                case AxisAlignment.Top:
                    axis.AxisAlignment = AxisAlignment.Right;
                    break;
                case AxisAlignment.Left:
                    axis.AxisAlignment = AxisAlignment.Top;
                    break;
            }
        }
    }
}
