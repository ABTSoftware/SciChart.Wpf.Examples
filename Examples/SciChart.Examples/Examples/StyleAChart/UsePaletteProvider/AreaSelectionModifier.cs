// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AreaSelectionModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Windows;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.Axes;
using SciChart.Core.Utility.Mouse;

namespace SciChart.Examples.Examples.StyleAChart.UsePaletteProvider
{
    public class AreaSelectionModifier : RubberBandXyZoomModifier
    {
        /// <summary>
        /// Defines the Ellipse dependency property
        /// </summary>
        public static readonly DependencyProperty SelectionProperty = DependencyProperty.Register("AreaSelection", typeof(AreaSelection), typeof(AreaSelectionModifier), new PropertyMetadata(null));

        /// <summary>Defines the YAxisId DependencyProperty</summary>
        public static readonly DependencyProperty YAxisIdProperty = DependencyProperty.Register("YAxisId", typeof(string), typeof(AreaSelectionModifier), new PropertyMetadata(AxisCore.DefaultAxisId));

        private bool _isDragging;
        private Point _startPoint;
        private Rect _rectangle;

        /// <summary>
        /// Initializes a new instance of the <see cref="AreaSelectionModifier"/> class.
        /// </summary>
        public AreaSelectionModifier()
        {
            IsXAxisOnly = true;
        }

        public AreaSelection AreaSelection
        {
            get { return (AreaSelection)GetValue(SelectionProperty); }
            set { SetValue(SelectionProperty, value); }
        }

        /// <summary>
        /// Gets or sets the ID of the Y-Axis which this Annotation is measured against
        /// </summary>
        public string YAxisId
        {
            get { return (string)GetValue(YAxisIdProperty); }
            set { SetValue(YAxisIdProperty, value); }
        }

        /// <summary>
        /// Called when the Chart Modifier is attached to the Chart Surface
        /// </summary>
        /// <remarks></remarks>
        public override void OnAttached()
        {
            base.OnAttached();

            ClearReticule();
        }

        /// <summary>
        /// Called when the Chart Modifier is detached from the Chart Surface
        /// </summary>
        /// <remarks></remarks>
        public override void OnDetached()
        {
            base.OnDetached();

            ClearReticule();
        }

        /// <summary>
        /// Called when a Mouse Button is pressed on the parent <see cref="SciChartSurface"/>
        /// </summary>
        /// <param name="e">Arguments detailing the mouse button operation</param>
        /// <remarks></remarks>
        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);

            if (_isDragging || !MatchesExecuteOn(e.MouseButtons, e.Modifier))
                return;

            // Exit if the mouse down was outside the bounds of the ModifierSurface
            if (e.IsMaster && !ModifierSurface.GetBoundsRelativeTo(ModifierSurface).Contains(e.MousePoint))
                return;

            e.Handled = true;

            ModifierSurface.CaptureMouse();

            // Translate the mouse point (which is in RootGrid coordiantes) relative to the ModifierSurface
            // This accounts for any offset due to left Y-Axis
            Point ptTrans = RootGrid.TranslatePoint(e.MousePoint, ModifierSurface);
            _startPoint = ptTrans;
            _rectangle = new Rect();

            SetPosition(_startPoint, _startPoint);

            _isDragging = true;
        }

        private void UpdateSurface()
        {
            AreaSelection = AreaSelection ?? new AreaSelection { Surface = ParentSurface };

            var x = _rectangle.X;
           
            var width = _rectangle.Width;
           
            AreaSelection.SetSelectionRect(x, width);

            ParentSurface.ViewportManager.InvalidateParentSurface(RangeMode.None);
        }

        /// <summary>
        /// Called when the Mouse is moved on the parent <see cref="SciChartSurface"/>
        /// </summary>
        /// <param name="e">Arguments detailing the mouse move operation</param>
        /// <remarks></remarks>
        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            if (!_isDragging)
                return;

            base.OnModifierMouseMove(e);
            e.Handled = true;

            // Translate the mouse point (which is in RootGrid coordiantes) relative to the ModifierSurface
            // This accounts for any offset due to left Y-Axis
            Point ptTrans = RootGrid.TranslatePoint(e.MousePoint, ModifierSurface);

            SetPosition(_startPoint, ptTrans);

            UpdateSurface();
        }

        /// <summary>
        /// Called when a Mouse Button is released on the parent <see cref="SciChartSurface"/>
        /// </summary>
        /// <param name="e">Arguments detailing the mouse button operation</param>
        /// <remarks></remarks>
        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            if (!_isDragging)
                return;

            base.OnModifierMouseUp(e);
            e.Handled = true;

            // Translate the mouse point (which is in RootGrid coordiantes) relative to the ModifierSurface
            // This accounts for any offset due to left Y-Axis
            //Point ptTrans = RootGrid.TranslatePoint(e.MousePoint, ModifierSurface);

            _rectangle = Rect.Empty;

            UpdateSurface();

            ClearReticule();

            _isDragging = false;

            ModifierSurface.ReleaseMouseCapture();
        }

        private Point SetPosition(Point startPoint, Point endPoint)
        {
            if (XAxis.IsHorizontalAxis)
            {
                startPoint.Y = 0;
                endPoint.Y = ModifierSurface.ActualHeight;
            }
            else
            {
                startPoint.X = 0;
                endPoint.X = ModifierSurface.ActualWidth;
            }

            var modifierRect = new Rect(0, 0, ModifierSurface.ActualWidth, ModifierSurface.ActualHeight);
            endPoint = ClipToBounds(modifierRect, endPoint);

            _rectangle = new Rect(startPoint, endPoint);

            return endPoint;
        }

        private Point ClipToBounds(Rect rect, Point point)
        {
            double rightEdge = rect.Right;
            double leftEdge = rect.Left;
            double topEdge = rect.Top;
            double bottomEdge = rect.Bottom;

            point.X = point.X > rightEdge ? rightEdge : point.X;
            point.X = point.X < leftEdge ? leftEdge : point.X;
            point.Y = point.Y > bottomEdge ? bottomEdge : point.Y;
            point.Y = point.Y < topEdge ? topEdge : point.Y;

            return point;
        }

        private void ClearReticule()
        {
            _isDragging = false;
            _rectangle = Rect.Empty;
        }
    }
}