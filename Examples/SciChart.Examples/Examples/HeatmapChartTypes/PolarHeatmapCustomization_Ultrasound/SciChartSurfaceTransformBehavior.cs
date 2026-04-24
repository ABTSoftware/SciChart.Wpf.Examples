// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SciChartSurfaceTransformBehavior.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors;
using SciChart.Charting.Visuals;

namespace SciChart.Examples.Examples.HeatmapChartTypes.PolarHeatmapCustomization_Ultrasound
{
    /// <summary>
    /// A behavior that applies render transforms for scaling and repositioning the SciChartSurface.
    /// </summary>
    /// <remarks>
    /// This behavior is typically used in polar heatmap scenarios where custom positioning
    /// of the chart surface is required. It automatically responds to size changes to maintain
    /// the correct positioning.
    /// </remarks>
    public class SciChartSurfaceTransformBehavior : Behavior<SciChartSurface>
    {
        public double ScaleFactor { get; set; } = 1d;

        /// <summary>
        /// Called after the behavior is attached to the SciChartSurface.
        /// Subscribes to the SizeChanged event.
        /// </summary>
        protected override void OnAttached()
        {
            base.OnAttached();

            AssociatedObject.SizeChanged += SciChartSurface_SizeChanged;
        }

        /// <summary>
        /// Called when the behavior is being detached from the SciChartSurface.
        /// Unsubscribes from the SizeChanged event.
        /// </summary>
        protected override void OnDetaching()
        {
            base.OnDetaching();

            AssociatedObject.SizeChanged -= SciChartSurface_SizeChanged;
        }

        /// <summary>
        /// Handles the SizeChanged event of the SciChartSurface.
        /// Updates the render transform for scaling and positioning the surface.
        /// </summary>
        /// <param name="sender">The SciChartSurface that raised the event.</param>
        /// <param name="e">Event arguments containing the old and new size information.</param>
        private void SciChartSurface_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var polarViewportSize = Math.Min(e.NewSize.Height, e.NewSize.Width);
            var transformGroup = new TransformGroup();

            transformGroup.Children.Add(new TranslateTransform(0, -polarViewportSize / 3.5));
            transformGroup.Children.Add(new ScaleTransform(ScaleFactor, ScaleFactor, 0.5, 0.5));

            AssociatedObject.RenderTransformOrigin = new Point(0.5, 0.5);
            AssociatedObject.RenderTransform = transformGroup;       
        }
    }
}