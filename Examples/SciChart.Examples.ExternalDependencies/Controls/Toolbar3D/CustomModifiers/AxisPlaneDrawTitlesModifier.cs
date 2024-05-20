using SciChart.Charting3D;
using SciChart.Charting3D.Modifiers;
using SciChart.Charting3D.Visuals.Axis;
using SciChart.Charting3D.Visuals.AxisLabels;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar3D.CustomModifiers
{
    internal class AxisPlaneDrawTitlesModifier : ChartModifierBase3D
    {
        private AxisPlaneDrawTitlesMode _xyPlaneDrawTitlesMode;
        private AxisPlaneDrawTitlesMode _zxPlaneDrawTitlesMode;
        private AxisPlaneDrawTitlesMode _zyPlaneDrawTitlesMode;

        public AxisPlaneDrawTitlesMode XyPlaneDrawTitlesMode
        {
            get => _xyPlaneDrawTitlesMode;
            set
            {
                _xyPlaneDrawTitlesMode = value;
                OnPropertyChanged(nameof(XyPlaneDrawTitlesMode));
                SetXyPlaneDrawTitlesMode(_xyPlaneDrawTitlesMode);
            }
        }

        public AxisPlaneDrawTitlesMode ZxPlaneDrawTitlesMode
        {
            get => _zxPlaneDrawTitlesMode;
            set
            {
                _zxPlaneDrawTitlesMode = value;
                OnPropertyChanged(nameof(ZxPlaneDrawTitlesMode));
                SetZxPlaneDrawTitlesMode(_zxPlaneDrawTitlesMode);
            }
        }

        public AxisPlaneDrawTitlesMode ZyPlaneDrawTitlesMode
        {
            get => _zyPlaneDrawTitlesMode;
            set
            {
                _zyPlaneDrawTitlesMode = value;
                OnPropertyChanged(nameof(ZyPlaneDrawTitlesMode));
                SetZyPlaneDrawTitlesMode(_zyPlaneDrawTitlesMode);
            }
        }

        public override void OnAttached()
        {
            base.OnAttached();

            if (ParentSurface is SciChart3DSurface sciChartSurface)
            {
                XyPlaneDrawTitlesMode = XyAxisPlane.GetDrawTitlesMode(sciChartSurface);
                ZxPlaneDrawTitlesMode = ZxAxisPlane.GetDrawTitlesMode(sciChartSurface);
                ZyPlaneDrawTitlesMode = ZyAxisPlane.GetDrawTitlesMode(sciChartSurface);
            }
        }

        private void SetXyPlaneDrawTitlesMode(AxisPlaneDrawTitlesMode titlesMode)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                XyAxisPlane.SetDrawTitlesMode(sciChartSurface, titlesMode);
            }
        }

        private void SetZxPlaneDrawTitlesMode(AxisPlaneDrawTitlesMode titlesMode)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                ZxAxisPlane.SetDrawTitlesMode(sciChartSurface, titlesMode);
            }
        }

        private void SetZyPlaneDrawTitlesMode(AxisPlaneDrawTitlesMode titlesMode)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                ZyAxisPlane.SetDrawTitlesMode(sciChartSurface, titlesMode);
            }
        }
    }
}