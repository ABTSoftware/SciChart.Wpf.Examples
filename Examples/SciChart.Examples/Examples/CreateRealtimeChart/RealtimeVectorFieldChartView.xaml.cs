// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// RealtimeVectorFieldChartView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Model;
using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SciChart.Examples.Examples.CreateRealtimeChart
{
    /// <summary>
    /// Demonstrates a real-time animated <see cref="VectorFieldRenderableSeries"/> backed by a
    /// <see cref="UniformVectorFieldDataSeries"/> on a regular grid.
    /// The vector field is computed from four interfering radial wave sources, producing smooth
    /// animated flow patterns.
    ///
    /// Two update modes are supported:
    /// <list type="bullet">
    ///   <item><description>
    ///     <b>Normal</b> — a <see cref="System.Threading.Timer"/> fires every 20 ms on a thread-pool
    ///     thread to update the data arrays and sets a flag; <see cref="CompositionTarget.Rendering"/>
    ///     calls <c>InvalidateParentSurface</c> only when the flag is set. This caps updates at ~50 fps
    ///     while keeping the render thread free between updates.
    ///   </description></item>
    ///   <item><description>
    ///     <b>Performance</b> (<see cref="_enablePerformanceShowMode"/> = true) — the data update runs
    ///     directly inside the <see cref="CompositionTarget.Rendering"/> handler every frame, giving
    ///     the maximum achievable throughput with no artificial cap.
    ///   </description></item>
    /// </list>
    /// </summary>
    public partial class RealtimeVectorFieldChartView : UserControl
    {
        // Grid layout uses a 3:2 aspect ratio matching the [-15..15] x [-10..10] data domain
        private const double MagnitudeMax = 2.1;

        // VectorScale converts raw wave amplitude (~1.0 peak) to data-space displacement units
        private const double VectorScale = MagnitudeMax / 4.0;

        // WaveK: spatial wave number (rad/unit); WaveSpeed: temporal animation rate (rad/s)
        private const double WaveK = 0.8;
        private const double WaveSpeed = 1.5;

        // Interval between data updates in normal (non-performance) mode
        private const int UpdateIntervalMs = 20;

        // Four point sources at varied positions and initial phases produce interference patterns
        private static readonly (double Cx, double Cy, double Phase0)[] WaveSources =
        {
            (-8.0, -5.0, 0.00),
            ( 7.0, -4.0, 1.05),
            (-3.0,  7.0, 2.09),
            ( 5.0,  5.5, 3.14),
        };

        // Pre-built distribution args for InvalidateParentSurface.
        // Providing the expected data extents lets SciChart skip a full range scan each frame.
        private static readonly FieldDataDistributionArgs DistributionArgs = new FieldDataDistributionArgs
        {
            XRange = new DoubleRange(-15d - MagnitudeMax, 15d + MagnitudeMax),
            YRange = new DoubleRange(-10d - MagnitudeMax, 10d + MagnitudeMax),
            MagnitudeRange = new DoubleRange(0, MagnitudeMax),
            IsStationary = false
        };

        // _dXs / _dYs are the backing displacement arrays shared with the data series.
        // The background thread writes to them; SciChart reads them during its render pass.
        private double[,] _dXs;
        private double[,] _dYs;
        private double _phase;

        private UniformVectorFieldDataSeries _dataSeries;
        private Timer _updateTimer;
        private bool _isRunning;
        private volatile bool _newDataReady;

        private bool _enablePerformanceShowMode;

        /// <summary>
        /// Initializes a new instance of <see cref="RealtimeVectorFieldChartView"/>.
        /// </summary>
        public RealtimeVectorFieldChartView()
        {
            InitializeComponent();
            // Timer starts suspended; Change() activates it when the animation starts
            _updateTimer = new Timer(OnUpdateTimer, null, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// When <c>true</c>, the data update runs directly inside the <see cref="CompositionTarget.Rendering"/>
        /// handler on every frame, giving maximum throughput with no rate cap.
        /// When <c>false</c>, a <see cref="System.Threading.Timer"/> drives updates at
        /// <see cref="UpdateIntervalMs"/> ms intervals, keeping the render thread free between updates.
        /// Also controls visibility of the <see cref="ChartPerformanceOverlay"/>.
        /// </summary>
        private bool EnablePerformanceShowMode
        {
            get => _enablePerformanceShowMode;
            set
            {
                _enablePerformanceShowMode = value;
                ChartPerformanceOverlay.Visibility = _enablePerformanceShowMode ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void OnExampleLoaded(object sender, RoutedEventArgs e)
        {
            _dataSeries = GenerateVectorFieldData(70, 46);
            VectorSeries.DataSeries = _dataSeries;

            foreach (VectorColorMode mode in Enum.GetValues(typeof(VectorColorMode)))
                ColorModeCombo.Items.Add(mode);
            ColorModeCombo.SelectedItem = VectorColorMode.ByMagnitude;

            foreach (VectorLengthMode mode in Enum.GetValues(typeof(VectorLengthMode)))
                LengthModeCombo.Items.Add(mode);
            LengthModeCombo.SelectedItem = VectorLengthMode.DataUnits;

            OnStartClick(this, null);
        }

        private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        {
            OnStopClick(this, null);
            _updateTimer?.Dispose();
            _dataSeries?.Clear(true);
            ColorModeCombo.Items.Clear();
            LengthModeCombo.Items.Clear();
        }

        /// <summary>
        /// Creates a new <see cref="UniformVectorFieldDataSeries"/> for the given grid dimensions,
        /// allocating backing displacement arrays and evaluating the field at phase 0.
        /// </summary>
        private UniformVectorFieldDataSeries GenerateVectorFieldData(int xCount, int yCount)
        {
            const double xMin = -15d, xMax = 15d;
            const double yMin = -10d, yMax = 10d;

            double xStep = (xMax - xMin) / (xCount - 1);
            double yStep = (yMax - yMin) / (yCount - 1);

            _dXs = new double[xCount, yCount];
            _dYs = new double[xCount, yCount];

            for (int xi = 0; xi < xCount; xi++)
            {
                double x = xMin + xi * xStep;
                for (int yi = 0; yi < yCount; yi++)
                {
                    GetFieldVector(x, yMin + yi * yStep, 0, out double fdx, out double fdy);
                    _dXs[xi, yi] = fdx * VectorScale;
                    _dYs[xi, yi] = fdy * VectorScale;
                }
            }

            return new UniformVectorFieldDataSeries(xMin, xStep, yMin, yStep, _dXs, _dYs);
        }

        /// <summary>
        /// Computes the superimposed radial wave field at data point (x, y) and time t.
        /// Each wave source contributes a radially-directed component with cosine amplitude.
        /// </summary>
        private static void GetFieldVector(double x, double y, double t, out double vx, out double vy)
        {
            vx = 0.0;
            vy = 0.0;
            foreach (var (cx, cy, phi0) in WaveSources)
            {
                double rx = x - cx;
                double ry = y - cy;
                double r = Math.Sqrt(rx * rx + ry * ry);
                if (r < 0.2) continue;

                double c = Math.Cos(WaveK * r - WaveSpeed * t + phi0);
                vx += c * rx / r;
                vy += c * ry / r;
            }
        }

        /// <summary>
        /// Timer callback — fires every 20 ms on a thread-pool thread.
        /// Advances the animation phase, rewrites all displacement values, then signals
        /// the rendering handler via <see cref="_newDataReady"/>.
        /// </summary>
        private void OnUpdateTimer(object state)
        {
            // Snapshot data references so a concurrent grid-size change (UI thread) cannot
            // cause index-out-of-bounds mid-write.
            var ds = _dataSeries;
            var dxs = _dXs;
            var dys = _dYs;
            if (ds == null || dxs == null || dys == null) return;

            _phase += 0.04;
            for (int xi = 0; xi < ds.XCount; xi++)
            {
                double x = ds.XStart + xi * ds.XStep;
                for (int yi = 0; yi < ds.YCount; yi++)
                {
                    GetFieldVector(x, ds.YStart + yi * ds.YStep, _phase, out double vx, out double vy);
                    dxs[xi, yi] = vx * VectorScale;
                    dys[xi, yi] = vy * VectorScale;
                }
            }

            // All writes complete — signal the rendering handler that fresh data is available
            _newDataReady = true;
        }

        /// <summary>
        /// Called every WPF composition frame.
        /// In performance mode the data update runs here directly, so the chart redraws every frame
        /// with no cap. In normal mode the chart is invalidated only when the timer has set the flag.
        /// </summary>
        private void OnRenderingTick(object sender, EventArgs e)
        {
            if (_enablePerformanceShowMode)
            {
                OnUpdateTimer(null);
                _dataSeries?.InvalidateParentSurface(RangeMode.None, DistributionArgs, true);
            }
            else
            {
                if (!_newDataReady) return;
                _newDataReady = false;
                _dataSeries?.InvalidateParentSurface(RangeMode.None, DistributionArgs, true);
            }
        }

        private void OnStartClick(object sender, RoutedEventArgs e)
        {
            ChangeGridSizeButton.IsEnabled = false;
            StartButton.IsEnabled = false;
            StartButton.IsChecked = true;
            StopButton.IsEnabled = true;
            StopButton.IsChecked = false;

            if (!_isRunning)
            {
                _isRunning = true;
                CompositionTarget.Rendering -= OnRenderingTick;
                CompositionTarget.Rendering += OnRenderingTick;
                // In performance mode the rendering handler drives updates itself every frame;
                // in normal mode the timer produces data independently at ~50 fps.
                if (!_enablePerformanceShowMode)
                    _updateTimer.Change(0, UpdateIntervalMs);
            }
        }

        private void OnStopClick(object sender, RoutedEventArgs e)
        {
            if (_isRunning)
            {
                _isRunning = false;
                if (!_enablePerformanceShowMode)
                    _updateTimer.Change(Timeout.Infinite, Timeout.Infinite);
                CompositionTarget.Rendering -= OnRenderingTick;
            }

            ChangeGridSizeButton.IsEnabled = true;
            StartButton.IsEnabled = true;
            StartButton.IsChecked = false;
            StopButton.IsEnabled = false;
            StopButton.IsChecked = true;
        }

        private void OnPerformanceModeToggled(object sender, RoutedEventArgs e)
        {
            bool wasRunning = _isRunning;
            if (wasRunning) OnStopClick(this, null);
            EnablePerformanceShowMode = PerformanceModeButton.IsChecked == true;
            if (wasRunning) OnStartClick(this, null);
        }

        private void OnLengthModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VectorSeries == null || LengthModeCombo.SelectedItem == null) return;
            var mode = (VectorLengthMode)LengthModeCombo.SelectedItem;
            VectorSeries.LengthMode = mode;
            if (MagnitudeScaleHeader != null)
                MagnitudeScaleHeader.Visibility = mode == VectorLengthMode.MagnitudePixels ? Visibility.Visible : Visibility.Collapsed;
            if (MagnitudeScalePanel != null)
                MagnitudeScalePanel.Visibility = mode == VectorLengthMode.MagnitudePixels ? Visibility.Visible : Visibility.Collapsed;
            if (PixelLengthHeader != null)
                PixelLengthHeader.Visibility = mode == VectorLengthMode.FixedPixels ? Visibility.Visible : Visibility.Collapsed;
            if (PixelLengthPanel != null)
                PixelLengthPanel.Visibility = mode == VectorLengthMode.FixedPixels ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnColorModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VectorSeries == null || ColorModeCombo.SelectedItem == null) return;

            var mode = (VectorColorMode)ColorModeCombo.SelectedItem;
            VectorSeries.ColorMode = mode;
            VectorSeries.ColorMap = mode == VectorColorMode.ByDirection
                ? (HeatmapColorPalette)Resources["CoolWarmCircularPalette"]
                : (HeatmapColorPalette)Resources["CoolWarmLinearPalette"];

            switch (mode)
            {
                case VectorColorMode.ByMagnitude:
                    VectorSeries.ColorMap.Minimum = 0;
                    VectorSeries.ColorMap.Maximum = MagnitudeMax;
                    break;
                case VectorColorMode.ByDirection:
                    VectorSeries.ColorMap.Minimum = 0;
                    VectorSeries.ColorMap.Maximum = 2 * Math.PI;
                    break;
            }
        }

        private void OnGridSizeChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (GridSizeLabel == null) return;

            int xCount = (int)Math.Round(e.NewValue);
            int yCount = Math.Max(1, xCount * 2 / 3);
            GridSizeLabel.Text = $"{xCount} × {yCount}";

            // Replace the data series and backing arrays.
            // The background thread snapshots data references each iteration, so it safely
            // finishes with the old arrays while new writes target the new ones.
            _dataSeries = GenerateVectorFieldData(xCount, yCount);
            VectorSeries.DataSeries = _dataSeries;
        }
    }
}
