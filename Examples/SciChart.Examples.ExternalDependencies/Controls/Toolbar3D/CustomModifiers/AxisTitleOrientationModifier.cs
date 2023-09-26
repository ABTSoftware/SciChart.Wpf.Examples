using SciChart.Charting3D;
using SciChart.Charting3D.Axis;
using SciChart.Charting3D.Modifiers;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar3D.CustomModifiers
{
    internal class AxisTitleOrientationModifier : ChartModifierBase3D
    {
        private AxisTitleOrientation3D _xAxisTitleOrientation;
        private AxisTitleOrientation3D _yAxisTitleOrientation;
        private AxisTitleOrientation3D _zAxisTitleOrientation;

        public AxisTitleOrientation3D XAxisTitleOrientation
        {
            get => _xAxisTitleOrientation;
            set
            {
                _xAxisTitleOrientation = value;
                OnPropertyChanged(nameof(XAxisTitleOrientation));
                SetXAxisTitleOrientation(_xAxisTitleOrientation);
            }
        }

        public AxisTitleOrientation3D YAxisTitleOrientation
        {
            get => _yAxisTitleOrientation;
            set
            {
                _yAxisTitleOrientation = value;
                OnPropertyChanged(nameof(YAxisTitleOrientation));
                SetYAxisTitleOrientation(_yAxisTitleOrientation);
            }
        }

        public AxisTitleOrientation3D ZAxisTitleOrientation
        {
            get => _zAxisTitleOrientation;
            set
            {
                _zAxisTitleOrientation = value;
                OnPropertyChanged(nameof(ZAxisTitleOrientation));
                SetZAxisTitleOrientation(_zAxisTitleOrientation);
            }
        }

        public override void OnAttached()
        {
            base.OnAttached();

            if (ParentSurface is SciChart3DSurface sciChartSurface)
            {
                if (sciChartSurface.XAxis is AxisBase3D xAxisBase)
                {
                    XAxisTitleOrientation = xAxisBase.AxisTitleOrientation;
                }

                if (sciChartSurface.YAxis is AxisBase3D yAxisBase)
                {
                    YAxisTitleOrientation = yAxisBase.AxisTitleOrientation;
                }

                if (sciChartSurface.ZAxis is AxisBase3D zAxisBase)
                {
                    ZAxisTitleOrientation = zAxisBase.AxisTitleOrientation;
                }
            }
        }

        private void SetXAxisTitleOrientation(AxisTitleOrientation3D titleOrientation)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                if (sciChartSurface.XAxis is AxisBase3D axisBase)
                {
                    axisBase.AxisTitleOrientation = titleOrientation;
                }
            }
        }

        private void SetYAxisTitleOrientation(AxisTitleOrientation3D titleOrientation)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                if (sciChartSurface.YAxis is AxisBase3D axisBase)
                {
                    axisBase.AxisTitleOrientation = titleOrientation;
                }
            }
        }

        private void SetZAxisTitleOrientation(AxisTitleOrientation3D titleOrientation)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                if (sciChartSurface.ZAxis is AxisBase3D axisBase)
                {
                    axisBase.AxisTitleOrientation = titleOrientation;
                }
            }
        }
    }
}
