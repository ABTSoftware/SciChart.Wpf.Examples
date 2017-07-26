using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Primitives;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart
{
    /// <summary>
    /// A class which allows loading of Wavefront *.obj files into the 3D Scene
    /// </summary>
    public class ObjSceneEntity : BaseSceneEntity
    {
        private readonly SCRTModelSceneEntity _innerObject;

        private Vector3 _position = new Vector3(0,0,0);
        private Vector3 _scale = new Vector3(1, 1, 1);
        private readonly bool _isSemiTransparent;
        private bool _drawBoundingBox = true;
        private Color _boundingBoxStroke = Colors.LimeGreen;
        private float _boundingBoxStrokeThickness = 1.0f;

        static ObjSceneEntity()
        {
            SCRTDllLoader.InitNativeLibs();
        }

        public ObjSceneEntity(string objectName, byte[] objectData, bool isSemiTransparent = false)
        {
            _isSemiTransparent = isSemiTransparent;

            // Given an *.obj file loaded into a byte array
            // Create a SCRTModelSceneEntity and load the model
            _innerObject = new SCRTModelSceneEntity();
            _innerObject.LoadModel(objectName, objectData, objectData.Length);

            this.SetBounds(_innerObject.GetBounds());

            // Add the model to the scene. This will draw it 
            this.AddChildEntityInternal(_innerObject);

            // Note: ObjSceneEntity must also be added to SciChart3DSurface.Viewport3D.RootEntity.Children
        }

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position.SafeDispose();
                _position = value;
                _innerObject.SetPosition(_position);
                base.InvalidateScene();
            }
        }

        public Vector3 Scale
        {
            get { return _scale; }
            set
            {
                _scale.SafeDispose();
                _scale = value;
                _innerObject.SetScale(_scale);
                base.InvalidateScene();
            }
        }

        public bool DrawBoundingBox
        {
            get { return _drawBoundingBox; }
            set
            {
                _drawBoundingBox = value;
                InvalidateScene();
            }
        }

        public Color BoundingBoxStroke
        {
            get { return _boundingBoxStroke; }
            set
            {
                _boundingBoxStroke = value;
                InvalidateScene();
            }
        }

        public float BoundingBoxStrokeThickness
        {
            get { return _boundingBoxStrokeThickness; }
            set
            {
                _boundingBoxStrokeThickness = value;
                InvalidateScene();
            }
        }

        public override bool IsTransparent()
        {
            return _isSemiTransparent;
        }

        public override void UpdateScene(IRenderPassInfo3D e)
        {
            // Do nothing. Internal engine takes care of rendering
        }

        public override void RenderScene(IRenderPassInfo3D e)
        {
            // Do nothing. Internal engine takes care of rendering


            // Draw the bounding box
            if (_drawBoundingBox == false) return;

            using (var bounds = this.GetBounds())
            {
                var topLeft = bounds.m_vMax;
                var bottomRight = bounds.m_vMin;

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
                Vector3[] corners =
                {
                    new Vector3(topLeft.x, topLeft.y, topLeft.z), //0
                    new Vector3(bottomRight.x, topLeft.y, topLeft.z), //1
                    new Vector3(bottomRight.x, bottomRight.y, topLeft.z), //2
                    new Vector3(topLeft.x, bottomRight.y, topLeft.z), //3
                    new Vector3(topLeft.x, topLeft.y, bottomRight.z), //4
                    new Vector3(bottomRight.x, topLeft.y, bottomRight.z), //5
                    new Vector3(bottomRight.x, bottomRight.y, bottomRight.z), //6
                    new Vector3(topLeft.x, bottomRight.y, bottomRight.z), //7
                };

                CreateSquare(_boundingBoxStrokeThickness, true, _boundingBoxStroke, new[] { corners[0], corners[1], corners[2], corners[3] });
                CreateSquare(_boundingBoxStrokeThickness, true, _boundingBoxStroke, new[] { corners[4], corners[5], corners[6], corners[7] });
                CreateSquare(_boundingBoxStrokeThickness, true, _boundingBoxStroke, new[] { corners[0], corners[4], corners[7], corners[3] });
                CreateSquare(_boundingBoxStrokeThickness, true, _boundingBoxStroke, new[] { corners[5], corners[1], corners[2], corners[6] });
            }            
        }

        public override void PerformSelection(bool isSelected, List<VertexId> vertexIds)
        {
        }

        public override void Dispose()
        {
            base.Dispose();

            _position.SafeDispose();
            _innerObject.SafeDispose();
        }

        private void CreateSquare(float lineThickness, bool isAntiAlias, Color lineColor, Vector3[] vertices)
        {
            using (var lineContext = base.BeginLineStrips(lineThickness, isAntiAlias))
            {
                lineContext.SetVertexColor(lineColor);

                foreach (var v in vertices)
                {
                    SetVertex(lineContext, v);
                }
                SetVertex(lineContext, vertices.First());
                lineContext.Freeze();
                lineContext.Draw();
            }
        }

        private void SetVertex(ILinesMesh linesContext, Vector3 vector3)
        {
            linesContext.SetVertex3(vector3.X, vector3.Y, vector3.Z);
        }
    }
}