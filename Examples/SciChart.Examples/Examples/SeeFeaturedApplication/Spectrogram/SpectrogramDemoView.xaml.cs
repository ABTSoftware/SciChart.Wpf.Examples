// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SpectrogramDemoView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Spectrogram
{
    /// <summary>
    /// Interaction logic for SpectrogramDemoView.xaml
    /// </summary>
    public partial class SpectrogramDemoView : UserControl
    {
        private readonly DispatcherTimer _timer;
        private readonly XyDataSeries<double, double> _xyDataSeries = new XyDataSeries<double>();

        private readonly Random _random = new Random();        
        private readonly FFT2 _transform;

        private readonly object _tickLocker = new object();

        private readonly double[] _re = new double[1024];
        private readonly double[] _im = new double[1024];

        private readonly double[,] _spectrogramBuffer = new double[100, 1024];
        private readonly double[,] _pastFrame = new double[100, 1024];

        private readonly IDataSeries _uniformHeatmapDataSeries;

        public SpectrogramDemoView()
        {
            InitializeComponent();

            _transform = new FFT2();
            _transform.init(10);

            _uniformHeatmapDataSeries = new UniformHeatmapDataSeries<int, int, double>(_spectrogramBuffer, 0, 1, 0, 1); 

            FirstCreateSeries();

            LineRenderableSeries.DataSeries = _xyDataSeries;
            HeatmapRenderableSeries.DataSeries = _uniformHeatmapDataSeries; 

            UpdateDataOnTimerTick();

            _timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(10),
                IsEnabled = true
            };
            _timer.Tick += TimerOnTick;
        }

        private void OnExampleLoaded(object sender, RoutedEventArgs e)
        {
            _timer.Start();
        }

        private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void TimerOnTick(object sender, EventArgs e)
        {
            UpdateDataOnTimerTick();
        }

        private void FirstCreateSeries()
        {
            for (int x = 0; x < 100; x++)
            {
                UpdateXyDataSeries();
                for (int y = 0; y < 1024; y++)
                    _pastFrame[x, y] = _xyDataSeries.YValues[y];
            }
        }

        private void UpdateDataOnTimerTick()
        {
            using (_xyDataSeries.SuspendUpdates())
            {
                UpdateXyDataSeries();
                UpdateSpectrogramHeatmapSeries(_xyDataSeries);
            }
        }
        
        private void UpdateSpectrogramHeatmapSeries(XyDataSeries<double,double> series)
        {           
            // Compute the new spectrogram frame
            for (int x = 99; x >= 0; x--)
                for (int y = 0; y < 1024; y++)
                    _spectrogramBuffer[x, y] = (x == 99) ? series.YValues[y] : _pastFrame[x + 1, y];

            // Preserve the past frame, as current spectrogram is computed based on last + Xy fft values
            Array.Copy(_spectrogramBuffer, _pastFrame, _spectrogramBuffer.Length);

            // Forces Heatmap to redraw after updating values
            _uniformHeatmapDataSeries.InvalidateParentSurface(RangeMode.None);
        }

        private void UpdateXyDataSeries()
        {
            lock (_tickLocker)
            {               
                for (int i = 0; i < 1024; i++)
                {
                    _re[i] = 2.0 * Math.Sin(2 * Math.PI * i / 20) +
                            5 * Math.Sin(2 * Math.PI * i / 10) +
                            2.0 * _random.NextDouble();
                    _im[i] = -10;
                }

                _transform.run(_re, _im);
                for (int i = 0; i < 1024; i++)
                {
                    double mag = Math.Sqrt(_re[i] * _re[i] + _im[i] * _im[i]);
                    _re[i] = 20 * Math.Log10(mag / 1024);
                    _im[i] = i;
                }

                _xyDataSeries.Clear();
                _xyDataSeries.Append(_im, _re);             
            }
        }
    }
}