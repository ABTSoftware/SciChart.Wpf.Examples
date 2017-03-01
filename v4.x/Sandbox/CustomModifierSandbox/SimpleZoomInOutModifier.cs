// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SimpleZoomInOutModifier.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Windows;
using System.Windows.Input;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Wpf.TestSuite.ExampleSandbox.CustomModifiers
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