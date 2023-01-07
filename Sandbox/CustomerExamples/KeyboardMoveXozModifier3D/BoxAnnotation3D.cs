using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Primitives;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Data.Model;

namespace KeyboardMoveXozModifier3DExample
{
	class BoxAnnotation3D : BaseSceneEntity<SCRTSceneEntity>, INotifyPropertyChanged
	{
		// private members
		private Vector3 _bottomRight;
		private Vector3 _topLeft;
		private Vector3 _center;
		Vector3[] _corners;
		Vector3[] _normals;

		private Vector3 _dragVector = null;
		private int _dragFaceId = -1;

		SciChart3DSurface currentSurface;

		// public properties
		// Uses INotifyPropertyChanged interface for binding

		private DoubleRange rangeX;
		public DoubleRange RangeX
		{
			get => rangeX;
			set
			{
				if (value == rangeX) return;

				rangeX = value;
				NotifyPropertyChanged(nameof(RangeX));
				refresh();
			}
		}

		private DoubleRange rangeY;
		public DoubleRange RangeY
		{
			get => rangeY;
			set
			{
				if (value == rangeY) return;

				rangeY = value;
				NotifyPropertyChanged(nameof(RangeY));
				refresh();
			}
		}

		private DoubleRange rangeZ;
		public DoubleRange RangeZ
		{
			get => rangeZ;
			set
			{
				if (value == rangeZ) return;

				rangeZ = value;
				NotifyPropertyChanged(nameof(RangeZ));
				refresh();
			}
		}

		private Color color = Color.FromArgb(150, 255, 255, 255);
		public Color Color
		{
			get => color;
			set
			{
				if (value == color) return;

				color = value;
				NotifyPropertyChanged(nameof(Color));
				refresh();
			}
		}

		private Color dragColor = Colors.White;
		public Color DragColor
		{
			get => dragColor;
			set
			{
				if (value == dragColor) return;

				dragColor = value;
				NotifyPropertyChanged(nameof(DragColor));
				refresh();
			}
		}

		private Color stroke = Colors.White;
		public Color Stroke
		{
			get => stroke;
			set
			{
				if (value == stroke) return;

				stroke = value;
				NotifyPropertyChanged(nameof(Stroke));
				refresh();
			}
		}

		private double strokeWidth = 2;
		public double StrokeWidth
		{
			get => strokeWidth;
			set
			{
				if (value == strokeWidth) return;

				strokeWidth = value;
				NotifyPropertyChanged(nameof(StrokeWidth));
				refresh();
			}
		}

		private bool doubleSided = false;
		public bool DoubleSided
		{
			get => doubleSided;
			set
			{
				if (value == doubleSided) return;

				doubleSided = value;
				NotifyPropertyChanged(nameof(DoubleSided));
				refresh();
			}
		}

		private bool dragX = true;
		public bool DragX
		{
			get => dragX;
			set
			{
				dragX = value;
				NotifyPropertyChanged(nameof(DragX));
			}
		}

		private bool dragY = true;
		public bool DragY
		{
			get => dragY;
			set
			{
				dragY = value;
				NotifyPropertyChanged(nameof(DragY));
			}
		}

		private bool dragZ = true;
		public bool DragZ
		{
			get => dragZ;
			set
			{
				dragZ = value;
				NotifyPropertyChanged(nameof(DragZ));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void NotifyPropertyChanged(String PropertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(PropertyName));
		}

		public BoxAnnotation3D() : base(new SCRTSceneEntity())
		{

		}

		///<summary>
		/// Determines a kind of the entity. If SCRT_SCENE_ENTITY_KIND_TRANSPARENT then the 3D Engine must make some internal adjustments to allow order independent transparency
		///</summary>
		public override eSCRTSceneEntityKind GetKind()
		{
			return color.A == 255 ? eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_OPAQUE : eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_TRANSPARENT;
		}

		// redo the transform calculations
		// and re-render the view
		void refresh()
		{
			if (currentSurface != null)
			{
				// TODO find a better way to refresh
				using(currentSurface.SuspendUpdates())
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

		TSRVector3 getWorldCenter()
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
		void inverseTransform()
		{
			var xCoordinateCalculator = this.RootSceneEntity.Viewport3D.ParentSurface.XAxis.GetCurrentCoordinateCalculator();
			var yCoordinateCalculator = this.RootSceneEntity.Viewport3D.ParentSurface.YAxis.GetCurrentCoordinateCalculator();
			var zCoordinateCalculator = this.RootSceneEntity.Viewport3D.ParentSurface.ZAxis.GetCurrentCoordinateCalculator();

			var worldCenter = getWorldCenter();

			var rangeXStart = xCoordinateCalculator.GetDataValue(_topLeft.x + worldCenter.x);
			var rangeXEnd = xCoordinateCalculator.GetDataValue(_bottomRight.x + worldCenter.x);

			RangeX = new DoubleRange(Math.Min(rangeXStart, rangeXEnd), Math.Max(rangeXStart, rangeXEnd));

			var rangeYStart = yCoordinateCalculator.GetDataValue(_topLeft.y + worldCenter.y);
			var rangeYEnd = yCoordinateCalculator.GetDataValue(_bottomRight.y + worldCenter.y);

			RangeY = new DoubleRange(Math.Min(rangeYStart, rangeYEnd), Math.Max(rangeYStart, rangeYEnd));

			var rangeZStart = zCoordinateCalculator.GetDataValue(_topLeft.z + worldCenter.z);
			var rangeZEnd = zCoordinateCalculator.GetDataValue(_bottomRight.z + worldCenter.z);

			RangeZ = new DoubleRange(Math.Min(rangeZStart, rangeZEnd), Math.Max(rangeZStart, rangeZEnd));

			worldCenter.Dispose();
		}

		/// <summary>
		/// Converts RangeX, RangeY, and RangeZ into topLeft and bottomRight
		/// For rendering the box at a position
		/// </summary>
		void transform()
		{
			var xCoordinateCalculator = this.RootSceneEntity.Viewport3D.ParentSurface.XAxis.GetCurrentCoordinateCalculator();
			var yCoordinateCalculator = this.RootSceneEntity.Viewport3D.ParentSurface.YAxis.GetCurrentCoordinateCalculator();
			var zCoordinateCalculator = this.RootSceneEntity.Viewport3D.ParentSurface.ZAxis.GetCurrentCoordinateCalculator();

			var worldStartX = xCoordinateCalculator.GetCoordinate(rangeX.Min);
			var worlEndX = xCoordinateCalculator.GetCoordinate(rangeX.Max);

			var worldStartY = yCoordinateCalculator.GetCoordinate(rangeY.Min);
			var worlEndY = yCoordinateCalculator.GetCoordinate(rangeY.Max);

			var worldStartZ = zCoordinateCalculator.GetCoordinate(rangeZ.Min);
			var worlEndZ = zCoordinateCalculator.GetCoordinate(rangeZ.Max);

			var worldCenter = getWorldCenter();

			_topLeft = new Vector3((float)worldStartX - worldCenter.x, (float)worldStartY - worldCenter.y, (float)worldStartZ - worldCenter.z);
			_bottomRight = new Vector3((float)worlEndX - worldCenter.x, (float)worlEndY - worldCenter.y, (float)worlEndZ - worldCenter.z);
			

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
			if (rangeX == null || rangeY == null || rangeZ == null || RootSceneEntity == null)
				return;

			transform();

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
			_corners = new[] {
				new Vector3(_topLeft.X, _topLeft.Y, _topLeft.Z), //0
				new Vector3(_bottomRight.X, _topLeft.Y, _topLeft.Z), //1
				new Vector3(_bottomRight.X, _bottomRight.Y, _topLeft.Z), //2
				new Vector3(_topLeft.X, _bottomRight.Y, _topLeft.Z), //3
				new Vector3(_topLeft.X, _topLeft.Y, _bottomRight.Z), //4
				new Vector3(_bottomRight.X, _topLeft.Y, _bottomRight.Z), //5
				new Vector3(_bottomRight.X, _bottomRight.Y, _bottomRight.Z), //6
				new Vector3(_topLeft.X, _bottomRight.Y, _bottomRight.Z), //7
			};

			_normals = new[] {
				new Vector3(+0.0f, +0.0f, -1.0f), //front
				new Vector3(+1.0f, +0.0f, +0.0f), //right
				new Vector3(+0.0f, +1.0f, +0.0f), //top
				new Vector3(-1.0f, +0.0f, +0.0f), //left
				new Vector3(+0.0f, +0.0f, +1.0f), //back
				new Vector3(+0.0f, -1.0f, +0.0f), //bottom
			};

			// Pass Entity ID
			ulong selectionColor = VXccelEngine3D.EncodeSelectionId(EntityId, 0);
            VXccelEngine3D.SelectionColor(selectionColor);

			// We create a mesh context. There are various mesh render modes. The simplest is Triangles
			// For this mode we have to draw a single triangle (three vertices) for each corner of the cube
			// You can see 
			using (var meshContext = base.BeginLitMesh(TSRRenderMode.TRIANGLES))
			{
				// Set the Rasterizer State for this entity 
				if (doubleSided)
                    VXccelEngine3D.PushRasterizerState(RasterizerStates.Default.TSRRasterizerState);
				else
                    VXccelEngine3D.PushRasterizerState(RasterizerStates.CullBackFacesState.TSRRasterizerState);
				
				meshContext.SetVertexColor((_dragFaceId == 0 && DragZ) ? dragColor : color);
				
				// Front face
				SetNormal(meshContext, _normals[0]);
				meshContext.SetSelectionId(VXccelEngine3D.EncodeSelectionId(EntityId, 0));
				SetVertex(meshContext, _corners[0]);
				SetVertex(meshContext, _corners[2]);
				SetVertex(meshContext, _corners[1]);
				SetVertex(meshContext, _corners[2]);
				SetVertex(meshContext, _corners[0]);
				SetVertex(meshContext, _corners[3]);

				meshContext.SetVertexColor((_dragFaceId == 1 && DragX) ? dragColor : color);

				// Right side face
				SetNormal(meshContext, _normals[1]);
				meshContext.SetSelectionId(VXccelEngine3D.EncodeSelectionId(EntityId, 1));
				SetVertex(meshContext, _corners[1]);
				SetVertex(meshContext, _corners[2]);
				SetVertex(meshContext, _corners[6]);
				SetVertex(meshContext, _corners[1]);
				SetVertex(meshContext, _corners[6]);
				SetVertex(meshContext, _corners[5]);

				meshContext.SetVertexColor((_dragFaceId == 2 && DragY) ? dragColor : color);

				// Top face
				SetNormal(meshContext, _normals[2]);
				meshContext.SetSelectionId(VXccelEngine3D.EncodeSelectionId(EntityId, 2));
				SetVertex(meshContext, _corners[2]);
				SetVertex(meshContext, _corners[7]);
				SetVertex(meshContext, _corners[6]);
				SetVertex(meshContext, _corners[7]);
				SetVertex(meshContext, _corners[2]);
				SetVertex(meshContext, _corners[3]);

				meshContext.SetVertexColor((_dragFaceId == 3 && DragX) ? dragColor : color);

				// Left side face
				SetNormal(meshContext, _normals[3]);
				meshContext.SetSelectionId(VXccelEngine3D.EncodeSelectionId(EntityId, 3));
				SetVertex(meshContext, _corners[3]);
				SetVertex(meshContext, _corners[0]);
				SetVertex(meshContext, _corners[4]);
				SetVertex(meshContext, _corners[3]);
				SetVertex(meshContext, _corners[4]);
				SetVertex(meshContext, _corners[7]);

				meshContext.SetVertexColor((_dragFaceId == 4 && DragZ) ? dragColor : color);

				// Back face
				SetNormal(meshContext, _normals[4]);
				meshContext.SetSelectionId(VXccelEngine3D.EncodeSelectionId(EntityId, 4));
				SetVertex(meshContext, _corners[7]);
				SetVertex(meshContext, _corners[5]);
				SetVertex(meshContext, _corners[6]);
				SetVertex(meshContext, _corners[7]);
				SetVertex(meshContext, _corners[4]);
				SetVertex(meshContext, _corners[5]);

				meshContext.SetVertexColor((_dragFaceId == 5 && DragY) ? dragColor : color);

				// Bottom face 
				SetNormal(meshContext, _normals[5]);
				meshContext.SetSelectionId(VXccelEngine3D.EncodeSelectionId(EntityId, 5));
				SetVertex(meshContext, _corners[0]);
				SetVertex(meshContext, _corners[1]);
				SetVertex(meshContext, _corners[5]);
				SetVertex(meshContext, _corners[0]);
				SetVertex(meshContext, _corners[5]);
				SetVertex(meshContext, _corners[4]);
			}

			// Revert raster state
            VXccelEngine3D.PopRasterizerState();

			// Set the Rasterizer State for wireframe 
            VXccelEngine3D.PushRasterizerState(RasterizerStates.WireframeState.TSRRasterizerState);
			var strokeWidth = (float)this.strokeWidth;

			// Create a Line Context for a continuous line and draw the outline of the cube 
			CreateSquare(strokeWidth, true, stroke, new[] { _corners[0], _corners[1], _corners[2], _corners[3] });
			CreateSquare(strokeWidth, true, stroke, new[] { _corners[4], _corners[5], _corners[6], _corners[7] });
			CreateSquare(strokeWidth, true, stroke, new[] { _corners[0], _corners[4], _corners[7], _corners[3] });
			CreateSquare(strokeWidth, true, stroke, new[] { _corners[5], _corners[1], _corners[2], _corners[6] });

			// Revert raster state
            VXccelEngine3D.PopRasterizerState();
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
				ulong selectionId = VXccelEngine3D.EncodeSelectionId(EntityId, 0);
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
			currentSurface = this.RootSceneEntity.Viewport3D.ParentSurface as SciChart3DSurface;
			currentSurface.PreviewMouseDown -= Surface_PreviewMouseDown;
			currentSurface.PreviewMouseDown += Surface_PreviewMouseDown;
			currentSurface.PreviewMouseMove -= CurrentSurface_PreviewMouseMove;
			currentSurface.PreviewMouseMove += CurrentSurface_PreviewMouseMove;
			currentSurface.PreviewMouseUp -= CurrentSurface_PreviewMouseUp;
			currentSurface.PreviewMouseUp += CurrentSurface_PreviewMouseUp;

			transform();
		}

		public override void OnDetached()
		{
			base.OnDetached();
			currentSurface.PreviewMouseDown -= Surface_PreviewMouseDown;
			currentSurface.PreviewMouseMove -= CurrentSurface_PreviewMouseMove;
			currentSurface.PreviewMouseUp -= CurrentSurface_PreviewMouseUp;
		}

		/// <summary>
		/// Gets the center of the face
		/// </summary>
		/// <param name="faceId"></param>
		/// <returns></returns>
		Vector3 getFaceCenter(int faceId)
		{
			Vector3 sum = null;

			if (faceId == 0)
				sum = _corners[0] + _corners[2] + _corners[1] + _corners[3];
			else if(faceId == 1)
				sum = _corners[1] + _corners[2] + _corners[6] + _corners[5];
			else if (faceId == 2)
				sum = _corners[2] + _corners[7] + _corners[6] + _corners[3];
			else if(faceId == 3)
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
		void moveFace(int faceId, Vector3 delta)
		{
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

			inverseTransform();
		}

		/// <summary>
		/// Gets the faceId of face opposite to the given face
		/// </summary>
		/// <param name="faceId"></param>
		int oppositeFace(int faceId)
		{
			if (faceId == 0)
				return 4;
			else if (faceId == 1)
				return 3;
			else if (faceId == 2)
				return 5;
			else if (faceId == 4)
				return 0;
			else if (faceId == 3)
				return 1;
			else if (faceId == 5)
				return 2;

			return 0;
		}

		/// <summary>
		/// Converts 3D point to a 2D point
		/// </summary>
		/// <param name="vector"></param>
		/// <returns></returns>
		Point project(Vector3 vector)
		{
			var hitPoint2D = currentSurface.Camera.WorldToScreenSpace(vector);
			hitPoint2D.Y = currentSurface.ActualHeight - hitPoint2D.Y;

			return hitPoint2D;
		}

		/// <summary>
		/// Basic linear interpolation function
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="f"></param>
		/// <returns></returns>
		static float lerp(float a, float b, float f)
		{
			return a + f * (b - a);
		}

		/// <summary>
		/// Linear interpolation between vectors
		/// </summary>
		/// <param name="a"></param>
		/// <param name="b"></param>
		/// <param name="f"></param>
		static Vector3 lerp(Vector3 a, Vector3 b, float f)
		{
			return new Vector3(lerp(a.x, b.x, f), lerp(a.y, b.y, f), lerp(a.z, b.z, f));
		}
		
		static double vectorDotProduct(Vector v1, Vector v2)
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
		static double lineInterval(Point start, Point end, Point pnt)
		{
			var line = (end - start);
			var len = line.Length;
			line.Normalize();

			var v = pnt - start;
			var d = vectorDotProduct(v, line);
			return d / len;
		}

		/// <summary>
		/// Gets the (faceCenter+delta) point, where delta depends on the mousePosition
		/// </summary>
		/// <param name="faceCenter"></param>
		/// <param name="normal"></param>
		/// <param name="mousePosition"></param>
		/// <returns></returns>
		Vector3 getRelativePoint(Vector3 faceCenter, Vector3 normal, Point mousePosition)
		{
			// an imaginary line that crosses the faceCenter +-100 length
			var pointStart = faceCenter - normal * 100;
			var pointEnd = faceCenter + normal * 100;

			// we convert that to pixels
			var pixelStart = project(pointStart);
			var pixelEnd = project(pointEnd);

			// avoid stretching to infinity while dragging a face, facing cam
			// downside, we won't be able to drag when zoomed out far away
			if ((pixelStart - pixelEnd).Length < 40)
				return faceCenter;

			// find the normalized distance based on three pixel points (center, faceCenter, mousePosition)
			var interval = lineInterval(pixelStart, pixelEnd, mousePosition);

			// convert that to a 3D point, using the interval and lerp ;)
			var point3D = lerp(pointStart, pointEnd, (float)interval);

			return point3D;
		}

		private void Surface_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			var mousePosition = e.GetPosition(currentSurface);
			HitTestInfo3D hitTestInfo = currentSurface.RenderableSeries.First().HitTest(mousePosition);
			
			if (hitTestInfo.EntityId == EntityId)
			{
				_dragFaceId = (int)hitTestInfo.VertexId;
				var faceCenter = getFaceCenter(_dragFaceId);
				_dragVector = getRelativePoint(faceCenter, _normals[_dragFaceId], mousePosition);
				
				e.Handled = true;
				currentSurface.CaptureMouse();
				refresh();
			}
		}

		private void CurrentSurface_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			var mousePosition = e.GetPosition(currentSurface);
			if (_dragVector != null)
			{
				var faceCenter = getFaceCenter(_dragFaceId);
				var newDragVector = getRelativePoint(faceCenter, _normals[_dragFaceId], mousePosition);
				var delta = newDragVector - _dragVector;

				moveFace(_dragFaceId, delta);

				if (Keyboard.Modifiers == ModifierKeys.Control)
					moveFace(oppositeFace(_dragFaceId), delta * -1);
				else if (Keyboard.Modifiers == ModifierKeys.Shift)
					moveFace(oppositeFace(_dragFaceId), delta);

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
				e.Handled = true;
				currentSurface.ReleaseMouseCapture();
				refresh();
			}
		}
	}
}