using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace Fifo100MillionPointsDemo
{
    public class PointCountViewModel : BindableObject
    {
        public string DisplayName { get; }
        public int SeriesCount { get; }
        public int PointCount { get; }

        public PointCountViewModel(string displayName, int seriesCount, int pointCount)
        {
            DisplayName = displayName;
            SeriesCount = seriesCount;
            PointCount = pointCount;
        }
    }
}