using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals;
using SciChart.Data.Model;
using SciChart.Core.Extensions;
using System;
using System.Linq;
using System.Windows;
using SciChart.Core.Utility.Mouse;
using System.Windows.Media;
using System.Windows.Shapes;
using SciChart.Charting.StrategyManager;
using System.Windows.Controls;
using SciChart.Drawing.Utility;

namespace CustomShapeZoomModifier
{
    public class ShapedRubberBandZoomModifier : ChartModifierBase
    {
        /// <summary>
        /// Defines the ShapeFill dependency property
        /// </summary>
        public static readonly DependencyProperty ShapeFillProperty = DependencyProperty.Register(nameof(ShapeFill), typeof(Brush), typeof(ShapedRubberBandZoomModifier), new PropertyMetadata(null));

        /// <summary>
        /// Defines the ShapeStroke dependency property
        /// </summary>
        public static readonly DependencyProperty ShapeStrokeProperty = DependencyProperty.Register(nameof(ShapeStroke), typeof(Brush), typeof(ShapedRubberBandZoomModifier), new PropertyMetadata(null));

        /// <summary>
        /// Defines the Modifier's Shape thickness
        /// </summary>
        public static readonly DependencyProperty ModifierShapeStrokeThicknessProperty = DependencyProperty.Register(nameof(ModifierShapeStrokeThickness), typeof(int), typeof(ShapedRubberBandZoomModifier), new PropertyMetadata(4));

        /// <summary>
        /// Defines the Modifier's Shape thickness
        /// </summary>
        public static readonly DependencyProperty ModifierEdgeShapeSizeProperty = DependencyProperty.Register(nameof(ModifierEdgeShapeSize), typeof(int), typeof(ShapedRubberBandZoomModifier), new PropertyMetadata(4));

        /// <summary>
        /// Defines the MinDragSensitivity DependencyProperty
        /// </summary>
        public static readonly DependencyProperty MinDragSensitivityProperty = DependencyProperty.Register(nameof(MinDragSensitivity), typeof(double), typeof(ShapedRubberBandZoomModifier), new PropertyMetadata(10.0));

        /// <summary>
        /// Defines in which direction we're zooming
        /// </summary>
        public static readonly DependencyProperty ZoomOnAxisProperty = DependencyProperty.Register(nameof(ZoomOnAxis), typeof(ModifierAxis), typeof(ShapedRubberBandZoomModifier), new PropertyMetadata(ModifierAxis.XY));

        private Point _startPoint;
        private Point _endPoint;

        private bool _isDragging;

        private Rectangle _rectangleShape;
        private Line _mainLineShape;
        private Line _leftEdgeLineShape;
        private Line _rightEdgeLineShape;

        /// <summary>
        /// Gets whether the user is currently dragging the mouse
        /// </summary>
        public bool IsDragging => _isDragging;

        /// <summary>
        /// Gets or sets the Fill brush of the recticule drawn on the screen as the user zooms
        /// </summary>
        public Brush ShapeFill
        {
            get => (Brush)GetValue(ShapeFillProperty);
            set => SetValue(ShapeFillProperty, value);
        }

        /// <summary>
        /// Gets or sets the Stroke brush of the recticule drawn on the screen as the user zooms
        /// </summary>
        public Brush ShapeStroke
        {
            get => (Brush)GetValue(ShapeStrokeProperty);
            set => SetValue(ShapeStrokeProperty, value);
        }

        public ModifierAxis ZoomOnAxis
        {
            get => (ModifierAxis)GetValue(ZoomOnAxisProperty);
            set => SetValue(ZoomOnAxisProperty, value);
        }

        /// <summary>
        /// Gets or sets the drag sensitivity - rectangles dragged smaller than this size in the diagonal will be ignored when zooming. Default is 10 pixels
        /// </summary>
        public double MinDragSensitivity
        {
            get => (double)GetValue(MinDragSensitivityProperty);
            set => SetValue(MinDragSensitivityProperty, value);
        }

        public int ModifierShapeStrokeThickness
        {
            get => (int)GetValue(ModifierShapeStrokeThicknessProperty);
            set => SetValue(ModifierShapeStrokeThicknessProperty, value);
        }

        public int ModifierEdgeShapeSize
        {
            get => (int)GetValue(ModifierEdgeShapeSizeProperty);
            set => SetValue(ModifierEdgeShapeSizeProperty, value);
        }

        public override void OnAttached()
        {
            base.OnAttached();
            StopDragging(true);
        }

        public override void OnDetached()
        {
            base.OnDetached();
            StopDragging(true);
        }

        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);

            if (_isDragging ||
                !MatchesExecuteOn(e.MouseButtons, e.Modifier))
                return;

            // Exit if the mouse down was outside the bounds of the Master ModifierSurface
            // e.g. if this is a slave or master, only start dragging if mousedown occurred on master ModifierSurface. 
            if (!(e.Source is IChartModifier) || !IsPointWithinBounds(e))
                return;

            // Translate the mouse point (which is in RootGrid coordiantes) relative to the ModifierSurface
            // This accounts for any offset due to left Y-Axis
            var ptTrans = GetPointRelativeTo(e.MousePoint, ModifierSurface);

            e.Handled = true;
            _startPoint = ptTrans;

            // StartDragging 
            StartDragging(e.IsMaster);
        }

        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            if (!_isDragging)
                return;

            base.OnModifierMouseMove(e);
            e.Handled = true;

            // Translate the mouse point (which is in RootGrid coordiantes) relative to the ModifierSurface
            // This accounts for any offset due to left Y-Axis
            var ptTrans = GetPointRelativeTo(e.MousePoint, ModifierSurface);

            // Then UPDATE The SHAPE
            //UpdateShape(false, _startPoint, ptTrans); // IsXAxisOnly will be used later
            UpdateShape(_startPoint, ptTrans);
        }

        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            if (!_isDragging)
                return;

            base.OnModifierMouseUp(e);

            var strategy = Services.GetService<IStrategyManager>().GetTransformationStrategy();

            // Translate the mouse point (which is in RootGrid coordiantes) relative to the ModifierSurface
            // This accounts for any offset due to left Y-Axis
            var ptTrans = GetPointRelativeTo(e.MousePoint, ModifierSurface);

            // then update shape and SET endPoint 
            //_endPoint = UpdateShape(false, _startPoint, ptTrans);
            UpdateShape(_startPoint, ptTrans);
            _endPoint = ptTrans;

            var startPoint = strategy.Transform(_startPoint);
            var endPoint = strategy.Transform(_endPoint);
            var currentPoint = strategy.Transform(ptTrans);

            // Check for dragged distance(if it's bigger than min dragged sensitivity
            if (PointUtil.Distance(startPoint, currentPoint) > MinDragSensitivity)
            {
                // Zoom only if user drew a rectangle
                PerformZoom(startPoint, endPoint);

                e.Handled = true;

                if (ParentSurface.ZoomHistoryManager != null)
                {
                    ParentSurface.ZoomHistoryManager.PushAll();
                }
            }

            StopDragging(e.IsMaster);
        }

        public void CreateShape()
        {
            if (ZoomOnAxis == ModifierAxis.XY)
            {
                // Rubberband case
                _rectangleShape = new Rectangle
                {
                    Fill = ShapeFill,
                    Stroke = ShapeStroke,
                };

                Canvas.SetLeft(_rectangleShape, _startPoint.X);
                Canvas.SetTop(_rectangleShape, _startPoint.Y);
                _rectangleShape.Width = 1;
                _rectangleShape.Height = 1;
            }
            else
            {
                // Line case
                _mainLineShape = new Line
                {
                    Stroke = ShapeStroke,
                    StrokeThickness = ModifierShapeStrokeThickness,
                };

                _leftEdgeLineShape = new Line
                {
                    Stroke = ShapeStroke,
                    StrokeThickness = ModifierShapeStrokeThickness,
                };

                _rightEdgeLineShape = new Line
                {
                    Stroke = ShapeStroke,
                    StrokeThickness = ModifierShapeStrokeThickness,
                };

                _mainLineShape.X1 = _mainLineShape.X2 = _startPoint.X;
                _mainLineShape.Y1 = _mainLineShape.Y2 = _startPoint.Y;

                var edgeHalfSize = ModifierEdgeShapeSize / 2;

                if (ZoomOnAxis == ModifierAxis.X)
                {
                    // Horizontal case, place Edges Vertically 
                    _leftEdgeLineShape.Y1 = _rightEdgeLineShape.Y1 = _startPoint.Y - edgeHalfSize;
                    _leftEdgeLineShape.Y2 = _rightEdgeLineShape.Y2 = _startPoint.Y + edgeHalfSize;
                    _leftEdgeLineShape.X1 = _leftEdgeLineShape.X2 = _rightEdgeLineShape.X1 = _rightEdgeLineShape.X2 = _startPoint.X;
                }
                else
                {
                    // Vertical case, place Edges Horizontally
                    _leftEdgeLineShape.X1 = _rightEdgeLineShape.X1 = _startPoint.X - edgeHalfSize;
                    _leftEdgeLineShape.X2 = _rightEdgeLineShape.X2 = _startPoint.X + edgeHalfSize;
                    _leftEdgeLineShape.Y1 = _leftEdgeLineShape.Y2 = _rightEdgeLineShape.Y1 = _rightEdgeLineShape.Y2 = _startPoint.Y;
                }
            }
        }

        public void UpdateShape(Point start, Point end)
        {
            var modifierRect = new Rect(0, 0, ModifierSurface.ActualWidth, ModifierSurface.ActualHeight);
            end = modifierRect.ClipToBounds(end);

            if (ZoomOnAxis == ModifierAxis.XY)
            {
                var rect = new Rect(start, end);
                Canvas.SetLeft(_rectangleShape, rect.X);
                Canvas.SetTop(_rectangleShape, rect.Y);

                _rectangleShape.Width = rect.Width;
                _rectangleShape.Height = rect.Height;
            }
            else
            {
                if (ZoomOnAxis == ModifierAxis.X)
                {
                    _mainLineShape.X2 = _rightEdgeLineShape.X1 = _rightEdgeLineShape.X2 = end.X;
                }
                else
                {
                    _mainLineShape.Y2 = _rightEdgeLineShape.Y1 = _rightEdgeLineShape.Y2 = end.Y;
                }
            }
        }

        private void AddZoomShapeToCanvas()
        {
            if (ZoomOnAxis == ModifierAxis.XY)
            {
                ModifierSurface.Children.Add(_rectangleShape);
            }
            else
            {
                ModifierSurface.Children.Add(_leftEdgeLineShape);
                ModifierSurface.Children.Add(_mainLineShape);
                ModifierSurface.Children.Add(_rightEdgeLineShape);
            }
        }

        private void StartDragging(bool captureMouse)
        {
            CreateShape();
            AddZoomShapeToCanvas();

            if (captureMouse)
            {
                ModifierSurface.CaptureMouse();
            }
            _isDragging = true;
        }

        public void PerformZoom(Point startPoint, Point endPoint)
        {
            if (Math.Abs(startPoint.X - endPoint.X) < double.Epsilon ||
                Math.Abs(startPoint.Y - endPoint.Y) < double.Epsilon)
                return;

            if (XAxes?.Any() != true || YAxes?.Any() != true)
                return;

            SetZoomState(ZoomStates.UserZooming);

            var zoomRect = new Rect(startPoint, endPoint);

            using (ParentSurface.SuspendUpdates())
            {
                if (ZoomOnAxis == ModifierAxis.X || ZoomOnAxis == ModifierAxis.XY)
                {
                    foreach (var xAxis in XAxes)
                    {
                        PerformZoomOnAxis(xAxis, zoomRect);
                    }
                }

                if (ZoomOnAxis == ModifierAxis.Y || ZoomOnAxis == ModifierAxis.XY)
                {
                    foreach (var yAxis in YAxes)
                    {
                        PerformZoomOnAxis(yAxis, zoomRect);
                    }
                }
            }
        }

        private void PerformZoomOnAxis(IAxis axis, Rect zoomRect)
        {
            if (axis == null) return;

            var fromCoord = axis.IsHorizontalAxis ? zoomRect.Left : zoomRect.Bottom;
            var toCoord = axis.IsHorizontalAxis ? zoomRect.Right : zoomRect.Top;

            var min = axis.GetDataValue(fromCoord).ToDouble();
            var max = axis.GetDataValue(toCoord).ToDouble();

            var toRange = RangeFactory.NewWithMinMax(axis.VisibleRange, min, max);
            axis.AnimateVisibleRangeTo(toRange, TimeSpan.FromMilliseconds(500));
        }

        private void StopDragging(bool releaseMouseCapture)
        {
            if (ModifierSurface != null)
            {
                if (_rectangleShape != null)
                {
                    ModifierSurface.Children.Remove(_rectangleShape);
                }
                if (_mainLineShape != null)
                {
                    ModifierSurface.Children.Remove(_leftEdgeLineShape);
                    ModifierSurface.Children.Remove(_mainLineShape);
                    ModifierSurface.Children.Remove(_rightEdgeLineShape);
                }

                if (releaseMouseCapture)
                {
                    ModifierSurface.ReleaseMouseCapture();
                }
            }

            _isDragging = false;

            _rectangleShape = null;
            _leftEdgeLineShape = null;
            _mainLineShape = null;
            _rightEdgeLineShape = null;
        }
    }
}
