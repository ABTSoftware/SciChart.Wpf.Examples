using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace ColumnSeriesNoGapsExample
{
    public partial class ColumnSeriesNoGaps : Window
    {
        public ColumnSeriesNoGaps()
        {
            InitializeComponent();

            var data = new XyDataSeries<double>();
            int offset = 10;
            data.Append(0+offset,1);
            data.Append(20 + offset, 2);
            data.Append(40 + offset, 3);
            data.Append(60 + offset, 4);
            data.Append(80 + offset, 5);
            columnSeries.DataSeries = data;
            columnSeries.PaletteProvider = new RandomColumnColorizer();
        }
    }

    public class RandomColumnColorizer : IFillPaletteProvider, IStrokePaletteProvider
    {
        private Color[] colors = new[]
        {
            Colors.DarkOrange,
            Colors.ForestGreen,
            Colors.CornflowerBlue,
            Colors.MediumPurple,
            Colors.OrangeRed
        };
        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {            
        }

        public Color? OverrideStrokeColor(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            return colors[index];
        }

        public Brush OverrideFillBrush(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            return new SolidColorBrush(colors[index]);
        }
    }
}
