// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HeatMapExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.RenderableSeries.Animations;

namespace SciChart.Examples.Examples.HeatmapChartTypes.RealTimeHeatmap
{
    /// <summary>
    /// demonstrates use of FastHeatmapRenderableSeries
    /// creates a list of Heatmap2dArrayDataSeries and displays them one by one in loop on timer
    /// </summary>
    public partial class HeatMapExampleView : UserControl
    {
        /// <summary>
        /// number of series to be displayed in a loop (period)
        /// </summary>
        private const int seriesPerPeriod = 100;

        private readonly DispatcherTimer _timer;

        private int _timerIndex = 0;

        public HeatMapExampleView()
        {
            InitializeComponent();

            heatmapSeries.DataSeries = CreateSeries(0, 300, 200, 0, heatmapSeries.ColorMap.Maximum);

            _timer = new DispatcherTimer(DispatcherPriority.Render)
            {
                Interval = TimeSpan.FromMilliseconds(1),

                IsEnabled = SeriesAnimationBase.GlobalEnableAnimations
            };

            _timer.Tick += TimerOnTick;
        }

        /// <summary>
        /// displays next data series in list
        /// </summary>
        private void TimerOnTick(object sender, EventArgs eventArgs)
        {
            var colormap = heatmapSeries.ColorMap;
            var cpMax = colormap.Maximum;

            // _dataSeries[i] = CreateSeries(i, 300, 200, cpMin, cpMax);
            _timerIndex++;
            heatmapSeries.DataSeries = CreateSeries((int)(_timerIndex % seriesPerPeriod), 300, 200, 0, cpMax);
        }

        private IDataSeries CreateSeries(int index, int width, int height, double cpMin, double cpMax)
        {
            var seed = SeriesAnimationBase.GlobalEnableAnimations ? (Environment.TickCount << index) : 0;
            var random = new Random(seed);
            
            double angle = Math.Round(Math.PI * 2 * index, 3) / seriesPerPeriod;
           
            int w = width, h = height;
            var data = new double[h, w];
            
            for (int x = 0; x < w; x++)
            { 
                for (int y = 0; y < h; y++)
                {
                    var v = (1 + Math.Round(Math.Sin(x * 0.04 + angle), 3)) * 50 + (1 + Math.Round(Math.Sin(y * 0.1 + angle), 3)) * 50 * (1 + Math.Round(Math.Sin(angle * 2), 3));
                    var cx = w / 2; var cy = h / 2;
                    var r = Math.Sqrt((x - cx) * (x - cx) + (y - cy) * (y - cy));
                    var exp = Math.Max(0, 1 - r * 0.008);
                    var zValue = (v * exp + random.NextDouble() * 10);
                    data[y, x] = (zValue > cpMax) ? cpMax : zValue;
                }
            }
            return new UniformHeatmapDataSeries<int, int, double>(data, 0, 1, 0, 1);
        }

        private void OnExampleLoaded(object sender, RoutedEventArgs e)
        {
            if (SeriesAnimationBase.GlobalEnableAnimations)
            {
                _timer.Start();
            }
        }

        private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        {
            _timer.Stop();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Start();

            StartButton.IsChecked = true;
            StopButton.IsChecked = false;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _timer.Stop();

            StartButton.IsChecked = false;
            StopButton.IsChecked = true;
        }
    }
}