// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HeatMapWithTextInCellsExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.HeatmapChartTypes.UniformHeatmapAndPaletteProvider
{
    /// <summary>
    /// Interaction logic for UniformHeatmapAndPaletteProvider.xaml
    /// </summary>
    public partial class UniformHeatmapAndPaletteProvider : UserControl
    {
        private readonly CustomPaletteProvider _customPaletteProvider = new CustomPaletteProvider();
        private readonly Random _random = new Random();

        private double _threshholdValue;

        public double ThreshholdValue
        {
            get { return _threshholdValue; }
            set
            {
                _threshholdValue = value;
                _customPaletteProvider.ThreshholdValue = _threshholdValue;
            }
        }

        public UniformHeatmapAndPaletteProvider()
        {
            InitializeComponent();
        }

        private IDataSeries CreateSeries()
        {
            double angle = Math.PI * 2 * 1 / 30;
            int w = 300, h = 200;
            var data = new double[h, w];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    var v = (1 + Math.Sin(x * 0.04 + angle)) * 50 + (1 + Math.Sin(y * 0.1 + angle)) * 50 * (1 + Math.Sin(angle * 2));
                    var cx = 150; var cy = 100;
                    var r = Math.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                    var exp = Math.Max(0, 1 - r * 0.008);
                    data[y, x] = (v * exp + _random.NextDouble() * 50);
                }

            var xStart = new DateTime(2017, 1, 13, 0, 0, 0);
            var xStep = DateTime.MinValue.AddDays(1).AddHours(6).AddMinutes(30);
            return new UniformHeatmapDataSeries<DateTime, int, double>(data, xStart, xStep, 0, 2) { SeriesName = "UniformHeatmap seria" };
        }

        private void UniformHeatmapAndPaletteProvider_OnLoaded(object sender, RoutedEventArgs e)
        {
            Slider.DataContext = this;
            UniformHeatmapRenderableSeries.DataSeries = CreateSeries();
           
            UniformHeatmapRenderableSeries.ColorMap = new HeatmapColorPalette
            {
                GradientStops = 
                {
                    new GradientStop(Colors.Blue, 0), 
                    new GradientStop(Colors.White, 0.3), 
                    new GradientStop(Colors.Green, 0.5),
                    new GradientStop(Colors.Yellow, 0.7),
                    new GradientStop(Colors.Red, 1.0),
                },
                Minimum = 0,
                Maximum = 100,
            };
            ThreshholdValue = 50;
        }

        private void OnPalletProvider_Clicked(object sender, RoutedEventArgs e)
        {
            UniformHeatmapRenderableSeries.PaletteProvider = UniformHeatmapRenderableSeries.PaletteProvider == null ? _customPaletteProvider : null;
            SciChartSurface.ZoomExtents();
        }
    }

    // Custom PaletteProvider
    public class CustomPaletteProvider : IHeatmapPaletteProvider
    {
        private FastUniformHeatmapRenderableSeries _rSeries;
        private double[,] _zValues;
        private bool _isYFlipped;
        private bool _isXFlipped;
        private int _textureHeight;
        private int _textureWidth;

        private double _threshholdValue;
        public double ThreshholdValue
        {
            get { return _threshholdValue; }
            set
            {
                _threshholdValue = value;
                if (_rSeries == null) return;
                _rSeries.OnInvalidateParentSurface();
            }
        }

        Color? IHeatmapPaletteProvider.OverrideCellColor(IRenderableSeries rSeries, int xIndex, int yIndex)
        {
            var y = !_isYFlipped ? (_textureHeight - 1) - yIndex : yIndex;
            var x = _isXFlipped ? (_textureWidth - xIndex) - 1 : xIndex;

            var zValue = _zValues[y, x];

            return zValue >= ThreshholdValue ? Colors.Black : Colors.White;
        }

        void IPaletteProvider.OnBeginSeriesDraw(IRenderableSeries rSeries)
        {
            _rSeries = (FastUniformHeatmapRenderableSeries)rSeries;
            //var dataSeries = (IUniformHeatmapDataSeries) _rSeries.DataSeries;
            var dataSeries = (IBaseHeatmapDataSeries)_rSeries.DataSeries;
            _zValues = dataSeries.GetZValuesAsDoubles();

            _textureHeight = _zValues.GetLength(0);
            _textureWidth = _zValues.GetLength(1);

            _isXFlipped = _rSeries.XAxis.FlipCoordinates;
            _isYFlipped = _rSeries.YAxis.FlipCoordinates;
        }
    }
}
