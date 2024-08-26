using SciChart.Charting.Visuals;
using System.Windows;
using System.Windows.Media;

namespace DpiAware_SciChartSurface
{
    public class DpiAwareSciChartSurface : SciChartSurface
    {
        private readonly ScaleTransform _scaleTransform;

        protected override void OnDpiChanged(DpiScale oldDpi, DpiScale newDpi)
        {
            var xDpiScale = 1d / newDpi.DpiScaleX;
            var yDpiScale = 1d / newDpi.DpiScaleY;

            _scaleTransform.ScaleX = xDpiScale;
            _scaleTransform.ScaleY = yDpiScale;
        }

        public DpiAwareSciChartSurface()
        {
            // Prevents gridlines blurriness when display scaling is > 100%
            SetValue(RenderOptions.BitmapScalingModeProperty, BitmapScalingMode.NearestNeighbor);

            // Scales down Chart when display scaling is > 100%
            _scaleTransform = new ScaleTransform();
            LayoutTransform = _scaleTransform;
        }
    }
}