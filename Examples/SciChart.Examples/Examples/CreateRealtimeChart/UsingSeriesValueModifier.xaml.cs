// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// UsingSeriesValueModifier.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.CreateRealtimeChart
{
    public partial class UsingSeriesValueModifier : UserControl
    {
        // Data Sample Rate (sec) - 20 Hz
        private const double dt = 0.05;

        // FIFO Size is 100 samples, meaning after 100 samples have been appended, each new sample appended
        // results in one sample being discarded
        private const int FifoSize = 100;

        // Timer to process updates
        private readonly Timer _timerNewDataUpdate;

        // The current time
        private double t;

        // The dataseries to fill
        private readonly IUniformXyDataSeries<double> _series0;
        private readonly IUniformXyDataSeries<double> _series1;
        private readonly IUniformXyDataSeries<double> _series2;

        public UsingSeriesValueModifier()
        {
            InitializeComponent();

            _timerNewDataUpdate = new Timer(dt * 1000) {AutoReset = true};
            _timerNewDataUpdate.Elapsed += OnNewData;

            // Create new dataseries of type X=double, Y=double
            _series0 = new UniformXyDataSeries<double>(t, dt) { FifoCapacity = FifoSize, SeriesName = "Orange Series" };
            _series1 = new UniformXyDataSeries<double>(t, dt) { FifoCapacity = FifoSize, SeriesName = "Blue Series" };
            _series2 = new UniformXyDataSeries<double>(t, dt) { FifoCapacity = FifoSize, SeriesName = "Green Series" };

            // Set the dataseries on the chart's RenderableSeries
            renderableSeries0.DataSeries = _series0;
            renderableSeries1.DataSeries = _series1;
            renderableSeries2.DataSeries = _series2;
        }

        private void ClearDataSeries()
        {
            using (sciChartSurface.SuspendUpdates())
            {
                _series0?.Clear();
                _series1?.Clear();
                _series2?.Clear();
            }
        }

        private void OnNewData(object sender, EventArgs e)
        {
            // Compute our three series values
            double y1 = 3.0 * Math.Sin(2 * Math.PI * 1.4 * t * 0.02);
            double y2 = 2.0 * Math.Cos(2 * Math.PI * 0.8 * t * 0.02);
            double y3 = 1.0 * Math.Sin(2 * Math.PI * 2.2 * t * 0.02);

            // Suspending updates is optional, and ensures we only get one redraw
            // once all three dataseries have been appended to
            using (sciChartSurface.SuspendUpdates())
            {
                // Append x,y data to previously created series
                _series0.Append(y1);
                _series1.Append(y2);
                _series2.Append(y3);
            }

            // Increment current time
            t += dt;
        }

        private void OnExampleLoaded(object sender, RoutedEventArgs e)
        {
            ClearDataSeries();

            _timerNewDataUpdate.Start();
        }

        private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        {
            _timerNewDataUpdate?.Stop();
        }
    }
}