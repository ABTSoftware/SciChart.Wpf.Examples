using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.VitalSignsMonitor
{
    public class VitalSignsPaletteProvider : IStrokePaletteProvider
    {
        private readonly byte _defaultAlpha;
        private readonly byte _diffAlpha;

        public Values<Color> Colors { get; } = new Values<Color>();

        public VitalSignsPaletteProvider(byte defaultAlpha = 50)
        {
            _defaultAlpha = defaultAlpha;
            _diffAlpha = (byte)(255 - defaultAlpha);
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            var defaultColor = rSeries.Stroke;
            var count = rSeries.DataSeries.Count; 
            
            if (Colors.Count != count)
            {
                Colors.Count = count;

                for (int i = 0; i < count; i++)
                {
                    var color = defaultColor;
                    color.A = (byte)(_defaultAlpha + _diffAlpha * i / count);
                    Colors.Items[i] = color;
                }
            }
        }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            return Colors.Items[index];
        }
    }
}