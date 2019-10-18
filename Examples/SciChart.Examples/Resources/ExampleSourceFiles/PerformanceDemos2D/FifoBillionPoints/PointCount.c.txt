using SciChart.Data.Model;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class PointCount : BindableObject
    {
        public string DisplayName { get; }
        public int SeriesCount { get; }
        public int PointsCount { get; }

        public PointCount(string displayName, int seriesCount, int pointsCount)
        {
            DisplayName = displayName;
            SeriesCount = seriesCount;
            PointsCount = pointsCount;
        }
    }
}