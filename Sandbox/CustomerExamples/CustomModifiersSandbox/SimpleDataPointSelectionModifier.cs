using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Utility;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Core.Utility.Mouse;
using SciChart.Drawing.Utility;

namespace CustomModifierSandboxExample
{
    public struct DataPoint
    {
        public double XValue;
        public double YValue;
        public int Index;
    }

    public class SimpleDataPointSelectionModifier : ChartModifierBase
    {
        public static readonly DependencyProperty SelectionPolygonStyleProperty = DependencyProperty.Register
            (nameof(SelectionPolygonStyle), typeof(Style), typeof(SimpleDataPointSelectionModifier), new PropertyMetadata(default(Style)));

        public static readonly DependencyProperty SelectedPointsProperty = DependencyProperty.Register
            (nameof(SelectedPoints), typeof(IDictionary<IRenderableSeries, List<DataPoint>>), typeof(SimpleDataPointSelectionModifier),
            new PropertyMetadata(default(IDictionary<IRenderableSeries, List<DataPoint>>)));

        public static readonly DependencyProperty SelectedPointColorProperty = DependencyProperty.Register
            (nameof(SelectedPointColor), typeof(Color), typeof(SimpleDataPointSelectionModifier), new PropertyMetadata(Colors.White));

        public static readonly DependencyProperty SelectedPointSizeProperty = DependencyProperty.Register
            (nameof(SelectedPointSize), typeof(double), typeof(SimpleDataPointSelectionModifier), new PropertyMetadata(5.0));

        private Rectangle _rectangle;
        private bool _isDragging;

        private Point _startPoint;
        private Point _endPoint;

        /// <summary>
        /// This is the Color we will draw our selected points ellipse
        /// </summary>
        public Color SelectedPointColor
        {
            get => (Color)GetValue(SelectedPointColorProperty);
            set => SetValue(SelectedPointColorProperty, value);
        }

        /// <summary>
        /// This is the Size of the selected points ellipse
        /// </summary>
        public double SelectedPointSize
        {
            get => (double)GetValue(SelectedPointSizeProperty);
            set => SetValue(SelectedPointSizeProperty, value);
        }

        /// <summary>
        /// This is the dictionary of selected points, which you can bind TwoWay to a viewmodel property if you want notifications of selected changed
        /// </summary>
        public IDictionary<IRenderableSeries, List<DataPoint>> SelectedPoints
        {
            get => (IDictionary<IRenderableSeries, List<DataPoint>>)GetValue(SelectedPointsProperty);
            set => SetValue(SelectedPointsProperty, value);
        }

        /// <summary>
        /// This is the style applied to the drag rectangle as you select points
        /// </summary>
        public Style SelectionPolygonStyle
        {
            get => (Style)GetValue(SelectionPolygonStyleProperty);
            set => SetValue(SelectionPolygonStyleProperty, value);
        }

        /// <summary>
        /// Gets whether the user is currently dragging the mouse
        /// </summary>
        public bool IsDragging => _isDragging;

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
        /// Called when the IsEnabled property changes on this <see cref="ChartModifierBase" /> instance
        /// </summary>
        protected override void OnIsEnabledChanged()
        {
            base.OnIsEnabledChanged();

            if (!IsEnabled)
            {
                ClearReticule();
                SelectedPoints = null;
            }
        }

        /// <summary>
        /// Called when a Mouse Button is pressed on the parent <see cref="SciChartSurface" />
        /// </summary>
        /// <param name="e">Arguments detailing the mouse button operation</param>
        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);

            // Check the ExecuteOn property and if we are already dragging. If so, exit
            if (_isDragging || !MatchesExecuteOn(e.MouseButtons, e.Modifier))
            {
                return;
            }

            // Check the mouse point was inside the ModifierSurface (the central chart area). If not, exit
            var modifierSurfaceBounds = ModifierSurface.GetBoundsRelativeTo(RootGrid);
            if (!modifierSurfaceBounds.Contains(e.MousePoint))
            {
                return;
            }

            // Capture the mouse, so if mouse goes out of bounds, we retain mouse events
            if (e.IsMaster)
            {
                ModifierSurface.CaptureMouse();
            }

            // Translate the mouse point (which is in RootGrid coordiantes) relative to the ModifierSurface
            // This accounts for any offset due to left Y-Axis
            var ptTrans = GetPointRelativeTo(e.MousePoint, ModifierSurface);

            _startPoint = ptTrans;
            _rectangle = new Rectangle
            {
                Style = SelectionPolygonStyle,
            };

            // Update the zoom recticule position
            SetReticulePosition(_rectangle, _startPoint, _startPoint);

            // Add the zoom reticule to the ModifierSurface - a canvas over the chart
            ModifierSurface.Children.Add(_rectangle);

            // Set flag that a drag has begun
            _isDragging = true;
        }

        /// <summary>
        /// Called when the Mouse is moved on the parent <see cref="SciChartSurface" />
        /// </summary>
        /// <param name="e">Arguments detailing the mouse move operation</param>
        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            if (!_isDragging) return;

            base.OnModifierMouseMove(e);

            e.Handled = true;

            // Translate the mouse point (which is in RootGrid coordiantes) relative to the ModifierSurface
            // This accounts for any offset due to left Y-Axis
            var ptTrans = GetPointRelativeTo(e.MousePoint, ModifierSurface);

            // Update the zoom recticule position
            SetReticulePosition(_rectangle, _startPoint, ptTrans);
        }

        /// <summary>
        /// Called when a Mouse Button is released on the parent <see cref="SciChartSurface" />
        /// </summary>
        /// <param name="e">Arguments detailing the mouse button operation</param>
        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            if (!_isDragging) return;

            base.OnModifierMouseUp(e);

            // Translate the mouse point (which is in RootGrid coordiantes) relative to the ModifierSurface
            // This accounts for any offset due to left Y-Axis
            var ptTrans = GetPointRelativeTo(e.MousePoint, ModifierSurface);

            _endPoint = SetReticulePosition(_rectangle, _startPoint, ptTrans);

            double distanceDragged = PointUtil.Distance(_startPoint, ptTrans);
            if (distanceDragged > 10.0)
            {
                PerformSelection(_startPoint, _endPoint);
                e.Handled = true;
            }
            else
            {
                SelectedPoints = null;
                ParentSurface.InvalidateElement();
            }

            ClearReticule();
            _isDragging = false;

            if (e.IsMaster)
            {
                ModifierSurface.ReleaseMouseCapture();
            }
        }

        /// <summary>
        /// Called when the parent <see cref="SciChartSurface" /> is rendered. Here is where we draw selected points
        /// </summary>
        /// <param name="e">The <see cref="SciChartRenderedMessage" /> which contains the event arg data</param>
        public override void OnParentSurfaceRendered(SciChartRenderedMessage e)
        {
            base.OnParentSurfaceRendered(e);

            var selectedPoints = SelectedPoints;
            if (selectedPoints == null) return;

            double size = SelectedPointSize;

            // Create Fill brush for the point marker
            using (var fill = e.RenderContext.CreateBrush(SelectedPointColor))
            {
                // Iterating over renderable series
                foreach (var renderSeries in selectedPoints.Keys)
                {
                    // Create stroke pen based on r-series color
                    using (var stroke = e.RenderContext.CreatePen(renderSeries.Stroke, true, 1.0f))
                    {
                        // We need XAxis/YAxis.GetCurrentCoordinateCalculator() from the current series (as they can be per-series)
                        // to convert data points to pixel coords
                        var xAxis = renderSeries.XAxis;
                        var yAxis = renderSeries.YAxis;
                        var xCalc = xAxis.GetCurrentCoordinateCalculator();
                        var yCalc = yAxis.GetCurrentCoordinateCalculator();

                        var pointList = selectedPoints[renderSeries];

                        // Iterate over the selected points
                        foreach (var point in pointList)
                        {
                            // Draw the selected point marker
                            e.RenderContext.DrawEllipse(
                                stroke,
                                fill,
                                new Point(xCalc.GetCoordinate(point.XValue), yCalc.GetCoordinate(point.YValue)),
                                size,
                                size);
                        }
                    }
                }
            }
        }

        private void ClearReticule()
        {
            if (ModifierSurface != null && _rectangle != null)
            {
                ModifierSurface.Children.Remove(_rectangle);
                _rectangle = null;
                _isDragging = false;
            }
        }

        private Point SetReticulePosition(Rectangle rectangle, Point startPoint, Point endPoint)
        {
            var modifierRect = new Rect(0, 0, ModifierSurface.ActualWidth, ModifierSurface.ActualHeight);
            endPoint = modifierRect.ClipToBounds(endPoint);
            var rect = new Rect(startPoint, endPoint);

            Canvas.SetLeft(rectangle, rect.X);
            Canvas.SetTop(rectangle, rect.Y);

            rectangle.Width = rect.Width;
            rectangle.Height = rect.Height;

            return endPoint;
        }

        private void PerformSelection(Point startPoint, Point endPoint)
        {
            // Find all the points that are inside this bounds and store them     
            var dataPoints = new Dictionary<IRenderableSeries, List<DataPoint>>();
            foreach (var renderSeries in base.ParentSurface.RenderableSeries)
            {
                var dataSeries = renderSeries.DataSeries;
                if (dataSeries == null) continue;

                // We're going to convert our start/end mouse points to data values using the CoordinateCalculator API
                // Note: that RenderSeries can have different XAxis, YAxis, so we use the axes from the RenderSeries not the primary axes on the chart
                var xCalc = renderSeries.XAxis.GetCurrentCoordinateCalculator();
                var yCalc = renderSeries.YAxis.GetCurrentCoordinateCalculator();

                // Find the bounds of the data inside the rectangle
                var leftXData = xCalc.GetDataValue(startPoint.X);
                var rightXData = xCalc.GetDataValue(endPoint.X);
                var topYData = yCalc.GetDataValue(startPoint.Y);
                var bottomYData = yCalc.GetDataValue(endPoint.Y);
                var dataRect = new Rect(new Point(leftXData, topYData), new Point(rightXData, bottomYData));

                dataPoints[renderSeries] = new List<DataPoint>();

                for (int i = 0; i < dataSeries.Count; i++)
                {
                    var currentPoint = new Point(((DateTime)dataSeries.XValues[i]).Ticks, (double)dataSeries.YValues[i]);
                    if (dataRect.Contains(currentPoint))
                    {
                        dataPoints[renderSeries].Add(new DataPoint() { Index = i, XValue = currentPoint.X, YValue = currentPoint.Y });
                    }
                }
            }

            SelectedPoints = dataPoints;

            ParentSurface.InvalidateElement();
        }
    }
}
