// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RealTimePerformanceDemoView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Utility;
using SciChart.Data.Numerics;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.PerformanceDemo
{
    public partial class RealTimePerformanceDemoView : UserControl
    {
        private Random _random;
        private bool _running;
        private MovingAverage _maLow;
        private MovingAverage _maHigh;
        private Stopwatch _stopWatch;
        private MovingAverage _fpsAverage;
        private double _lastFrameTime;

        private const int MaxCount = 10000000; // Max number of points to draw before demo stops
        private const int BufferSize = 1000; // Number of points to append to each channel each tick
        private const int TimerInterval = 10; // Interval of the timer to generate data in ms

        // X, Y buffers used to buffer data into the Scichart in blocks of BufferSize
        // This gives an increase in rendering throughput over one off appends of X, Y points
        private int[] xBuffer = new int[BufferSize];
        private float[] yBuffer = new float[BufferSize];
        private float[] maLowBuffer = new float[BufferSize];
        private float[] maHighBuffer = new float[BufferSize];

        private Timer _timer;
        private TimedMethod _startDelegate;
        private XyDataSeries<int, float> _mainSeries;
        private XyDataSeries<int, float> _maLowSeries;
        private XyDataSeries<int, float> _maHighSeries;

        public RealTimePerformanceDemoView()
        {
            InitializeComponent();

            ResamplingCombo.Items.Add(ResamplingMode.None);
            ResamplingCombo.Items.Add(ResamplingMode.MinMax);            
            ResamplingCombo.Items.Add(ResamplingMode.Mid);
            ResamplingCombo.Items.Add(ResamplingMode.Min);
            ResamplingCombo.Items.Add(ResamplingMode.Max);
            ResamplingCombo.Items.Add(ResamplingMode.Auto);
            ResamplingCombo.Items.Add(ResamplingMode.MinMaxWithUnevenSpacing);
            ResamplingCombo.SelectedItem = ResamplingMode.Auto;

            StrokeCombo.Items.Add(1);
            StrokeCombo.Items.Add(2);
            StrokeCombo.Items.Add(3);
            StrokeCombo.Items.Add(4);
            StrokeCombo.Items.Add(5);
            StrokeCombo.SelectedItem = 1;

            // Used purely for FPS reporting
            sciChart.Rendered += OnSciChartRendered;
        }

        private void DataAppendLoop()
        {
            // By nesting multiple updates inside a SuspendUpdates using block, you get one redraw at the end
            using (sciChart.SuspendUpdates())
            {
                // Preload previous value with k-1 sample, or 0.0 if the count is zero
                int xValue = _mainSeries.Count > 0 ? _mainSeries.XValues[_mainSeries.Count - 1] : 0;
                double yValue = _mainSeries.Count > 0 ? _mainSeries.YValues[_mainSeries.Count - 1] : 10.0f;

                // Add N points at a time. We want to get to the higher point counts 
                // quickly to demonstrate performance. 
                // Also, it is more efficient to buffer and block update the chart
                // even if you use SuspendUpdates due to the overhead of calculating min, max
                // for a series
                for (int i = 0; i < BufferSize; i++)
                {
                    // Generate a new X,Y value in the random walk and buffer
                    xValue = xValue + 1;
                    yValue = (double)(yValue + (_random.NextDouble() - 0.5));

                    xBuffer[i] = xValue;
                    yBuffer[i] = (float)yValue;

                    // Update moving averages
                    maLowBuffer[i] = (float)_maLow.Push(yValue).Current;
                    maHighBuffer[i] = (float)_maHigh.Push(yValue).Current;
                }

                // Append block of values to all three series
                _mainSeries.Append(xBuffer, yBuffer);
                _maLowSeries.Append(xBuffer, maLowBuffer);
                _maHighSeries.Append(xBuffer, maHighBuffer);
            }
        }

        private void OnSciChartRendered(object sender, EventArgs e)
        {
            // Compute the render time
            double frameTime = _stopWatch.ElapsedMilliseconds;
            double delta = frameTime - _lastFrameTime;
            double fps = 1000.0 / delta;
            double fpsAverageBefore = _fpsAverage.Current;

            // Push the fps to the movingaverage, we want to average the FPS to get a more reliable reading
            if (!double.IsInfinity(fps))
            {
                _fpsAverage.Push(fps);
            }

            double fpsAverageAfter = _fpsAverage.Current;

            // Render the fps to the screen
            if (Math.Abs(fpsAverageAfter - fpsAverageBefore) >= 0.1)
                FpsCounter.Text = double.IsNaN(_fpsAverage.Current) ? "-" : string.Format("{0:0}", _fpsAverage.Current);

            // Render the total point count (all series) to the screen
            int numPoints = 3 * _mainSeries.Count;
            PointCount.Text = string.Format("{0:n0}", numPoints);

            if (numPoints > MaxCount)
            {
                this.PauseButton_Click(this, null);
            }

            _lastFrameTime = frameTime;
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Start();

            StartButton.IsChecked = true;
            PauseButton.IsChecked = false;
            ResetButton.IsChecked = false;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Pause();

            StartButton.IsChecked = false;
            PauseButton.IsChecked = true;
            ResetButton.IsChecked = false;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();

            StartButton.IsChecked = false;
            PauseButton.IsChecked = false;
            ResetButton.IsChecked = true;
        }

        private void Start()
        {
            if (!_running)
            {
                EnableInteractivity(false);
                _running = true;
                _stopWatch.Start();
                _timer = new Timer(TimerInterval);
                _timer.Elapsed += OnTick;
                _timer.AutoReset = true;
                _timer.Start();
            }

            sciChart.InvalidateElement();
        }

        private void Pause()
        {
            if (_running)
            {
                EnableInteractivity(true);
                _running = false;
                _timer.Stop();
                _timer.Elapsed -= OnTick;
                _timer = null;
            }

            sciChart.InvalidateElement();
        }

        private void Reset()
        {
            if (_running)
            {
                Pause();
            }

            using (sciChart.SuspendUpdates())
            {
                var yRange = sciChart.YAxis.VisibleRange;
                var xRange = sciChart.XAxis.VisibleRange;

                RenderableSeries0 = (FastLineRenderableSeries)sciChart.RenderableSeries[0];
                RenderableSeries1 = (FastLineRenderableSeries)sciChart.RenderableSeries[1];
                RenderableSeries2 = (FastLineRenderableSeries)sciChart.RenderableSeries[2];

                // Create three DataSeries
                _mainSeries = new XyDataSeries<int, float>();
                _maLowSeries = new XyDataSeries<int, float>();
                _maHighSeries = new XyDataSeries<int, float>();

                RenderableSeries0.DataSeries = _mainSeries;
                RenderableSeries1.DataSeries = _maLowSeries;
                RenderableSeries2.DataSeries = _maHighSeries;

                EnableInteractivity(false);
                _maLow = new MovingAverage(200);
                _maHigh = new MovingAverage(1000);
                _fpsAverage = new MovingAverage(50);
                _random = new Random((int)(DateTime.Now.Ticks));
                _lastFrameTime = 0;
                _stopWatch = new Stopwatch();

                if (_timer != null)
                {
                    _timer.Elapsed -= OnTick;
                    _timer = null;
                }

                sciChart.YAxis.VisibleRange = yRange;
                sciChart.XAxis.VisibleRange = xRange;
            }
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Ensure only one timer Tick processed at a time
            lock (_timer)
            {
                DataAppendLoop();
            }
        }

        private void EnableInteractivity(bool enable)
        {
            if (!enable)
            {
                sciChart.XAxis.AutoRange = AutoRange.Always;
                sciChart.YAxis.AutoRange = AutoRange.Always;

                enableZoom.IsEnabled = false;
                enablePan.IsEnabled = false;
                enableZoom.IsChecked = false;
                enablePan.IsChecked = false;
            }
            else
            {
                enableZoom.IsEnabled = true;
                enablePan.IsEnabled = true;
                enableZoom.IsChecked = true;
                enablePan.IsChecked = false;

                sciChart.XAxis.AutoRange = AutoRange.Once;
                sciChart.YAxis.AutoRange = AutoRange.Once;
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RenderableSeries0 == null || RenderableSeries1 == null || RenderableSeries2 == null)
                return;

            // batch updates for efficiency
            using (sciChart.SuspendUpdates())
            {
                var mode = (ResamplingMode)ResamplingCombo.SelectedItem;

                // Set the resampling mode on all series
                RenderableSeries0.ResamplingMode = mode;
                RenderableSeries1.ResamplingMode = mode;
                RenderableSeries2.ResamplingMode = mode;

                if (StrokeCombo.SelectedItem == null)
                    return;

                var strokeThickness = (int)StrokeCombo.SelectedItem;

                // Set the StrokeThickness on all series
                RenderableSeries0.StrokeThickness = strokeThickness;
                RenderableSeries1.StrokeThickness = strokeThickness;
                RenderableSeries2.StrokeThickness = strokeThickness;
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (RenderableSeries0 == null || RenderableSeries1 == null || RenderableSeries2 == null)
                return;

            // batch updates for efficiency
            using (sciChart.SuspendUpdates())
            {
                // Set Antialiasing Flag on all series
                bool useAA = ((CheckBox)sender).IsChecked == true;
                RenderableSeries0.AntiAliasing = useAA;
                RenderableSeries1.AntiAliasing = useAA;
                RenderableSeries2.AntiAliasing = useAA;
            }
        }

        private void OnExampleLoaded(object sender, RoutedEventArgs e)
        {
            // Manages the state of example on enter
            Reset();

            _startDelegate = TimedMethod.Invoke(this.Start).After(500).Go();
        }

        private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        {
            // Manages the state of the example on exit
            if (_startDelegate != null)
            {
                _startDelegate.Dispose();
                _startDelegate = null;
            }

            Pause();
        }
    }
}
