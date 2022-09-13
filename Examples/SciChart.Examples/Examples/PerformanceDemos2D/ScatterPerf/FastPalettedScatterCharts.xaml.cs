using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;

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

            if (VisualXcceleratorEngine.SupportsHardwareAcceleration)
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

            var random = new Random(0);
            var dataPointColors = new List<Color>();

            for (int i = 0; i < 100000; i++)
            {
                dataSeries.Append(random.NextDouble(), random.NextDouble());
                dataPointColors.Add(GetRandomColor(random));
            }

            var scatterSeries = new XyScatterRenderableSeries()
            {
                PointMarker = new EllipsePointMarker() {Width = 5, Height = 5}
            };
            sciChart.RenderableSeries.Add(scatterSeries);
            scatterSeries.DataSeries = dataSeries;
            scatterSeries.PaletteProvider = new ScatterSeriesPaletteProvider(dataPointColors);
        }

        private static Color GetRandomColor(Random random)
        {
            return Color.FromArgb(0xFF, (byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
        }
    }

    public class ScatterSeriesPaletteProvider : IPointMarkerPaletteProvider 
    {
        private readonly List<Color> _dataPointColors;
        private readonly Values<Color> _colors = new Values<Color>();

        public ScatterSeriesPaletteProvider(List<Color> dataPointColors)
        {
            _dataPointColors = dataPointColors;
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }

        public PointPaletteInfo? OverridePointMarker(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            var ppi = new PointPaletteInfo();
            ppi.Stroke = _dataPointColors[index];
            ppi.Fill = _dataPointColors[index];
            return ppi;
        }
    }
}
