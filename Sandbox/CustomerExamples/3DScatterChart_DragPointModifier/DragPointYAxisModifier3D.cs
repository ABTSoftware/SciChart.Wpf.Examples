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

namespace Scatter3DChart_DragPointModifier
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

        private double GetYDrag(Point endPoint, Vector3 coordVector)
        {
            if (Viewport3D == null || coordVector == null) return double.NaN;

            // Project 3D point to screen, cast a ray through that screen pixel,
            // find where the ray intersects a plane perpendicular to the camera
            // passing through the original 3D point, and return the Y of that intersection.
            var camera = Viewport3D.GetWorld().GetMainCamera();
            var rayStart = new TSRVector3();
            var rayDirection = new TSRVector3();
            var rayEnd = new TSRVector3();
            var viewport = new TSRViewPort
            {
                Width = Convert.ToUInt32(Viewport3D.ViewportSize.Width),
                Height = Convert.ToUInt32(Viewport3D.ViewportSize.Height)
            };

            camera.TransformIntoScreenCoords(viewport, coordVector, out var screenX, out var screenY, out var depth);
            camera.ComputeRay(viewport, (int)screenX, (int)endPoint.Y, rayStart, rayDirection);
            rayEnd.x = rayStart.x + rayDirection.x;
            rayEnd.y = rayStart.y + rayDirection.y;
            rayEnd.z = rayStart.z + rayDirection.z;

            var pointPlane = new TSRPlane(camera.m_Fwd, coordVector);
            var endVector = pointPlane.Split(rayStart, rayEnd);
            return endVector.y;
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
                var yDrag = GetYDrag(e.MousePoint, coordVector);

                float maxY = 500.0f;
                float minY = -500.0f;

                if ((yDrag > minY) && (yDrag < maxY))
                {
                    if (!yDrag.IsNaN())
                    {
                        PointDragDelta?.Invoke(this, new DragPointEventArgs
                        {
                            PointIndex = (int)_entityVertexId.VertexId - 1,
                            YValue = ParentSurface.YAxis.GetDataValue(yDrag)
                        });

                        StartPoint = e.MousePoint;
                    }
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