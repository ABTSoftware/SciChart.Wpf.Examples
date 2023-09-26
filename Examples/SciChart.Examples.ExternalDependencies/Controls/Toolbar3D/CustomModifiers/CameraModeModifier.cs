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
