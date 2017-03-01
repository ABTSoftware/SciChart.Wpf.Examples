// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
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
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Primitives;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart
{
    /// <summary>
    /// A class to demonstrate a 3D Geometry added to the SciChart3D Scene. Created using our BaseSceneEntity and Mesh APIs
    /// </summary>
    public class CubeGeometry : BaseSceneEntity
    {
        private readonly Vector3 bottomRight;
        private readonly Vector3 topLeft;

        private readonly Color cubeColor;


        public CubeGeometry(Vector3 topLeft, Vector3 bottomRight, Color cubeColor)
        {
            // Shady : Setting the position of scene entities will be used back when sorting them from camera perspective back to front
            using (TSRVector3 centerPosition = new TSRVector3(
                    0.5f*(topLeft.x + bottomRight.x),
                    0.5f*(topLeft.y + bottomRight.y), 
                    0.5f*(topLeft.z + bottomRight.z)))
            {
                SetPosition(centerPosition);
            }                        

            this.topLeft = topLeft;
            this.bottomRight = bottomRight;
            this.cubeColor = cubeColor;
        }

        /// <summary>
        /// Determines whether this instance is transparent. If TRUE then the 3D Engine must make some internal adjustments to allow order independent transparency
        /// </summary>
        public override bool IsTransparent()
        {
            return cubeColor.A != 255;
        }

        /// <summary>
        ///     Called when the 3D Engine wishes to update the geometry in this element. This is where we need to cache geometry
        ///     before draw.
        /// </summary>
        /// <param name="e">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void UpdateScene(IRenderPassInfo3D e)
        {
        }

        /// <summary>
        ///     Called when the 3D Engine wishes to render this element. This is where geometry must be drawn to the 3D scene
        /// </summary>
        /// <param name="e">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void RenderScene(IRenderPassInfo3D e)
        {
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
                new Vector3(topLeft.X, topLeft.Y, topLeft.Z), //0
                new Vector3(bottomRight.X, topLeft.Y, topLeft.Z), //1
                new Vector3(bottomRight.X, bottomRight.Y, topLeft.Z), //2
                new Vector3(topLeft.X, bottomRight.Y, topLeft.Z), //3
                new Vector3(topLeft.X, topLeft.Y, bottomRight.Z), //4
                new Vector3(bottomRight.X, topLeft.Y, bottomRight.Z), //5
                new Vector3(bottomRight.X, bottomRight.Y, bottomRight.Z), //6
                new Vector3(topLeft.X, bottomRight.Y, bottomRight.Z), //7
            };

            Vector3[] normals = {
                new Vector3(+0.0f, +0.0f, -1.0f), //front
                new Vector3(+0.0f, +0.0f, +1.0f), //back
                new Vector3(+1.0f, +0.0f, +0.0f), //right
                new Vector3(-1.0f, +0.0f, +0.0f), //left
                new Vector3(+0.0f, +1.0f, +0.0f), //top
                new Vector3(+0.0f, -1.0f, +0.0f), //bottom
            };

            // We create a mesh context. There are various mesh render modes. The simplest is Triangles
            // For this mode we have to draw a single triangle (three vertices) for each corner of the cube
            // You can see 
            using (var meshContext = base.BeginLitMesh(TSRRenderMode.TRIANGLES))
            {
                // Set the Rasterizer State for this entity 
                SCRTImmediateDraw.PushRasterizerState(RasterizerStates.CullBackFacesState.TSRRasterizerState);
            
                // Set the color before drawing vertices
                meshContext.SetVertexColor(cubeColor);
            
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
            SCRTImmediateDraw.PopRasterizerState();

            // Create a Line Context for a continuous line and draw the outline of the cube 
            var lineColor = Color.FromArgb(0xFF, cubeColor.R, cubeColor.G, cubeColor.B);

            CreateSquare(2.0f, true, lineColor, new[] { corners[0], corners[1], corners[2], corners[3] });
            CreateSquare(2.0f, true, lineColor, new[] { corners[4], corners[5], corners[6], corners[7] });
            CreateSquare(2.0f, true, lineColor, new[] { corners[0], corners[4], corners[7], corners[3] });
            CreateSquare(2.0f, true, lineColor, new[] { corners[5], corners[1], corners[2], corners[6] });
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

        /// <summary>
        ///     Performs selection on this entity, setting the IsSelected flag to True or False on the specified
        ///     <see cref="VertexId">Vertex Ids</see>
        /// </summary>
        /// <param name="isSelected">if set to <c>true</c> the vertices become .</param>
        /// <param name="vertexIds">The vertex ids.</param>
        public override void PerformSelection(bool isSelected, List<VertexId> vertexIds)
        {
            // Do nothing
        }
    }
}