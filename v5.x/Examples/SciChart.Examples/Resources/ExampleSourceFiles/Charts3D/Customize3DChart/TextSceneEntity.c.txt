// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// TextSceneEntity.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Windows.Media;
using SciChart.Charting3D;
using SciChart.Charting3D.Primitives;
using SciChart.Charting3D.Visuals.Primitives;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart
{
    public enum TextDisplayMode
    {
        Default,
        FacingCameraAlways,
    }

    /// <summary>
    /// A class to demonstrate a 3D Text Elements added to the SciChart3D Scene. Created using our BaseSceneEntity and Font3D APIs
    /// </summary>
    public class TextSceneEntity : BaseSceneEntity
    {
        private readonly string _text;
        private readonly Color _textColor;
        private readonly Vector3 _location;
        private readonly TextDisplayMode _textDisplayMode;
        Font3D _font;

        static TextSceneEntity()
        {
            SCRTDllLoader.InitNativeLibs();
        }

        public TextSceneEntity(string text, Color textColor, Vector3 location, TextDisplayMode textDisplayMode = TextDisplayMode.FacingCameraAlways, int fontSize = 8, string fontFamily = "Arial")
        {
            _text = text;
            _textColor = textColor;
            _location = location;
            _textDisplayMode = textDisplayMode;
            _font = new Font3D(fontFamily, (uint) fontSize);

            // Set requires SeletionId to false to exclude this item from selection, tooltips and also 
            // prevent issues with maximum number of selectable meshes 
            this.RequiresSelectionId = false;
        }

        /// <summary>
        /// Determines whether this instance is transparent. When True SciChart internally
        /// </summary>
        /// <returns></returns>
        public override bool IsTransparent()
        {
            return _textColor.A < 255;
        }

        /// <summary>
        /// Called when the 3D Engine wishes to render this element. This is where geometry must be drawn to the 3D scene
        /// </summary>
        /// <param name="e">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void RenderScene(IRenderPassInfo3D e)
        {
            if (_font == null) return;

            var currentCamera = RootSceneEntity != null ? RootSceneEntity.Viewport3D.CameraController : null;
            if (_textDisplayMode == TextDisplayMode.Default || currentCamera == null)
            {
                // Just display text
                _font.Begin();
                _font.AddText(_text, _textColor, _location.X, _location.Y, _location.Z);
                _font.End();
                return;
            }
            else if (_textDisplayMode == TextDisplayMode.FacingCameraAlways)
            {
                // Display billboarded text using camera vectors                
                Vector3 cameraFwd = currentCamera.Forward;
                Vector3 cameraUp = currentCamera.Up;                

                // Compute a side vector
                Vector3 cameraSide = cameraFwd ^ cameraUp;
                cameraSide.Normalize();

                // Compute orthogonal up vector
                cameraUp = cameraSide ^ cameraFwd;
                cameraUp.Normalize();

                // Display text billboarded using camera side, up vectors (text will always face camera) 
                _font.BeginBillboard(cameraSide, cameraUp);
                _font.AddText(_text, _textColor, _location.X, _location.Y, _location.Z);
                _font.End();
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public override void Dispose()
        {
            _font.SafeDispose();
            _font = null;
            base.Dispose();
        }

        /// <summary>
        /// Called when the 3D Engine wishes to update the geometry in this element. This is where we need to cache geometry before draw.
        /// </summary>
        /// <param name="e">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void UpdateScene(IRenderPassInfo3D e)
        {
        }


        /// <summary>
        /// Performs selection on this entity, setting the IsSelected flag to True or False on the specified <see cref="VertexId">Vertex Ids</see>
        /// </summary>
        /// <param name="isSelected">if set to <c>true</c> the vertices become .</param>
        /// <param name="vertexIds">The vertex ids.</param>
        public override void PerformSelection(bool isSelected, List<VertexId> vertexIds)
        {
        }
    }
}