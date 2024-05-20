using SciChart.Charting3D;
using SciChart.Charting3D.Axis;
using SciChart.Charting3D.Modifiers;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar3D.CustomModifiers
{
    internal class AxisLabelsOrientationModifier : ChartModifierBase3D
    {
        private TickLabelOrientation3D _xAxisLabelOrientation;
        private TickLabelOrientation3D _yAxisLabelOrientation;
        private TickLabelOrientation3D _zAxisLabelOrientation;

        public TickLabelOrientation3D XAxisLabelOrientation
        {
            get => _xAxisLabelOrientation;
            set
            {
                _xAxisLabelOrientation = value;
                OnPropertyChanged(nameof(XAxisLabelOrientation));
                SetXAxisLabelOrientation(_xAxisLabelOrientation);
            }
        }

        public TickLabelOrientation3D YAxisLabelOrientation
        {
            get => _yAxisLabelOrientation;
            set
            {
                _yAxisLabelOrientation = value;
                OnPropertyChanged(nameof(YAxisLabelOrientation));
                SetYAxisLabelOrientation(_yAxisLabelOrientation);
            }
        }

        public TickLabelOrientation3D ZAxisLabelOrientation
        {
            get => _zAxisLabelOrientation;
            set
            {
                _zAxisLabelOrientation = value;
                OnPropertyChanged(nameof(ZAxisLabelOrientation));
                SetZAxisLabelOrientation(_zAxisLabelOrientation);
            }
        }

        public override void OnAttached()
        {
            base.OnAttached();

            if (ParentSurface is SciChart3DSurface sciChartSurface)
            {
                if (sciChartSurface.XAxis is AxisBase3D xAxisBase)
                {
                    XAxisLabelOrientation = xAxisBase.TickLabelOrientation;
                }

                if (sciChartSurface.YAxis is AxisBase3D yAxisBase)
                {
                    YAxisLabelOrientation = yAxisBase.TickLabelOrientation;
                }

                if (sciChartSurface.ZAxis is AxisBase3D zAxisBase)
                {
                    ZAxisLabelOrientation = zAxisBase.TickLabelOrientation;
                }
            }
        }

        private void SetXAxisLabelOrientation(TickLabelOrientation3D labelOrientation)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                if (sciChartSurface.XAxis is AxisBase3D axisBase)
                {
                    axisBase.TickLabelOrientation = labelOrientation;
                }
            }
        }

        private void SetYAxisLabelOrientation(TickLabelOrientation3D labelOrientation)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                if (sciChartSurface.YAxis is AxisBase3D axisBase)
                {
                    axisBase.TickLabelOrientation = labelOrientation;
                }
            }
        }

        private void SetZAxisLabelOrientation(TickLabelOrientation3D labelOrientation)
        {
            if (IsAttached && ParentSurface is SciChart3DSurface sciChartSurface)
            {
                if (sciChartSurface.ZAxis is AxisBase3D axisBase)
                {
                    axisBase.TickLabelOrientation = labelOrientation;
                }
            }
        }
    }
}