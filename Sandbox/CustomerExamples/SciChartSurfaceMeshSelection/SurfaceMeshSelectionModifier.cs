using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D;
using SciChart.Charting3D.Modifiers;
using SciChart.Charting3D.Primitives;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Core.Utility.Mouse;
using SciChart.Drawing.Utility;
using SelectionMode = SciChart.Charting.ChartModifiers.SelectionMode;

namespace SciChartSurfaceMeshSelection
{
    public class SurfaceMeshSelectionModifier : ChartModifierBase3D
    {
        /// <summary>
        /// Defines the IncludeSeries AttachedProperty. When set to True on a <see cref="BaseRenderableSeries3D"/>, 
        /// the series will be included in vertex selection. When false, the series will be excluded
        /// </summary>
        public static readonly DependencyProperty IncludeSeriesProperty = DependencyProperty.RegisterAttached
            ("IncludeSeries", typeof(bool), typeof(SurfaceMeshSelectionModifier), new PropertyMetadata(true));

        /// <summary>
        /// Defined DragReticuleStyle DependencyProperty. This is the style (TargetType=Rectangle) which is applied to the drag rectangle
        /// </summary>
        public static readonly DependencyProperty DragReticuleStyleProperty = DependencyProperty.Register
            (nameof(DragReticuleStyle), typeof(Style), typeof(SurfaceMeshSelectionModifier), new PropertyMetadata(default(Style), DragReticuleStylePropertyChanged));

        /// <summary>
        /// Defines the MinDragSensitivity DependencyProperty
        /// </summary>
        public static readonly DependencyProperty MinDragSensitivityProperty = DependencyProperty.Register
            (nameof(MinDragSensitivity), typeof(double), typeof(SurfaceMeshSelectionModifier), new PropertyMetadata(3.0));

        public SurfaceMeshMetadataPaletteProvider BoundedPaletteProvider { get; set; }

        private readonly Rectangle _dragReticule = new Rectangle();

        private readonly Dictionary<uint, HashSet<ulong>> _selectedVertices = new Dictionary<uint, HashSet<ulong>>();

        /// <summary>
        /// Initializes a new instance of the <see cref="SurfaceMeshSelectionModifier"/> class.
        /// </summary>
        public SurfaceMeshSelectionModifier()
        {
            DefaultStyleKey = typeof(SurfaceMeshSelectionModifier);
        }

        /// <summary>
        /// Gets the StartPoint for the drag operation
        /// </summary>
        public Point StartPoint { get; protected set; }

        /// <summary>
        /// Gets or sets the style to apply to the Drag Rectangle. TargetType is System.Windows.Shapes.Rectangle
        /// </summary>
        public Style DragReticuleStyle
        {
            get => (Style)GetValue(DragReticuleStyleProperty);
            set => SetValue(DragReticuleStyleProperty, value);
        }

        /// <summary>
        /// Gets or sets the drag sensitivity - rectangles dragged smaller than this size in the diagonal will be ignored when selecting. Default is 10 pixels
        /// </summary>
        public double MinDragSensitivity
        {
            get => (double)GetValue(MinDragSensitivityProperty);
            set => SetValue(MinDragSensitivityProperty, value);
        }

        /// <summary>
        /// Gets the include Series or not
        /// </summary>
        public static bool GetIncludeSeries(DependencyObject obj)
        {
            return (bool)obj.GetValue(IncludeSeriesProperty);
        }

        /// <summary>
        /// Sets the include Series or not
        /// </summary>
        public static void SetIncludeSeries(DependencyObject obj, bool value)
        {
            obj.SetValue(IncludeSeriesProperty, value);
        }

        /// <summary>
        /// Called when the Chart Modifier is attached to the Chart Surface
        /// </summary>
        public override void OnAttached()
        {
            base.OnAttached();
            ClearAll();
        }

        /// <summary>
        /// Called immediately before the Chart Modifier is detached from the Chart Surface
        /// </summary>
        public override void OnDetached()
        {
            base.OnDetached();
            ClearAll();
        }

        /// <summary>
        /// Called when a Mouse Button is pressed on the parent <see cref="SciChart3DSurface" />
        /// </summary>
        /// <param name="e">Arguments detailing the mouse button operation</param>
        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            base.OnModifierMouseDown(e);

            if (!IsDragging && MatchesExecuteOn(e.MouseButtons, ExecuteOn) && MatchesExecuteWhen(e.Modifier))
            {
                CaptureMouse();
                IsDragging = true;
                StartPoint = e.MousePoint;
                UpdateReticulePosition(StartPoint, e.MousePoint);
                ModifierSurface.Children.AddIfNotContains(_dragReticule);
            }
        }

        /// <summary>
        /// Called when the Mouse is moved on the parent <see cref="SciChart3DSurface" />
        /// </summary>
        /// <param name="e">Arguments detailing the mouse move operation</param>
        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            if (IsDragging)
            {
                base.OnModifierMouseMove(e);

                UpdateReticulePosition(StartPoint, e.MousePoint);
            }
        }

        /// <summary>
        /// When overriden in derived classes, is used to override the default selection behavior.
        /// </summary>
        /// <param name="modifierKey">The modifier key which has been pressed.</param>
        /// <param name="isAreaSelection"><value>True</value> when selection was performed by dragging a reticule, othetwise <value>False</value>.</param>
        /// <returns></returns>
        protected virtual SelectionMode GetSelectionMode(MouseModifier modifierKey, bool isAreaSelection)
        {
            var selectionMode = SelectionMode.Replace;

            if (modifierKey.HasFlag(ExecuteWhen == MouseModifier.Ctrl ? MouseModifier.Shift : MouseModifier.Ctrl))
            {
                selectionMode = isAreaSelection ? SelectionMode.Union : SelectionMode.Inverse;
            }
            return selectionMode;
        }

        /// <summary>
        /// Determines whether the currently pressed modifier key matches the <see cref="ChartModifierBase3D.ExecuteWhen"/> 
        /// </summary>
        protected bool MatchesExecuteWhen(MouseModifier modifier)
        {
            if (ExecuteWhen == MouseModifier.Ctrl)
            {
                return modifier == MouseModifier.Ctrl || modifier == (MouseModifier.Ctrl | MouseModifier.Shift);
            }
            return modifier == ExecuteWhen || modifier == (ExecuteWhen | MouseModifier.Ctrl);
        }

        /// <summary>
        /// Called when a Mouse Button is released on the parent <see cref="SciChart3DSurface" />
        /// </summary>
        /// <param name="e">Arguments detailing the mouse button operation</param>
        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            base.OnModifierMouseUp(e);

            ClearAll();

            if (!IsDragging || Viewport3D == null || Viewport3D.RootEntity == null) return;

            var endPoint = e.MousePoint;
            var distanceDragged = PointUtil.Distance(StartPoint, endPoint);
            var isAreaSelection = distanceDragged > MinDragSensitivity;

            IList<EntityVertexId> hitEntityVertexIds;
            if (isAreaSelection)
            {
                // Drag select
                hitEntityVertexIds = Viewport3D.PickScene(new Rect(StartPoint, e.MousePoint));
            }
            else
            {
                // Point select
                var vertexId = Viewport3D.PickScene(e.MousePoint);
                hitEntityVertexIds = vertexId.HasValue ? new[] { vertexId.Value } : new EntityVertexId[0];
            }
            if (!hitEntityVertexIds.IsNullOrEmpty())
            {
                var hitEntityGroups = hitEntityVertexIds
                    .GroupBy(x => x.EntityId)
                    .ToDictionary(x => x.Key, x => x.Select(i => new VertexId { Id = i.VertexId }).ToList());

                var xSize = BoundedPaletteProvider.XSize - 1;

                //Visit entities to perform selection or deselection
                Viewport3D.RootEntity.VisitEntities(entity =>
                {
                    var entityId = entity.EntityId;
                    var hitVertexIds = new List<VertexId>();

                    if (hitEntityGroups.ContainsKey(entityId))
                    {
                        hitVertexIds = hitEntityGroups[entityId];
                    }

                    if (hitVertexIds.Any())
                    {
                        if (!_selectedVertices.ContainsKey(entityId))
                            _selectedVertices.Add(entityId, new HashSet<ulong>());


                        var selectedVertices = _selectedVertices[entityId];
                        hitVertexIds.ForEach(x => selectedVertices.Add(x.Id));

                        BoundedPaletteProvider.SelectedIndexes.Clear();
                        foreach (var hitEntityVertexId in hitEntityVertexIds)
                        {
                            var id = Convert.ToInt32(hitEntityVertexId.VertexId) - 1;
                            var vertexIndexInfo = new SurfaceMeshVertexInfo();
                            if (id < xSize)
                            {
                                vertexIndexInfo.XIndex = id;
                            }
                            else
                            {
                                vertexIndexInfo.ZIndex = id / xSize;
                                vertexIndexInfo.XIndex = id - (vertexIndexInfo.ZIndex * xSize);
                            }

                            BoundedPaletteProvider.SelectedIndexes.Add(vertexIndexInfo);
                        }
                    }
                    else
                    {
                        _selectedVertices.Remove(entityId);
                        BoundedPaletteProvider.SelectedIndexes.Clear();
                    }
                });
            }

            IsDragging = false;
            ReleaseMouseCapture();
            BoundedPaletteProvider.DataSeries.IsDirty = true;
            BoundedPaletteProvider.DataSeries.OnDataSeriesChanged(DataSeriesUpdate.SelectionChanged,
                DataSeriesAction.None);
        }

        /// <summary>
        /// Clears all drag reticules from the chart surface
        /// </summary>
        protected virtual void ClearAll()
        {
            if (ModifierSurface != null)
            {
                ModifierSurface.Children.Remove(_dragReticule);
            }
        }

        /// <summary>
        /// Updates the drag reticule position.
        /// </summary>
        /// <param name="startPoint">The start point.</param>
        /// <param name="endPoint">The end point.</param>
        protected virtual void UpdateReticulePosition(Point startPoint, Point endPoint)
        {
            var modifierRect = new Rect(0, 0, ModifierSurface.ActualWidth, ModifierSurface.ActualHeight);
            endPoint = modifierRect.ClipToBounds(endPoint);
            var rect = new Rect(startPoint, endPoint);

            _dragReticule.Width = rect.Width;
            _dragReticule.Height = rect.Height;

            Canvas.SetLeft(_dragReticule, rect.X);
            Canvas.SetTop(_dragReticule, rect.Y);
        }

        private static void DragReticuleStylePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var modifier = (SurfaceMeshSelectionModifier)d;

            if (args.NewValue is Style newStyle)
            {
                modifier._dragReticule.Style = newStyle;
            }
        }
    }
}
