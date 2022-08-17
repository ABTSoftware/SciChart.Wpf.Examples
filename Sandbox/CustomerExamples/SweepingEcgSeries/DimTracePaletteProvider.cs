using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace SweepingEcgExample
{
    public class DimTracePaletteProvider : IStrokePaletteProvider
    {
        private readonly byte _defaultAlpha;
        private readonly byte _diffAlpha;

        private readonly int _pointsCount;
        private int _startIndex;

        public Values<Color> Colors { get; } = new Values<Color>();

        public DimTracePaletteProvider(int pointsCount, byte defaultAlpha = 25)
        {
            _defaultAlpha = defaultAlpha;
            _diffAlpha = (byte)(255 - defaultAlpha);
            _pointsCount = pointsCount;
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            var xyzSeries = (XyzDataSeries<double, double, double>)rSeries.DataSeries;
            var totalIndex = (int)xyzSeries.Tag;
            var defaultColor = rSeries.Stroke;

            if (totalIndex >= 0 && totalIndex < _pointsCount)
                _startIndex = 0;
           
            if (totalIndex > _startIndex + _pointsCount)
                _startIndex += _pointsCount;
            
            Colors.Count = xyzSeries.Count;

            for (int i = 0, j = 0; i < xyzSeries.Count; i++)
            {
                var color = defaultColor;

                if (xyzSeries.ZValues[i] <= _startIndex)
                {
                    color.A = (byte)(_defaultAlpha + _diffAlpha * j / xyzSeries.Count);

                    j++;
                }

                Colors.Items[i] = color;
            }
        }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            return Colors.Items[index];
        }
    }
}