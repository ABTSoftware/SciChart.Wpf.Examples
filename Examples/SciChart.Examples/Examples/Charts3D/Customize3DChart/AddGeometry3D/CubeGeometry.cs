// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CubeGeometry.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Linq;
using System.Windows.Media;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Primitives;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart.AddGeometry3D
{
    /// <summary>
    /// A class to demonstrate a 3D Geometry added to the SciChart3D Scene. Created using our BaseSceneEntity and Mesh APIs
    /// </summary>
    public class CubeGeometry : BaseSceneEntity<SCRTSceneEntity>
    {
        private readonly Vector3 _bottomRight;
        private readonly Vector3 _topLeft;
        private readonly Color _cubeColor;

        /// <summary>
        /// Creates a representation of a Cube in the 3D space, defined in world coordinates
        /// </summary>
        /// <param name="topLeft">Point in the 3D space that determines the top-left corner of a cube</param>
        /// <param name="bottomRight">Point in the 3D space that determines the bottom-right corner of a cube</param>
        /// <param name="cubeColor">Color of the cube surface</param>
        public CubeGeometry(Vector3 topLeft, Vector3 bottomRight, Color cubeColor) : base(new SCRTSceneEntity())
        {
            // Setting the position of scene entities will be used back when sorting them from camera perspective back to front
            using (TSRVector3 centerPosition = new TSRVector3(
                    0.5f*(topLeft.x + bottomRight.x),
                    0.5f*(topLeft.y + bottomRight.y), 
                    0.5f*(topLeft.z + bottomRight.z)))
            {
                SetPosition(centerPosition);
            }                        

            this._topLeft = topLeft;
            this._bottomRight = bottomRight;
            this._cubeColor = cubeColor;
        }

        /// <summary>
        /// Determines a kind of the entity. If SCRT_SCENE_ENTITY_KIND_TRANSPARENT then the 3D Engine must make some internal adjustments to allow order independent transparency
        /// </summary>
        public override eSCRTSceneEntityKind GetKind()
        {
            return _cubeColor.A == 255 ? eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_OPAQUE : eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_TRANSPARENT;
        }

        /// <summary>
        /// Called when the 3D Engine wishes to render this element. This is where geometry must be drawn to the 3D scene
        /// </summary>
        /// <param name="rpi">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void RenderScene(IRenderPassInfo3D rpi)
        {
            float bottomRightCoordX = _bottomRight.X;
            float bottomRightCoordY = _bottomRight.Y;
            float bottomRightCoordZ = _bottomRight.Z;
            float topLeftCoordX = _topLeft.X;
            float topLeftCoordY = _topLeft.Y;
            float topLeftCoordZ = _topLeft.Z;

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
            Vector3[] corners = {
                new Vector3(topLeftCoordX, topLeftCoordY, topLeftCoordZ), //0
                new Vector3(bottomRightCoordX, topLeftCoordY, topLeftCoordZ), //1
                new Vector3(bottomRightCoordX, bottomRightCoordY, topLeftCoordZ), //2
                new Vector3(topLeftCoordX, bottomRightCoordY, topLeftCoordZ), //3
                new Vector3(topLeftCoordX, topLeftCoordY, bottomRightCoordZ), //4
                new Vector3(bottomRightCoordX, topLeftCoordY, bottomRightCoordZ), //5
                new Vector3(bottomRightCoordX, bottomRightCoordY, bottomRightCoordZ), //6
                new Vector3(topLeftCoordX, bottomRightCoordY, bottomRightCoordZ), //7
            };

            Vector3[] normals = {
                new Vector3(+0.0f, +0.0f, -1.0f), //front
                new Vector3(+0.0f, +0.0f, +1.0f), //back
                new Vector3(+1.0f, +0.0f, +0.0f), //right
                new Vector3(-1.0f, +0.0f, +0.0f), //left
                new Vector3(+0.0f, +1.0f, +0.0f), //top
                new Vector3(+0.0f, -1.0f, +0.0f), //bottom
            };

            eSCRTUpAxis upaxis = VXccelEngine3D.GetUpAxis();
            // We create a mesh context. There are various mesh render modes. The simplest is Triangles
            // For this mode we have to draw a single triangle (three vertices) for each corner of the cube
            using (var meshContext = BeginLitMesh(TSRRenderMode.TRIANGLES))
            {
                // Set the Rasterizer State for this entity 
                if (upaxis == eSCRTUpAxis.Z_UP)
                {
                    VXccelEngine3D.PushRasterizerState(RasterizerStates.Default.TSRRasterizerState);
                }
                else
                {
                    VXccelEngine3D.PushRasterizerState(RasterizerStates.CullBackFacesState.TSRRasterizerState);
                }
                
                // Set the color before drawing vertices
                meshContext.SetVertexColor(_cubeColor);
            
                // Pass Entity ID value for a hit test purpose
                ulong selectionColor = VXccelEngine3D.EncodeSelectionId(EntityId, 0);
                meshContext.SetSelectionId(selectionColor);

                // Now draw the triangles. Each face of the cube is made up of two triangles
                // Front face
                SetNormal(meshContext, normals[0]);
                SetVertex(meshContext, corners[0]);
                SetVertex(meshContext, corners[2]);
                SetVertex(meshContext, corners[1]);
                SetVertex(meshContext, corners[2]);
                SetVertex(meshContext, corners[0]);
                SetVertex(meshContext, corners[3]);
            
                // Right side face
                SetNormal(meshContext, normals[2]);
                SetVertex(meshContext, corners[1]);
                SetVertex(meshContext, corners[2]);
                SetVertex(meshContext, corners[6]);
                SetVertex(meshContext, corners[1]);
                SetVertex(meshContext, corners[6]);
                SetVertex(meshContext, corners[5]);
            
                // Top face
                SetNormal(meshContext, normals[4]);
                SetVertex(meshContext, corners[2]);
                SetVertex(meshContext, corners[7]);
                SetVertex(meshContext, corners[6]);
                SetVertex(meshContext, corners[7]);
                SetVertex(meshContext, corners[2]);
                SetVertex(meshContext, corners[3]);
            
                // Left side face
                SetNormal(meshContext, normals[3]);
                SetVertex(meshContext, corners[3]);
                SetVertex(meshContext, corners[0]);
                SetVertex(meshContext, corners[4]);
                SetVertex(meshContext, corners[3]);
                SetVertex(meshContext, corners[4]);
                SetVertex(meshContext, corners[7]);
            
                // Back face
                SetNormal(meshContext, normals[1]);
                SetVertex(meshContext, corners[7]);
                SetVertex(meshContext, corners[5]);
                SetVertex(meshContext, corners[6]);
                SetVertex(meshContext, corners[7]);
                SetVertex(meshContext, corners[4]);
                SetVertex(meshContext, corners[5]);
            
                // Bottom face 
                SetNormal(meshContext, normals[5]);
                SetVertex(meshContext, corners[0]);
                SetVertex(meshContext, corners[1]);
                SetVertex(meshContext, corners[5]);
                SetVertex(meshContext, corners[0]);
                SetVertex(meshContext, corners[5]);
                SetVertex(meshContext, corners[4]);
            }

            // Revert raster state
            VXccelEngine3D.PopRasterizerState();

            // Set the Rasterizer State for the cube mesh wireframe 
            VXccelEngine3D.PushRasterizerState(RasterizerStates.WireframeState.TSRRasterizerState);

            // Create a Line Context for a continuous line and draw the outline of the cube 
            var lineColor = Color.FromArgb(0xFF, _cubeColor.R, _cubeColor.G, _cubeColor.B);

            CreateSquare(2.0f, true, lineColor, new[] { corners[0], corners[1], corners[2], corners[3] });
            CreateSquare(2.0f, true, lineColor, new[] { corners[4], corners[5], corners[6], corners[7] });
            CreateSquare(2.0f, true, lineColor, new[] { corners[0], corners[4], corners[7], corners[3] });
            CreateSquare(2.0f, true, lineColor, new[] { corners[5], corners[1], corners[2], corners[6] });

            // Revert raster state
            VXccelEngine3D.PopRasterizerState();
        }

        private void CreateSquare(float lineThickness, bool isAntiAlias, Color lineColor, Vector3[] vertices)
        {
            using (var lineContext = BeginLineStrips(lineThickness, isAntiAlias))
            {
                lineContext.SetVertexColor(lineColor);

                // Pass Entity ID value for a hit test purpose
                ulong selectionColor = VXccelEngine3D.EncodeSelectionId(EntityId, 0);
                lineContext.SetSelectionId(selectionColor);

                foreach (var v in vertices)
                {
                    SetVertex(lineContext, v);
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
    }
}