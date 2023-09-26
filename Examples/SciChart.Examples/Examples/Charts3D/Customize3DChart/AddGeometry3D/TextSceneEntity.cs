// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
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
using System;
using System.ComponentModel;
using System.Windows.Automation.Provider;
using System.Windows.Media;
using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Primitives;
using SciChart.Charting3D.Visuals.Primitives;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.Charts3D.Customize3DChart.AddGeometry3D
{
    public enum TextDisplayMode
    {
        Default,
        FacingCameraAlways,
        ScreenSpace
    }

    /// <summary>
    /// A class to demonstrate a 3D Text Elements added to the SciChart3D Scene. Created using our BaseSceneEntity and Font3D APIs
    /// </summary>
    public class TextSceneEntity : BaseSceneEntity<SCRTSceneEntity>
    {
        private readonly Color _textColor;
        private readonly int _fontSize;
        private readonly string _fontFamily;
        private Font3D _font;

        public string Text { get; set; }

        [TypeConverter(typeof(StringToVector3TypeConverter))]
        public Vector3 Location { get; set; }

        public double RotationAngle { get; set; }

        static TextSceneEntity()
        {
            SCRTDllLoader.InitNativeLibs();
        }

        /// <summary>
        /// Creates a representation of a Text Label in the 3D space, defined in world coordinates
        /// </summary>
        /// <param name="location">Point in the 3D space that determines the top-left corner of a label</param>
        public TextSceneEntity(string text, Color textColor, Vector3 location,
            int fontSize = 8, string fontFamily = "Arial")
            : base(new SCRTSceneEntity())
        {
            Text = text;
            _textColor = textColor;
            Location = location;
            _fontSize = fontSize;
            _fontFamily = fontFamily;

            // Set requires SelectionId to False to exclude this item from selection, tooltips and also 
            // prevent issues with maximum number of selectable meshes 
            RequiresSelectionId = false;
        }

        /// <summary>
        /// Called when the 3D Engine wishes to render this element. This is where geometry must be drawn to the 3D scene
        /// </summary>
        /// <param name="rpi">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void RenderScene(IRenderPassInfo3D rpi)
        {
            var currentCamera = RootSceneEntity?.Viewport3D.CameraController;
            if (currentCamera == null || Location == null) return;

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
            _font.AddText(Text, _textColor, Location.X, Location.Y, Location.Z);
            _font.End();
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeFont();
            }

            base.Dispose(disposing);
        }

        private void DisposeFont()
        {
            _font.SafeDispose();
            _font = null;
        }

        /// <summary>
        /// Called when the 3D Engine wishes to update the geometry in this element. This is where we need to cache geometry before draw.
        /// </summary>
        /// <param name="rpi">The <see cref="IRenderPassInfo3D" /> containing parameters for the current render pass.</param>
        public override void UpdateScene(IRenderPassInfo3D rpi)
        {
            if (_font == null)
            {
                _font = new Font3D(_fontFamily, (uint)_fontSize);
            }
        }

        /// <summary>
        /// Called when the D3DEngine Restarts. Meshes and DirectX related objects should be recreated
        /// </summary>
        public override void OnEngineRestart()
        {
            DisposeFont();

            base.OnEngineRestart();
        }

        public override eSCRTSceneEntityKind GetKind()
        {
            return eSCRTSceneEntityKind.SCRT_SCENE_ENTITY_KIND_HUD3D;
        }
    }
}