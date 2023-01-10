using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Drawing.VisualXcceleratorRasterizer;

namespace SciChart.Examples.Examples.PerformanceDemos2D.ScatterPerf
{
    /// <summary>
    /// Interaction logic for FastPalettedScatterCharts.xaml
    /// </summary>
    public partial class FastPalettedScatterCharts : UserControl
    {
        private XyScatterRenderableSeries _xyScatterSeries;

        public FastPalettedScatterCharts()
        {
            InitializeComponent();
            SetupSeriesForRenderSurface();

            var renderSurfaceProp = DependencyPropertyDescriptor.FromProperty
                (SciChartSurfaceBase.RenderSurfaceProperty, typeof(SciChartSurfaceBase));

            Loaded += (s, e) => renderSurfaceProp.AddValueChanged(sciChart, OnRenderSurfaceChanged);
            Unloaded += (s, e) => renderSurfaceProp.RemoveValueChanged(sciChart, OnRenderSurfaceChanged);
        }

        private void OnRenderSurfaceChanged(object sender, EventArgs args)
        {
            SetupSeriesForRenderSurface();
        }

        private void SetupSeriesForRenderSurface()
        {
            if (sciChart.RenderSurface is VisualXcceleratorRenderSurface &&
                VisualXcceleratorEngine.SupportsHardwareAcceleration)
            {
                if (_xyScatterSeries == null)
                {
                    var dataSeries = new XyDataSeries<double, double>() { AcceptsUnsortedData = true };
                    var random = new Random(0);
                    var dataPointColors = new List<Color>();

                    for (int i = 0; i < 100000; i++)
                    {
                        dataSeries.Append(random.NextDouble(), random.NextDouble());
                        dataPointColors.Add(GetRandomColor(random));
                    }

                    _xyScatterSeries = new XyScatterRenderableSeries()
                    {
                        DataSeries = dataSeries,
                        PointMarker = new EllipsePointMarker { Width = 5, Height = 5 },
                        PaletteProvider = new ScatterSeriesPaletteProvider(dataPointColors)
                    };
                }

                warningText.Visibility = Visibility.Collapsed;

                if (sciChart.RenderableSeries.Count == 0)
                {
                    sciChart.RenderableSeries.Add(_xyScatterSeries);
                }
            }
            else
            {
                warningText.Visibility = Visibility.Visible;

                sciChart.RenderableSeries.Clear();
            }
        }

        private static Color GetRandomColor(Random random)
        {
            return Color.FromArgb(0xFF, (byte)random.Next(255), (byte)random.Next(255), (byte)random.Next(255));
        }
    }

    public class ScatterSeriesPaletteProvider : IPointMarkerPaletteProvider
    {
        private PointPaletteInfo _paletteInfo;

        private readonly List<Color> _dataPointColors;

        public ScatterSeriesPaletteProvider(List<Color> dataPointColors)
        {
            _dataPointColors = dataPointColors;

            _paletteInfo = new PointPaletteInfo();
        }

        public void OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
        }

        public PointPaletteInfo? OverridePointMarker(IRenderableSeries rSeries, int index, IPointMetadata metadata)
        {
            _paletteInfo.Stroke = _dataPointColors[index];
            _paletteInfo.Fill = _dataPointColors[index];

            return _paletteInfo;
        }
    }
}