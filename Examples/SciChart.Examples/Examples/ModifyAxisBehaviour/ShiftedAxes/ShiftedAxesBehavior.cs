// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ShiftedAxesBehavior.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Linq;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour.ShiftedAxes
{
    public enum ShiftingMode
    {
        Absolute,
        Relative
    }

    /* NOTE:
     * We use Microsoft.Xaml.Behaviors.Behavior as a base class for this behaviour. We have embedded the source for
     * MS Behaviours in our SciChart.Examples.ExternalDependencies DLL for example purposes only and for compatibility with 
     * WPF and .NET Core
     *
     * What you should do is reference either System.Windows.Interactivity or Microsoft.Xaml.Behaviors.Wpf from NuGet
     * as it is not recommended to reference SciChart.Examples.ExternalDependencies in your applications 
     */
    public class ShiftedAxesBehavior : Behavior<SciChartSurface>
    {
        public ShiftingMode XMode { get; set; }
        public ShiftingMode YMode { get; set; }
        
        public double XAxisPosition { get; set; } 
        public double YAxisPosition { get; set; } 

        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.Rendered += SciChart_OnRendered;
        }


        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.Rendered -= SciChart_OnRendered;
        }

        private void SciChart_OnRendered(object sender, EventArgs eventArgs)
        {
            var xAxis = AssociatedObject.XAxes.FirstOrDefault() as AxisBase;
            var yAxis = AssociatedObject.YAxes.FirstOrDefault() as AxisBase;

            if (xAxis == null || yAxis == null) return;

            if (xAxis.IsCenterAxis)
                xAxis.Width = AssociatedObject.RenderSurface.ActualWidth;            
            if (yAxis.IsCenterAxis)
                yAxis.Height = AssociatedObject.RenderSurface.ActualHeight;

            using (AssociatedObject.SuspendUpdates())
            {
                var yCoordCalc = yAxis.GetCurrentCoordinateCalculator();
                var xCoordCalc = xAxis.GetCurrentCoordinateCalculator();

                if (yCoordCalc != null && xAxis.IsCenterAxis)
                {
                    var xAxisPos = XMode == ShiftingMode.Absolute
                        ? yCoordCalc.GetCoordinate(XAxisPosition)
                        : yAxis.Height*XAxisPosition;

                    Canvas.SetTop(xAxis, xAxisPos);
                    Canvas.SetLeft(xAxis, 0);
                }
                
                if (xCoordCalc != null && yAxis.IsCenterAxis)
                {
                    var yAxisPos = YMode == ShiftingMode.Absolute
                        ? xCoordCalc.GetCoordinate(YAxisPosition)
                        : xAxis.Width*YAxisPosition;

                    Canvas.SetTop(yAxis, 0);
                    Canvas.SetLeft(yAxis, yAxisPos);
                }
            }
        }
    }
}