using SciChart.Core.Extensions;
using SciChart.Data.Model;

namespace AspectRatioGridLines
{
    public static class VisibleRangeHelper
    {
        public static IRange GetXRangeByYRange(double height, double width, IRange yRange)
        {
            if (height <= 0 || width <= 0) return null;

            var ratio = width / height;

            var yMin = yRange.Min.ToDouble();
            var yMax = yRange.Max.ToDouble();

            return new DoubleRange(yMin * ratio, yMax * ratio);
        }

        public static IRange GetYRangeByXRange(double height, double width, IRange xRange)
        {
            if (height <= 0 || width <= 0) return null;

            var ratio = height / width;

            var xMin = xRange.Min.ToDouble();
            var xMax = xRange.Max.ToDouble();

            return new DoubleRange(xMin * ratio, xMax * ratio);
        }
    }
}