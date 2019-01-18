// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PlaneGeometry.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Sandbox.Examples.Plane3DAnnotation
{
    public class VerticalPlaneGeometry : BaseSceneEntity
    {
        private readonly double m_startX;
        private readonly double m_startY;
        private readonly double m_startZ;
        private readonly double m_endX;
        private readonly double m_endY;
        private readonly double m_endZ;
        private readonly double m_height;

        private readonly Color m_color;
        private readonly bool m_drawWireframe;


        public VerticalPlaneGeometry(double startX, double startY, double startZ,
            double endX, double endY, double endZ, double height, Color color, bool drawWireframe)
        {
            m_startX = startX;
            m_startY = startY;
            m_startZ = startZ;
            m_endX = endX;
            m_endY = endY;
            m_endZ = endZ;
            m_height = height;

            m_color = color;
            m_drawWireframe = drawWireframe;
        }

        public override void PerformSelection(bool isSelected, List<VertexId> vertexIds)
        {

        }

        /// <summary>
        /// Determines a kind of the entity. If SCRT_SCENE_ENTITY_KIND_TRANSPARENT then the 3D Engine must make some internal adjustments to allow order independent transparency
        /// </summary>
        public override eSCRTSceneEntityKind GetKind()
        {
            return m_color.A == 255 ? eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_OPAQUE : eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_TRANSPARENT;
        }

        public override void UpdateScene(IRenderPassInfo3D e)
        {

        }

        /// <summary>
        ///     Called when the 3D Engine wishes to render this element. This is where geometry must be drawn to the 3D scene
        /// </summary>
        /// <param name="rpi">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void RenderScene(IRenderPassInfo3D rpi)
        {
            float fStartCoordX = (float)rpi.XCalc.GetCoordinate(m_startX) - rpi.WorldDimensions.X / 2.0f;
            float fStartCoordY = (float)rpi.YCalc.GetCoordinate(m_startY);
            float fStartCoordZ = (float)rpi.ZCalc.GetCoordinate(m_startZ) - rpi.WorldDimensions.Z / 2.0f;

            float fEndCoordX = (float)rpi.XCalc.GetCoordinate(m_endX) - rpi.WorldDimensions.X / 2.0f;
            float fEndCoordY = (float)rpi.YCalc.GetCoordinate(m_endY);
            float fEndCoordZ = (float)rpi.ZCalc.GetCoordinate(m_endZ) - rpi.WorldDimensions.Z / 2.0f;

            float fHalfHeightCoord = (float)((rpi.YCalc.GetCoordinate(m_height) - rpi.YCalc.GetCoordinate(0.0)) / 2.0);

            Vector3[] corners = {
                new Vector3(fStartCoordX, fStartCoordY - fHalfHeightCoord, fStartCoordZ),   // bottom start
                new Vector3(fEndCoordX, fEndCoordY - fHalfHeightCoord, fEndCoordZ),         // bottom end
                new Vector3(fStartCoordX, fStartCoordY + fHalfHeightCoord, fStartCoordZ),   // top start
                new Vector3(fEndCoordX, fEndCoordY + fHalfHeightCoord, fEndCoordZ),         // top end
            };

            Plane plane = new Plane(corners[0], corners[1], corners[2]);

            Vector3[] normals = {
                new Vector3(plane.NormalX, plane.NormalY, plane.NormalZ),   // front
                new Vector3(-plane.NormalX, -plane.NormalY, -plane.NormalZ) // back
            };

            // We create a mesh context. There are various mesh render modes. The simplest is Triangles
            // For this mode we have to draw a couple of triangles (three vertices) for each side of the plane
            // You can see 
            using (var meshContext = BeginLitMesh(TSRRenderMode.TRIANGLES))
            {
                // Set the Rasterizer State for this entity 
                SCRTImmediateDraw.PushRasterizerState(RasterizerStates.CullBackFacesState.TSRRasterizerState);

                // Set the color before drawing vertices
                meshContext.SetVertexColor(m_color);

                // Now draw the triangles. Each face of the plane is made up of two triangles
                // Front face
                SetNormal(meshContext, normals[1]);
                SetVertex(meshContext, corners[0]);
                SetVertex(meshContext, corners[2]);
                SetVertex(meshContext, corners[1]);
                SetVertex(meshContext, corners[1]);
                SetVertex(meshContext, corners[2]);
                SetVertex(meshContext, corners[3]);

                // Back face
                SetNormal(meshContext, normals[0]);
                SetVertex(meshContext, corners[0]);
                SetVertex(meshContext, corners[1]);
                SetVertex(meshContext, corners[2]);
                SetVertex(meshContext, corners[1]);
                SetVertex(meshContext, corners[3]);
                SetVertex(meshContext, corners[2]);
            }

            // Revert raster state
            SCRTImmediateDraw.PopRasterizerState();

            if (m_drawWireframe)
            {


                // Set the Rasterizer State for wireframe 
                SCRTImmediateDraw.PushRasterizerState(RasterizerStates.WireframeState.TSRRasterizerState);

                // Create a Line Context for a continuous line and draw the outline of the cube 
                var lineColor = Color.FromArgb(0xFF, m_color.R, m_color.G, m_color.B);

                CreateSquare(2.0f, true, lineColor, new[] { corners[0], corners[1], corners[3], corners[2] });

                // Revert raster state
                SCRTImmediateDraw.PopRasterizerState();
            }
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
    }
}