// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RealTimeGhostedTraces.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public partial class RealTimeGhostedTraces : UserControl
    {
        private readonly CircularBuffer<XyDataSeries<double, double>> _dataSeries = new CircularBuffer<XyDataSeries<double, double>>(10);
        private double _lastAmplitude = 1.0;

        private DispatcherTimer _timer;

        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        readonly FasterRandom _random = new FasterRandom();

        public RealTimeGhostedTraces()
        {
            InitializeComponent();
        }

        /// <summary>
        ///     Every X milliseconds we create a new DataSeries and append new data to it. We also enqueue this into a circular buffer of 10 Data-Series. 
        ///     Then, all the DataSeries are re-assigned to RenderableSeries with varying opacity. This gives the impression of a trace 'ghosting' - becoming
        ///     more transparent as time elapses. 
        /// </summary>
        private void TimerOnElapsed(object sender, EventArgs e)
        {
            var newDataSeries = new XyDataSeries<double, double>();

            // Create a noisy sinewave and cache
            //  All this code is about the generation of data to create a nice randomized sinewave with 
            //  varying phase and amplitude
            double randomAmplitude = Constrain(_lastAmplitude + ((_random.NextDouble()) - 0.50), -2.0, 2.0);
            const double phase = 0.0;
            var noisySinewave = DataManager.Instance.GetNoisySinewave(randomAmplitude, phase, 1000, 0.25);
            _lastAmplitude = randomAmplitude;

            // Append to a new dataseries
            newDataSeries.Append(noisySinewave.XData, noisySinewave.YData);

            // Enqueue to the circular buffer
            _dataSeries.Add(newDataSeries);

            // Reassign all DataSeries to RenderableSeries
            ReassignRenderableSeries(_dataSeries);
        }

        private static double Constrain(double value, double noLowerThan, double noBiggerThan)
        {
            return Math.Max(Math.Min(value, noBiggerThan), noLowerThan);
        }

        /// <summary>
        ///     This method shifts all the data series, e.g. if you have RenderableSeries 0-9 and DataSeries 0-9, after 
        ///     a shift Dataseries 1-10 will be applied to renderableseries 0-9
        /// </summary>
        /// <param name="dataSeries"></param>
        private void ReassignRenderableSeries(CircularBuffer<XyDataSeries<double, double>> dataSeries)
        {
            // Prevent redrawing while reassigning
            using (sciChart.SuspendUpdates())
            {
                // Always the latest dataseries
                if (dataSeries.Count > 0) sciChart.RenderableSeries[0].DataSeries = dataSeries[dataSeries.Count - 1]; 
                if (dataSeries.Count > 1) sciChart.RenderableSeries[1].DataSeries = dataSeries[dataSeries.Count - 2];
                if (dataSeries.Count > 2) sciChart.RenderableSeries[2].DataSeries = dataSeries[dataSeries.Count - 3];
                if (dataSeries.Count > 3) sciChart.RenderableSeries[3].DataSeries = dataSeries[dataSeries.Count - 4];
                if (dataSeries.Count > 4) sciChart.RenderableSeries[4].DataSeries = dataSeries[dataSeries.Count - 5];
                if (dataSeries.Count > 5) sciChart.RenderableSeries[5].DataSeries = dataSeries[dataSeries.Count - 6];
                if (dataSeries.Count > 6) sciChart.RenderableSeries[6].DataSeries = dataSeries[dataSeries.Count - 7];
                if (dataSeries.Count > 7) sciChart.RenderableSeries[7].DataSeries = dataSeries[dataSeries.Count - 8];
                if (dataSeries.Count > 8) sciChart.RenderableSeries[8].DataSeries = dataSeries[dataSeries.Count - 9];
                
                // Always the oldest dataseries
                if (dataSeries.Count > 9) sciChart.RenderableSeries[9].DataSeries = dataSeries[dataSeries.Count - 10]; 
            }
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
