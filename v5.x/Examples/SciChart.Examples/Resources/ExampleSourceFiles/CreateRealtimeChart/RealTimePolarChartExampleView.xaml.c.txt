// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RealTimePolarChartExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Core.Helpers;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateRealtimeChart
{
    /// <summary>
    /// Interaction logic for RealTimePolarChart.xaml
    /// </summary>
    public partial class RealTimePolarChart : UserControl
    {
        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        readonly FasterRandom _random = new FasterRandom();
        private double _lastAmplitude = 1.0;
        private DispatcherTimer _timer;

        public RealTimePolarChart()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Every X milliseconds we create a new DataSeries and append new data to it.
        /// Then, DataSeries are re-assigned to RenderableSeries. 
        /// </summary>
        private void TimerOnElapsed(object sender, EventArgs e)
        {
            var newDataSeries = new XyDataSeries<double, double>();

            // Create a noisy sinewave and cache
            // All this code is about the generation of data to create a nice randomized sinewave with 
            // varying phase and amplitude
            var randomAmplitude = Constrain(_lastAmplitude + ((_random.NextDouble()) - 0.50)/2, -2.0, 2.0);
            const double phase = 0.0;
            var noisySinewave = DataManager.Instance.GetNoisySinewave(randomAmplitude, phase, 1000, 0.25);
            _lastAmplitude = randomAmplitude;

            // Append to a new dataseries
            newDataSeries.Append(noisySinewave.XData, noisySinewave.YData);


            lineSeries.DataSeries = newDataSeries;
        }

        private static double Constrain(double value, double noLowerThan, double noBiggerThan)
        {
            return Math.Max(Math.Min(value, noBiggerThan), noLowerThan);
        }

        private void Slider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (_timer != null)
            {
                _timer.Interval = TimeSpan.FromMilliseconds(Slider.Value);
            }
        }

        private void OnExampleLoaded(object sender, RoutedEventArgs e)
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer(DispatcherPriority.Render);

                _timer.Interval = TimeSpan.FromMilliseconds(Slider.Value);
                _timer.Tick += TimerOnElapsed;

                _timer.Start();
            }
        }

        private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        {
            if (_timer != null)
            {
                _timer.Stop();

                _timer.Tick -= TimerOnElapsed;

                _timer = null;
            }
        }
    }
}
