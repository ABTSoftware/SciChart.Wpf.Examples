using SciChart.Charting.Visuals;
using System;
using System.Windows.Media;

namespace DpiAware_SciChartSurface
{
    public class DpiAwareSciChartSurface : SciChartSurface
    {
        private readonly ScaleTransform _scaleTransform;

        public DpiInfo DpiInfo
        {
            get; private set;
        }

        public DpiAwareSciChartSurface()
        {
            // Prevents gridlines blurriness when display scaling is > 100%
            SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.NearestNeighbor);

            // Scales down Chart when display scaling is > 100% 
            _scaleTransform = new ScaleTransform();
            LayoutTransform = _scaleTransform;

            UpdateScale();
        }

        private void UpdateScale()
        {
            DpiInfo = DpiAwarenessHelper.Instance.GetDpiScale(this);
            var xDpiScale = 1d / DpiInfo.DpiScaleX;
            var yDpiScale = 1d / DpiInfo.DpiScaleY;

            if (Math.Abs(_scaleTransform.ScaleX - xDpiScale) > Double.Epsilon ||
                Math.Abs(_scaleTransform.ScaleY - yDpiScale) > Double.Epsilon)
            {
                _scaleTransform.ScaleX = xDpiScale;
                _scaleTransform.ScaleY = yDpiScale;
            }
        }

        protected override void DoDrawingLoop()
        {
            // On every redraw, check display scaling and adjust
            UpdateScale();

            base.DoDrawingLoop();
        }
    }
}
