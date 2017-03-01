// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SimpleZoomPanModifier.cs is part of SCICHART®, High Performance Scientific Charts
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
using System.Windows;
using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Wpf.TestSuite.ExampleSandbox.CustomModifiers
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