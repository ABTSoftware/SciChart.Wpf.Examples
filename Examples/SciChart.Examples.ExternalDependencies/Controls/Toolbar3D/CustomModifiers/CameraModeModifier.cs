// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CameraModeModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting3D;
using SciChart.Charting3D.Modifiers;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar3D.CustomModifiers
{
    internal class CameraModeModifier : ChartModifierBase3D
    {
        private CameraProjectionMode _selectedCameraMode;

        public CameraProjectionMode SelectedCameraMode
        {
            get => _selectedCameraMode;
            set
            {
                if (_selectedCameraMode != value)
                {
                    _selectedCameraMode = value;
                    OnPropertyChanged(nameof(SelectedCameraMode));
                    SetCameraMode(_selectedCameraMode);
                }
            }
        }

        private void SetCameraMode(CameraProjectionMode cameraMode)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                if (cameraMode == CameraProjectionMode.Perspective)
                {
                    sciChartSurface.Camera = new Camera3D
                    {
                        ZoomToFitOnAttach = true,
                        ProjectionMode = CameraProjectionMode.Perspective
                    };
                }
                else if (cameraMode == CameraProjectionMode.Orthogonal)
                {
                    sciChartSurface.Camera = new Camera3D
                    {
                        ZoomToFitOnAttach = true,
                        ProjectionMode = CameraProjectionMode.Orthogonal
                    };
                }
            }
        }
    }
}
