using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Media3D;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Modifiers;
using SciChart.Charting3D.Primitives;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Core.Utility.Mouse;

namespace _3DChartScatterSeriesWithDraggablePoints
{
    public struct DragPointEventArgs
    {
        public int PointIndex { get; set; }
        public IComparable YValue { get; set; }
    }

    public class DragPointYAxisModifier3D : ChartModifierBase3D
    {
        private Points3DSceneEntity _pointsEntity;

        private EntityVertexId _entityVertexId;

        public event EventHandler<DragPointEventArgs> PointDragStart;

        public event EventHandler<DragPointEventArgs> PointDragDelta;

        public event EventHandler<DragPointEventArgs> PointDragEnd;

        public Point StartPoint { get; protected set; }

        private Vector3D GetPointVector(EntityVertexId entityVertexId)
        {
            return _pointsEntity?.GetVertexAt(new HitTestInfo3D
            {
                IsHit = true,
                EntityId = entityVertexId.EntityId,
                VertexId = entityVertexId.VertexId

            }) ?? new Vector3D(0d, 0d, 0d);
        }

        private Vector3 GetCoordVector(Vector3D pointVector)
        {
            return _pointsEntity?.GetVertexCoords(pointVector) ?? Vector3.Zero;
        }

        private double GetYDragDelta(Point startPoint, Point endPoint, Vector3 coordVector)
        {
            // 1.Create a plane at the highlighted point using its position and camera forward vector
            // 2.Calculate a ray using the camera and mouse x,y position
            // 3.Intersect the ray with the plane giving a point in world space

            // Repeat 1,2,3 using the start point and actual mouse point
            // Calculate the Y delta using 2 positions in world space

            if (Viewport3D == null || coordVector == null) return double.NaN;

            var camera = Viewport3D.GetWorld().GetMainCamera();
            var pointPlane = new TSRPlane(camera.GetFwd(), coordVector);

            var rayStart = new TSRVector3();
            var rayDirection = new TSRVector3();

            camera.ComputeRay((int)startPoint.X, (int)startPoint.Y, rayStart, rayDirection);
            var startVector = pointPlane.Intersect(rayStart, rayDirection);

            camera.ComputeRay((int)endPoint.X, (int)endPoint.Y, rayStart, rayDirection);
            var endVector = pointPlane.Intersect(rayStart, rayDirection);

            startVector.Subtract(startVector, endVector);
            var delta = Math.Abs(startVector.y);

            return endPoint.Y > startPoint.Y ? -delta : delta;
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

                if (Viewport3D?.RootEntity == null) return;

                var entityVertexId = Viewport3D.PickScene(e.MousePoint);
                if (entityVertexId.HasValue)
                {
                    _entityVertexId = entityVertexId.Value;

                    Viewport3D.RootEntity.VisitEntities(entity =>
                    {
                        if (entity is Points3DSceneEntity pointsEntity && entity.EntityId == _entityVertexId.EntityId)
                        {
                            entity.PerformSelection(true, new List<VertexId>
                            {
                                new VertexId { Id = _entityVertexId.VertexId }
                            });

                            _pointsEntity = pointsEntity;

                            PointDragStart?.Invoke(this, new DragPointEventArgs
                            {
                                PointIndex = (int)_entityVertexId.VertexId - 1,
                                YValue = GetPointVector(_entityVertexId).Y
                            });
                        }
                    });
                }
            }
        }

        /// <summary>
        /// Called when the Mouse is moved on the parent <see cref="SciChart3DSurface" />
        /// </summary>
        /// <param name="e">Arguments detailing the mouse move operation</param>
        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            base.OnModifierMouseMove(e);

            if (IsDragging && ParentSurface?.YAxis != null && _pointsEntity != null)
            {
                var pointVector = GetPointVector(_entityVertexId);
                var coordVector = GetCoordVector(pointVector);
                var dragDelta = GetYDragDelta(StartPoint, e.MousePoint, coordVector);

                if (!dragDelta.IsNaN())
                {
                    PointDragDelta?.Invoke(this, new DragPointEventArgs
                    {
                        PointIndex = (int)_entityVertexId.VertexId - 1,
                        YValue = ParentSurface.YAxis.GetDataValue(coordVector.y + dragDelta)
                    });

                    StartPoint = e.MousePoint;
                }
            }
        }

        /// <summary>
        /// Called when a Mouse Button is released on the parent <see cref="SciChart3DSurface" />
        /// </summary>
        /// <param name="e">Arguments detailing the mouse button operation</param>
        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            base.OnModifierMouseUp(e);

            _pointsEntity = null;
            Viewport3D.RootEntity.VisitEntities(entity => entity.PerformSelection(false, null));

            IsDragging = false;
            ReleaseMouseCapture();

            PointDragEnd?.Invoke(this, new DragPointEventArgs
            {
                PointIndex = (int)_entityVertexId.VertexId - 1,
                YValue = GetPointVector(_entityVertexId).Y
            });
        }
    }
}