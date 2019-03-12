using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Core.Utility;
using SciChart.Core.Utility.Mouse;
using SciChart.Wpf.UI.Bootstrap.Utility;

namespace SciChart.Sandbox.Examples.CustomModifiers
{
    public class YAxisMousewheelZoomModifier : YAxisDragModifier
    {
        // Override the default MouseDown behaviour 
        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            // Do nothing
        }

        public override void OnModifierMouseWheel(ModifierMouseArgs e)
        {
            foreach (var axis in YAxes)
            {
                // Find the axis under the mouse now 
                var axisBounds = axis.GetBoundsRelativeTo(RootGrid);
                if (axis.IsHorizontalAxis && axisBounds.Height < MinTouchArea)
                {
                    axisBounds.Y -= (MinTouchArea - axisBounds.Height) / 2;
                    axisBounds.Height = MinTouchArea;
                }
                if (!axis.IsHorizontalAxis && axisBounds.Width < MinTouchArea)
                {
                    axisBounds.X -= (MinTouchArea - axisBounds.Width) / 2;
                    axisBounds.Width = MinTouchArea;
                }

                // Look only for the first axis that has been hit
                if (axisBounds.Contains(e.MousePoint))
                {
                    e.Handled = true;

                    const double mouseWheelDeltaCoef = 120;

                    using (ParentSurface.SuspendUpdates())
                    {
                        double value = -e.Delta / mouseWheelDeltaCoef;

                        var mousePoint = GetPointRelativeTo(e.MousePoint, ModifierSurface);

                        // Do the zoom on the axis 
                        const double GrowFactor = 0.1;
                        double fraction = GrowFactor * value;
                        GrowBy(mousePoint, axis, fraction);
                    }
                }
            }            
        }

        /// <summary>
        /// Performs a zoom on a specific axis around the <paramref name="mousePoint" /> by the specified scale factor
        /// </summary>
        /// <param name="mousePoint">The mouse point.</param>
        /// <param name="axis">The axis.</param>
        /// <param name="fraction">The scale factor.</param>
        protected void GrowBy(Point mousePoint, IAxis axis, double fraction)
        {
            double size = GetAxisDimension(axis);
            double coord = axis.IsHorizontalAxis ? mousePoint.X : (size - mousePoint.Y);

            // Compute relative fractions to expand or contract the axis Visiblerange by
            double lowFraction = (coord / size) * fraction;
            double highFraction = (1.0 - (coord / size)) * fraction;

            var isVerticalChart = (axis.IsHorizontalAxis && !axis.IsXAxis) || (!axis.IsHorizontalAxis && axis.IsXAxis);
            var flipCoords = (isVerticalChart && !axis.FlipCoordinates) || (!isVerticalChart && axis.FlipCoordinates);

            if (flipCoords)
            {
                double temp = lowFraction;
                lowFraction = highFraction;
                highFraction = temp;
            }

            axis.ZoomBy(lowFraction, highFraction);
        }

        private double GetAxisDimension(IAxis axis)
        {
            double size = axis.IsHorizontalAxis ? axis.Width : axis.Height;

            var parentSurface = axis.ParentSurface as SciChartSurface;
            // if axis.Visibility==Collapsed, try to get appropriate dimension from the RenderSurface
            if (axis.Visibility == Visibility.Collapsed && parentSurface != null)
            {
                // TODO: Temporary fix for http://abtsoftware.myjetbrains.com/youtrack/issue/SC-3323
                // In case of stacked axes this calculation is wrong for a collapsed axis. The reason being that we cannot get its dimensions and offset.
                // Also, if axis size is set manually, calculation will be wrong.
                size = axis.IsHorizontalAxis ? parentSurface.RenderSurface.ActualWidth : parentSurface.RenderSurface.ActualHeight;
            }

            return size;
        }
    }
}
