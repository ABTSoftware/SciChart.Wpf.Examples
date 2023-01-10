// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomRotateChartModifier.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
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
