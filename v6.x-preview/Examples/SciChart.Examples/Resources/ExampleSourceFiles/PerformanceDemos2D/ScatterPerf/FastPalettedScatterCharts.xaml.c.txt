using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Drawing.DirectX.Context.D3D11;

namespace SciChart.Examples.Examples.PerformanceDemos2D.ScatterPerf
{
    /// <summary>
    /// Interaction logic for FastPalettedScatterCharts.xaml
    /// </summary>
    public partial class FastPalettedScatterCharts : UserControl
    {
        public FastPalettedScatterCharts()
        {
            InitializeComponent();

            if (Direct3D11CompatibilityHelper.SupportsDirectX10)
            {
                warningText.IsHidden = true;
                SetupScatterSeries();
            }
            else
            {
                warningText.IsHidden = false;
            }
        }

        private void SetupScatterSeries()
        {
            var dataSeries = new XyDataSeries<double, double>() { AcceptsUnsortedData = true };

            var random = new Random();
            var dataPointColors = new List<Color>();

            for (int i = 0; i < 100000; i++)
            {
                dataSeries.Append(random.NextDouble(), random.NextDouble());
                dataPointColors.Add(GetRandomColor(random));
            }

            var scatterSeries = new ExtremeScatterRenderableSeries()
            {
                PointMarker = new EllipsePointMarker() {Width = 5, Height = 5}
            };
            sciChart.RenderableSeries.Add(scatterSeries);
            scatterSeries.DataSeries = dataSeries;
            scatterSeries.PaletteProvider = new TestPaletteProvider(dataPointColors);
        }

        private static Color GetRandomColor(Random random)
        {
            return Color.FromArgb(0xFF, (byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
        }
    }

    public class TestPaletteProvider : IExtremePointMarkerPaletteProvider
    {
        private readonly List<Color> _dataPointColors;
        private readonly Values<Color> _colors = new Values<Color>();

        public TestPaletteProvider(List<Color> dataPointColors)
        {
            _dataPointColors = dataPointColors;
        }

        public Values<Color> Colors { get { return _colors; } }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            var indexes = rSeries.CurrentRenderPassData.PointSeries.Indexes;

            var count = indexes.Count;
            _colors.Count = count;

            // copy required colors from list using data point indices
            for (int i = 0; i < count; i++)
            {
                var dataPointIndex = indexes[i];
                _colors[i] = _dataPointColors[dataPointIndex];
            }
        }
    }
}
