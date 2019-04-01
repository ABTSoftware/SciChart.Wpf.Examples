using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Primitives;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace SciChart.Sandbox.Examples.MouseDragModifier3D
{
    class BoxAnnotation3D : BaseSceneEntity
    {
        // private members
        private Vector3 _bottomRight;
        private Vector3 _topLeft;
        private Vector3 _center;
        Vector3[] _corners;
        Vector3[] _normals;

        private Vector3 _dragVector = null;
        private int _dragFaceId = -1;

        SciChart3DSurface _currentSurface;

        public DoubleRange RangeX
        {
            get => _rangeX;
            set
            {
                if (value == _rangeX) return;

                _rangeX = value;
                Refresh();
            }
        }
        private DoubleRange _rangeX;

        public DoubleRange RangeY
        {
            get => _rangeY;
            set
            {
                if (value == _rangeY) return;

                _rangeY = value;
                Refresh();
            }
        }
        private DoubleRange _rangeY;

        public DoubleRange RangeZ
        {
            get => _rangeZ;
            set
            {
                if (value == _rangeZ) return;

                _rangeZ = value;
                Refresh();
            }
        }
        private DoubleRange _rangeZ;

        public Color Color
        {
            get => _color;
            set
            {
                if (value == _color) return;

                _color = value;
                Refresh();
            }
        }
        private Color _color = Color.FromArgb(150, 255, 255, 255);

        public Color DragColor
        {
            get => _dragColor;
            set
            {
                if (value == _dragColor) return;

                _dragColor = value;
                Refresh();
            }
        }
        private Color _dragColor = Colors.White;

        public Color Stroke
        {
            get => _stroke;
            set
            {
                if (value == _stroke) return;

                _stroke = value;
                Refresh();
            }
        }
        private Color _stroke = Colors.White;

        public double StrokeWidth
        {
            get => _strokeWidth;
            set
            {
                if (value == _strokeWidth) return;

                _strokeWidth = value;
                Refresh();
            }
        }
        private double _strokeWidth = 2;

        public bool DoubleSided
        {
            get => _doubleSided;
            set
            {
                if (value == _doubleSided) return;

                _doubleSided = value;
                Refresh();
            }
        }
        private bool _doubleSided = false;

        public bool DragX { get; set; } = true;

        public bool DragY { get; set; } = true;

        public bool DragZ { get; set; } = true;

        ///<summary>
        /// Determines a kind of the entity. If SCRT_SCENE_ENTITY_KIND_TRANSPARENT then the 3D Engine must make some internal adjustments to allow order independent transparency
        ///</summary>
        public override eSCRTSceneEntityKind GetKind()
        {
            return _color.A == 255
                ? eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_OPAQUE
                : eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_TRANSPARENT;
        }

        // redo the transform calculations
        // and re-render the view
        void Refresh()
        {
            if (_currentSurface != null)
            {
                // TODO find a better way to refresh
                using (_currentSurface.SuspendUpdates())
                {

                }
            }
        }


        ///<summary>
        ///     Called when the 3D Engine wishes to update the geometry in this element. This is where we need to cache geometry
        ///     before draw.
        ///</summary>
        ///<param name="e">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void UpdateScene(IRenderPassInfo3D e)
        {

        }

        TSRVector3 GetWorldCenter()
        {
            TSRVector3 worldCenter = new TSRVector3(
                0.5f * (float)(this.RootSceneEntity.Viewport3D.ParentSurface.WorldDimensions.x),
                0f * (float)(this.RootSceneEntity.Viewport3D.ParentSurface.WorldDimensions.y),
                0.5f * (float)(this.RootSceneEntity.Viewport3D.ParentSurface.WorldDimensions.z));

            return worldCenter;
        }

        /// <summary>
        /// Converts topLeft and bottomRight to RangeX, RangeY, and RangeZ
        /// Used when some adjustment is made in topLeft/bottomRight, and that needs updating in range properties
        /// </summary>
        void InverseTransform()
        {
            var xCoordinateCalculator =
                this.RootSceneEntity.Viewport3D.ParentSurface.XAxis.GetCurrentCoordinateCalculator();
            var yCoordinateCalculator =
                this.RootSceneEntity.Viewport3D.ParentSurface.YAxis.GetCurrentCoordinateCalculator();
            var zCoordinateCalculator =
                this.RootSceneEntity.Viewport3D.ParentSurface.ZAxis.GetCurrentCoordinateCalculator();

            var worldCenter = GetWorldCenter();

            var rangeXMin = xCoordinateCalculator.GetDataValue(_topLeft.x + worldCenter.x);
            var rangeXMax = xCoordinateCalculator.GetDataValue(_bottomRight.x + worldCenter.x);

            RangeX = new DoubleRange(rangeXMin, rangeXMax);

            var rangeYMin = yCoordinateCalculator.GetDataValue(_topLeft.y + worldCenter.y);
            var rangeYMax = yCoordinateCalculator.GetDataValue(_bottomRight.y + worldCenter.y);

            RangeY = new DoubleRange(rangeYMin, rangeYMax);

            var rangeZMin = zCoordinateCalculator.GetDataValue(_topLeft.z + worldCenter.z);
            var rangeZMax = zCoordinateCalculator.GetDataValue(_bottomRight.z + worldCenter.z);

            RangeZ = new DoubleRange(rangeZMin, rangeZMax);

            worldCenter.Dispose();
        }

        /// <summary>
        /// Converts RangeX, RangeY, and RangeZ into topLeft and bottomRight
        /// For rendering the box at a position
        /// </summary>
        void Transform()
        {
            var xCoordinateCalculator =
                this.RootSceneEntity.Viewport3D.ParentSurface.XAxis.GetCurrentCoordinateCalculator();
            var yCoordinateCalculator =
                this.RootSceneEntity.Viewport3D.ParentSurface.YAxis.GetCurrentCoordinateCalculator();
            var zCoordinateCalculator =
                this.RootSceneEntity.Viewport3D.ParentSurface.ZAxis.GetCurrentCoordinateCalculator();

            var worldStartX = xCoordinateCalculator.GetCoordinate(_rangeX.Min);
            var worlEndX = xCoordinateCalculator.GetCoordinate(_rangeX.Max);

            var worldStartY = yCoordinateCalculator.GetCoordinate(_rangeY.Min);
            var worlEndY = yCoordinateCalculator.GetCoordinate(_rangeY.Max);

            var worldStartZ = zCoordinateCalculator.GetCoordinate(_rangeZ.Min);
            var worlEndZ = zCoordinateCalculator.GetCoordinate(_rangeZ.Max);

            var worldCenter = GetWorldCenter();

            _topLeft = new Vector3((float)worldStartX - worldCenter.x, (float)worldStartY - worldCenter.y,
                (float)worldStartZ - worldCenter.z);
            _bottomRight = new Vector3((float)worlEndX - worldCenter.x, (float)worlEndY - worldCenter.y,
                (float)worlEndZ - worldCenter.z);


            using (TSRVector3 centerPosition = new TSRVector3(
                0.5f * (_topLeft.X + _bottomRight.X),
                0.5f * (_topLeft.Y + _bottomRight.Y),
                0.5f * (_topLeft.Z + _bottomRight.Z)))
            {
                SetPosition(centerPosition);
                _center = new Vector3(centerPosition.x, centerPosition.y, centerPosition.z);
            }

            worldCenter.Dispose();
        }

        ///<summary>
        ///     Called when the 3D Engine wishes to render this element. This is where geometry must be drawn to the 3D scene
        ///</summary>
        ///<param name="e">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void RenderScene(IRenderPassInfo3D e)
        {
            if (_rangeX == null || _rangeY == null || _rangeZ == null || RootSceneEntity == null)
                return;

            Transform();

            // y          1--------0
            // |         /|       /|
            // |       5--------4  |
            // |       |  |     |  |
            // |       |  |     |  |
            // |       |  2--------3
            // |  z    | /      |/    
            // | /     6--------7        
            // |/
            // ----------- X
            _corners = new[]
            {
                new Vector3(_topLeft.X, _topLeft.Y, _topLeft.Z), //0
                new Vector3(_bottomRight.X, _topLeft.Y, _topLeft.Z), //1
                new Vector3(_bottomRight.X, _bottomRight.Y, _topLeft.Z), //2
                new Vector3(_topLeft.X, _bottomRight.Y, _topLeft.Z), //3
                new Vector3(_topLeft.X, _topLeft.Y, _bottomRight.Z), //4
                new Vector3(_bottomRight.X, _topLeft.Y, _bottomRight.Z), //5
                new Vector3(_bottomRight.X, _bottomRight.Y, _bottomRight.Z), //6
                new Vector3(_topLeft.X, _bottomRight.Y, _bottomRight.Z), //7
            };

            _normals = new[]
            {
                new Vector3(+0.0f, +0.0f, -1.0f), //front
                new Vector3(+0.0f, +0.0f, +1.0f), //back
                new Vector3(+1.0f, +0.0f, +0.0f), //right
                new Vector3(-1.0f, +0.0f, +0.0f), //left
                new Vector3(+0.0f, +1.0f, +0.0f), //top
                new Vector3(+0.0f, -1.0f, +0.0f), //bottom
            };

            // Pass Entity ID
            ulong selectionColor = SCRTImmediateDraw.EncodeSelectionId(EntityId, 0);
            SCRTImmediateDraw.SelectionColor(selectionColor);

            // We create a mesh context. There are various mesh render modes. The simplest is Triangles
            // For this mode we have to draw a single triangle (three vertices) for each corner of the cube
            // You can see 
            using (var meshContext = base.BeginLitMesh(TSRRenderMode.TRIANGLES))
            {
                // Set the Rasterizer State for this entity 
                if (_doubleSided)
                    SCRTImmediateDraw.PushRasterizerState(RasterizerStates.Default.TSRRasterizerState);
                else
                    SCRTImmediateDraw.PushRasterizerState(RasterizerStates.CullBackFacesState.TSRRasterizerState);

                meshContext.SetVertexColor((_dragFaceId == 0 && DragZ) ? _dragColor : _color);

                // Front face
                SetNormal(meshContext, _normals[0]);
                meshContext.SetSelectionId(SCRTImmediateDraw.EncodeSelectionId(EntityId, 0));
                SetVertex(meshContext, _corners[0]);
                SetVertex(meshContext, _corners[2]);
                SetVertex(meshContext, _corners[1]);
                SetVertex(meshContext, _corners[2]);
                SetVertex(meshContext, _corners[0]);
                SetVertex(meshContext, _corners[3]);

                meshContext.SetVertexColor((_dragFaceId == 1 && DragX) ? _dragColor : _color);

                // Right side face
                SetNormal(meshContext, _normals[2]);
                meshContext.SetSelectionId(SCRTImmediateDraw.EncodeSelectionId(EntityId, 1));
                SetVertex(meshContext, _corners[1]);
                SetVertex(meshContext, _corners[2]);
                SetVertex(meshContext, _corners[6]);
                SetVertex(meshContext, _corners[1]);
                SetVertex(meshContext, _corners[6]);
                SetVertex(meshContext, _corners[5]);

                meshContext.SetVertexColor((_dragFaceId == 2 && DragY) ? _dragColor : _color);

                // Top face
                SetNormal(meshContext, _normals[4]);
                meshContext.SetSelectionId(SCRTImmediateDraw.EncodeSelectionId(EntityId, 2));
                SetVertex(meshContext, _corners[2]);
                SetVertex(meshContext, _corners[7]);
                SetVertex(meshContext, _corners[6]);
                SetVertex(meshContext, _corners[7]);
                SetVertex(meshContext, _corners[2]);
                SetVertex(meshContext, _corners[3]);

                meshContext.SetVertexColor((_dragFaceId == 3 && DragX) ? _dragColor : _color);

                // Left side face
                SetNormal(meshContext, _normals[3]);
                meshContext.SetSelectionId(SCRTImmediateDraw.EncodeSelectionId(EntityId, 3));
                SetVertex(meshContext, _corners[3]);
                SetVertex(meshContext, _corners[0]);
                SetVertex(meshContext, _corners[4]);
                SetVertex(meshContext, _corners[3]);
                SetVertex(meshContext, _corners[4]);
                SetVertex(meshContext, _corners[7]);

                meshContext.SetVertexColor((_dragFaceId == 4 && DragZ) ? _dragColor : _color);

                // Back face
                SetNormal(meshContext, _normals[1]);
                meshContext.SetSelectionId(SCRTImmediateDraw.EncodeSelectionId(EntityId, 4));
                SetVertex(meshContext, _corners[7]);
                SetVertex(meshContext, _corners[5]);
                SetVertex(meshContext, _corners[6]);
                SetVertex(meshContext, _corners[7]);
                SetVertex(meshContext, _corners[4]);
                SetVertex(meshContext, _corners[5]);

                meshContext.SetVertexColor((_dragFaceId == 5 && DragY) ? _dragColor : _color);

                // Bottom face 
                SetNormal(meshContext, _normals[5]);
                meshContext.SetSelectionId(SCRTImmediateDraw.EncodeSelectionId(EntityId, 5));
                SetVertex(meshContext, _corners[0]);
                SetVertex(meshContext, _corners[1]);
                SetVertex(meshContext, _corners[5]);
                SetVertex(meshContext, _corners[0]);
                SetVertex(meshContext, _corners[5]);
                SetVertex(meshContext, _corners[4]);
            }

            // Revert raster state
            SCRTImmediateDraw.PopRasterizerState();

            // Set the Rasterizer State for wireframe 
            SCRTImmediateDraw.PushRasterizerState(RasterizerStates.WireframeState.TSRRasterizerState);
            var strokeWidth = (float)this._strokeWidth;

            // Create a Line Context for a continuous line and draw the outline of the cube 
            CreateSquare(strokeWidth, true, _stroke, new[] { _corners[0], _corners[1], _corners[2], _corners[3] });
            CreateSquare(strokeWidth, true, _stroke, new[] { _corners[4], _corners[5], _corners[6], _corners[7] });
            CreateSquare(strokeWidth, true, _stroke, new[] { _corners[0], _corners[4], _corners[7], _corners[3] });
            CreateSquare(strokeWidth, true, _stroke, new[] { _corners[5], _corners[1], _corners[2], _corners[6] });

            // Revert raster state
            SCRTImmediateDraw.PopRasterizerState();
        }

        /// <summary>
        /// Creates a wireframe square along the vertices
        /// </summary>
        /// <param name="lineThickness"></param>
        /// <param name="isAntiAlias"></param>
        /// <param name="lineColor"></param>
        /// <param name="vertices"></param>
        private void CreateSquare(float lineThickness, bool isAntiAlias, Color lineColor, Vector3[] vertices)
        {
            using (var lineContext = base.BeginLineStrips(lineThickness, isAntiAlias))
            {
                lineContext.SetVertexColor(lineColor);

                // Set selection id in order for the hit-test to be supported
                ulong selectionId = SCRTImmediateDraw.EncodeSelectionId(EntityId, 0);
                lineContext.SetSelectionId(selectionId);

                for (var i = 0; i < vertices.Length; i++)
                {
                    SetVertex(lineContext, vertices[i]);
                }

                SetVertex(lineContext, vertices.First());
                lineContext.Freeze();
                lineContext.Draw();
            }
        }

        private void SetVertex(IImmediateLitMeshContext meshContext, Vector3 vector3)
        {
            meshContext.SetVertex3(vector3.X, vector3.Y, vector3.Z);
        }

        private void SetVertex(ILinesMesh linesContext, Vector3 vector3)
        {
            linesContext.SetVertex3(vector3.X, vector3.Y, vector3.Z);
        }

        private void SetNormal(IImmediateLitMeshContext meshContext, Vector3 vector3)
        {
            meshContext.Normal3(vector3.X, vector3.Y, vector3.Z);
        }

        ///<summary>
        ///     Performs selection on this entity, setting the IsSelected flag to True or False on the specified
        ///<see cref="VertexId">Vertex Ids</see>
        ///</summary>
        ///<param name="isSelected">if set to <c>true</c> the vertices become .</param>
        ///<param name="vertexIds">The vertex ids.</param>
        public override void PerformSelection(bool isSelected, List<VertexId> vertexIds)
        {
            // Do nothing
        }

        public override void OnAttached()
        {
            base.OnAttached();
            _currentSurface = this.RootSceneEntity.Viewport3D.ParentSurface as SciChart3DSurface;
            _currentSurface.PreviewMouseDown -= Surface_PreviewMouseDown;
            _currentSurface.PreviewMouseDown += Surface_PreviewMouseDown;
            _currentSurface.PreviewMouseMove -= CurrentSurface_PreviewMouseMove;
            _currentSurface.PreviewMouseMove += CurrentSurface_PreviewMouseMove;
            _currentSurface.PreviewMouseUp -= CurrentSurface_PreviewMouseUp;
            _currentSurface.PreviewMouseUp += CurrentSurface_PreviewMouseUp;

            Transform();
        }

        public override void OnDetached()
        {
            base.OnDetached();
            _currentSurface.PreviewMouseDown -= Surface_PreviewMouseDown;
            _currentSurface.PreviewMouseMove -= CurrentSurface_PreviewMouseMove;
            _currentSurface.PreviewMouseUp -= CurrentSurface_PreviewMouseUp;
        }

        /// <summary>
        /// Gets the center of the face
        /// </summary>
        /// <param name="faceId"></param>
        /// <returns></returns>
        Vector3 GetFaceCenter(int faceId)
        {
            Vector3 sum = null;

            if (faceId == 0)
                sum = _corners[0] + _corners[2] + _corners[1] + _corners[3];
            else if (faceId == 1)
                sum = _corners[1] + _corners[2] + _corners[6] + _corners[5];
            else if (faceId == 2)
                sum = _corners[2] + _corners[7] + _corners[6] + _corners[3];
            else if (faceId == 3)
                sum = _corners[3] + _corners[0] + _corners[4] + _corners[7];
            else if (faceId == 4)
                sum = _corners[7] + _corners[5] + _corners[6] + _corners[4];
            else if (faceId == 5)
                sum = _corners[0] + _corners[1] + _corners[5] + _corners[4];

            return sum * 0.25;
        }

        /// <summary>
        /// Offsets a face by a certain Vector delta
        /// </summary>
        /// <param name="faceId"></param>
        /// <param name="delta"></param>
        void MoveFace(int faceId, Vector3 delta)
        {
            // TODO make sure that the face doesn't go absurdly away

            if (faceId == 0 && DragZ)
                _topLeft.z += delta.z;
            else if (faceId == 1 && DragX)
                _bottomRight.x += delta.x;
            else if (faceId == 2 && DragY)
                _bottomRight.y += delta.y;
            else if (faceId == 3 && DragX)
                _topLeft.x += delta.x;
            else if (faceId == 4 && DragZ)
                _bottomRight.z += delta.z;
            else if (faceId == 5 && DragY)
                _topLeft.y += delta.y;

            InverseTransform();
        }

        /// <summary>
        /// Converts 3D point to a 2D point
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        Point Project(Vector3 vector)
        {
            var hitPoint2D = _currentSurface.Camera.WorldToScreenSpace(vector);
            hitPoint2D.Y = _currentSurface.ActualHeight - hitPoint2D.Y;

            return hitPoint2D;
        }

        /// <summary>
        /// Basic linear interpolation function
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="f"></param>
        /// <returns></returns>
        static float Lerp(float a, float b, float f)
        {
            return a + f * (b - a);
        }

        /// <summary>
        /// Linear interpolation between vectors
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="f"></param>
        static Vector3 Lerp(Vector3 a, Vector3 b, float f)
        {
            return new Vector3(Lerp(a.x, b.x, f), Lerp(a.y, b.y, f), Lerp(a.z, b.z, f));
        }

        static double VectorDotProduct(Vector v1, Vector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        /// <summary>
        /// Finds the normalized distance between two points, depending on the closest third point
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="pnt"></param>
        /// <returns></returns>
        static double LineInterval(Point start, Point end, Point pnt)
        {
            var line = (end - start);
            var len = line.Length;
            line.Normalize();

            var v = pnt - start;
            var d = VectorDotProduct(v, line);
            return d / len;
        }

        /// <summary>
        /// Gets the (faceCenter+delta) point, where delta depends on the mousePosition
        /// </summary>
        /// <param name="faceCenter"></param>
        /// <param name="normal"></param>
        /// <param name="mousePosition"></param>
        /// <returns></returns>
        Vector3 GetRelativePoint(Vector3 faceCenter, Vector3 normal, Point mousePosition)
        {
            // an imaginary line from center of box to the face center
            var pointStart = _center;
            var pointEnd = faceCenter;

            // Important: when center and FaceCenter become same (box is thin)
            // The function may give unpredictable results
            // TODO replace pointStart and pointEnd with normal based imaginary points

            // we convert that to pixels
            var pixelStart = Project(pointStart);
            var pixelEnd = Project(pointEnd);

            // find the normalized distance based on three pixel points (center, faceCenter, mousePosition)
            var interval = LineInterval(pixelStart, pixelEnd, mousePosition);

            // convert that to a 3D point, using the interval and lerp ;)
            var point3D = Lerp(pointStart, pointEnd, (float)interval);

            return point3D;
        }

        private void Surface_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(_currentSurface);
            HitTestInfo3D hitTestInfo = _currentSurface.RenderableSeries.First().HitTest(mousePosition);

            if (hitTestInfo.EntityId == EntityId)
            {
                _dragFaceId = (int)hitTestInfo.VertexId;
                var faceCenter = GetFaceCenter(_dragFaceId);
                _dragVector = GetRelativePoint(faceCenter, _normals[_dragFaceId], mousePosition);

                e.Handled = true;
                Refresh();
            }
        }

        private void CurrentSurface_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var mousePosition = e.GetPosition(_currentSurface);
            if (_dragVector != null)
            {
                var faceCenter = GetFaceCenter(_dragFaceId);
                var newDragVector = GetRelativePoint(faceCenter, _normals[_dragFaceId], mousePosition);
                var delta = newDragVector - _dragVector;

                MoveFace(_dragFaceId, delta);

                _dragVector = newDragVector;
                e.Handled = true;
            }
        }

        private void CurrentSurface_PreviewMouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (_dragVector != null)
            {
                _dragVector = null;
                _dragFaceId = -1;
                Refresh();
                e.Handled = true;
            }
        }
    }
}