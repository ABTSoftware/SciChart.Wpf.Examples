using SciChart.Charting3D;
using SciChart.Charting3D.Interop;
using SciChart.Charting3D.Modifiers;
using SciChart.Charting3D.Visuals.AxisLabels;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar3D.CustomModifiers
{
    internal class AxisPlaneDrawLabelsModifier : ChartModifierBase3D
    {
        private eAxisPlaneDrawLabelsMode _xyPlaneDrawLabelsMode;
        private eAxisPlaneDrawLabelsMode _zxPlaneDrawLabelsMode;
        private eAxisPlaneDrawLabelsMode _zyPlaneDrawLabelsMode;

        public eAxisPlaneDrawLabelsMode XyPlaneDrawLabelsMode
        {
            get => _xyPlaneDrawLabelsMode;
            set
            {
                _xyPlaneDrawLabelsMode = value;
                OnPropertyChanged(nameof(XyPlaneDrawLabelsMode));
                SetXyPlaneDrawLabelsMode(_xyPlaneDrawLabelsMode);
            }
        }

        public eAxisPlaneDrawLabelsMode ZxPlaneDrawLabelsMode
        {
            get => _zxPlaneDrawLabelsMode;
            set
            {
                _zxPlaneDrawLabelsMode = value;
                OnPropertyChanged(nameof(ZxPlaneDrawLabelsMode));
                SetZxPlaneDrawLabelsMode(_zxPlaneDrawLabelsMode);
            }
        }

        public eAxisPlaneDrawLabelsMode ZyPlaneDrawLabelsMode
        {
            get => _zyPlaneDrawLabelsMode;
            set
            {
                _zyPlaneDrawLabelsMode = value;
                OnPropertyChanged(nameof(ZyPlaneDrawLabelsMode));
                SetZyPlaneDrawLabelsMode(_zyPlaneDrawLabelsMode);
            }
        }

        public override void OnAttached()
        {
            base.OnAttached();

            if (ParentSurface is SciChart3DSurface sciChartSurface)
            {
                XyPlaneDrawLabelsMode = XyAxisPlane.GetDrawLabelsMode(sciChartSurface);
                ZxPlaneDrawLabelsMode = ZxAxisPlane.GetDrawLabelsMode(sciChartSurface);
                ZyPlaneDrawLabelsMode = ZyAxisPlane.GetDrawLabelsMode(sciChartSurface);
            }
        }

        private void SetXyPlaneDrawLabelsMode(eAxisPlaneDrawLabelsMode labelsMode)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                XyAxisPlane.SetDrawLabelsMode(sciChartSurface, labelsMode);
            }
        }

        private void SetZxPlaneDrawLabelsMode(eAxisPlaneDrawLabelsMode labelsMode)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                ZxAxisPlane.SetDrawLabelsMode(sciChartSurface, labelsMode);
            }
        }

        private void SetZyPlaneDrawLabelsMode(eAxisPlaneDrawLabelsMode labelsMode)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                ZyAxisPlane.SetDrawLabelsMode(sciChartSurface, labelsMode);
            }
        }
    }
}