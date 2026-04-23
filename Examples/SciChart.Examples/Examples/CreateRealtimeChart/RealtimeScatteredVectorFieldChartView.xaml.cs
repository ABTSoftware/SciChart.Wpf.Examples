// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// RealtimeScatteredVectorFieldChartView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    /// <see cref="NonUniformVectorFieldDataSeries"/> with arbitrarily-placed particles.
    /// Particles move along the flow field defined by three Rankine vortices (regularised point
    /// vortices), wrapping around the domain boundary when they leave it.
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
    public partial class RealtimeScatteredVectorFieldChartView : UserControl
    {
        private const double _vectorScale = 0.35;
        private const double DomainX = 15.0;
        private const double DomainY = 10.0;

        // Epsilon regularises the vortex core singularity; smaller values produce tighter cores
        private const double Epsilon = 1.5;

        // Dt is the Euler integration time step used to advect particles each frame
        private const double Dt = 0.025;

        // Interval between data updates in normal (non-performance) mode
        private const int UpdateIntervalMs = 20;

        // Three Rankine vortices: (centre X, centre Y, circulation strength Γ)
        // Negative Γ = clockwise rotation; positive Γ = counter-clockwise
        private static readonly (double Cx, double Cy, double Gamma)[] Vortices =
        {
            (-6.0,  4.5,  8.0),
            ( 7.0, -3.0, -6.5),
            ( 0.5, -6.0,  5.5),
        };

        // Names and corresponding XAML resource keys for the selectable colour palettes.
        // The palette gradient stops are declared as HeatmapColorPalette resources in XAML.
        private static readonly string[] PaletteNames = { "Plasma", "Cool-Warm" };
        private static readonly string[] PaletteResourceKeys = { "PlasmaColorPalette", "CoolWarmColorPalette" };
        private static readonly string[] CircularPaletteResourceKeys = { "PlasmaCircularColorPalette", "CoolWarmCircularColorPalette" };

        private static readonly FieldDataDistributionArgs DistributionArgs = new FieldDataDistributionArgs
        {
            XRange = new DoubleRange(-DomainX, DomainX),
            YRange = new DoubleRange(-DomainY, DomainY),
            IsStationary = false
        };

        // Double-buffered particle state to prevent rendering artifacts caused by the background
        // timer writing positions while SciChart reads them mid-frame.
        //
        // The timer writes exclusively to the _*Buf arrays (under _dataLock).
        // OnRenderingTick (UI thread) copies _*Buf → _px/_py/_dxs/_dys under the same lock,
        // then calls InvalidateParentSurface. SciChart always reads a fully consistent snapshot.
        private double[] _px;
        private double[] _py;
        private double[] _dxs;
        private double[] _dys;
        private double[] _pxBuf;
        private double[] _pyBuf;
        private double[] _dxsBuf;
        private double[] _dysBuf;
        private readonly object _dataLock = new object();

        private NonUniformVectorFieldDataSeries _dataSeries;
        private Timer _updateTimer;
        private bool _isRunning;
        private volatile bool _newDataReady;

        private bool _enablePerformanceShowMode;

        /// <summary>
        /// Initializes a new instance of <see cref="RealtimeScatteredVectorFieldChartView"/>.
        /// </summary>
        public RealtimeScatteredVectorFieldChartView()
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
            foreach (VectorColorMode mode in Enum.GetValues(typeof(VectorColorMode)))
                ColorModeCombo.Items.Add(mode);
            ColorModeCombo.SelectedItem = VectorColorMode.ByMagnitude;

            foreach (VectorLengthMode mode in Enum.GetValues(typeof(VectorLengthMode)))
                LengthModeCombo.Items.Add(mode);
            LengthModeCombo.SelectedItem = VectorLengthMode.MagnitudePixels;

            foreach (string name in PaletteNames)
                ColorMapCombo.Items.Add(name);
            ColorMapCombo.SelectedIndex = 0;

            OnStartClick(this, null);
        }

        private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        {
            OnStopClick(this, null);
            _updateTimer?.Dispose();

            _dataSeries?.Clear(true);
            _px = _py = _dxs = _dys = null;
            _pxBuf = _pyBuf = _dxsBuf = _dysBuf = null;
            ColorModeCombo.Items.Clear();
            LengthModeCombo.Items.Clear();
            ColorMapCombo.Items.Clear();
        }


        /// <summary>
        /// Allocates particle state arrays and creates the initial <see cref="NonUniformVectorFieldDataSeries"/>.
        /// Particles are scattered randomly within the domain using a fixed seed for reproducibility.
        /// </summary>
        private void InitializeParticles(int count)
        {
            var rng = new Random(12345);
            _pxBuf = new double[count];
            _pyBuf = new double[count];
            _dxsBuf = new double[count];
            _dysBuf = new double[count];

            for (int i = 0; i < count; i++)
            {
                _pxBuf[i] = (rng.NextDouble() * 2.0 - 1.0) * DomainX;
                _pyBuf[i] = (rng.NextDouble() * 2.0 - 1.0) * DomainY;
                GetFlowFieldVector(_pxBuf[i], _pyBuf[i], out double vx, out double vy);
                _dxsBuf[i] = vx * _vectorScale;
                _dysBuf[i] = vy * _vectorScale;
            }

            _px = (double[])_pxBuf.Clone();
            _py = (double[])_pyBuf.Clone();
            _dxs = (double[])_dxsBuf.Clone();
            _dys = (double[])_dysBuf.Clone();

            _dataSeries = new NonUniformVectorFieldDataSeries(_px, _py, _dxs, _dys);
            VectorSeries.DataSeries = _dataSeries;
        }

        /// <summary>
        /// Computes the 2-D velocity induced at (x, y) by the superimposed regularised vortices.
        /// Uses the Rankine (cut-off) vortex model: v = Γ / (2π (r² + ε²)) × r⊥.
        /// </summary>
        private static void GetFlowFieldVector(double x, double y, out double vx, out double vy)
        {
            vx = 0.0;
            vy = 0.0;
            foreach (var (cx, cy, gamma) in Vortices)
            {
                double rx = x - cx;
                double ry = y - cy;
                double r2 = rx * rx + ry * ry + Epsilon * Epsilon;
                vx -= gamma * ry / r2;
                vy += gamma * rx / r2;
            }
        }

        /// <summary>
        /// Wraps a scalar value into the periodic domain [min, max].
        /// Used to keep particles inside the visible domain after advection.
        /// </summary>
        private static double Wrap(double value, double min, double max)
        {
            double range = max - min;
            double normalized = (value - min) / range;
            return min + (normalized - Math.Floor(normalized)) * range;
        }

        /// <summary>
        /// Timer callback — fires every 20 ms on a thread-pool thread.
        /// Advances each particle by one Euler step, evaluates the flow field at the new
        /// position, then signals the rendering handler via <see cref="_newDataReady"/>.
        /// </summary>
        private void OnUpdateTimer(object state)
        {
            if (_dataSeries == null) return;

            int n = _pxBuf.Length;
            lock (_dataLock)
            {
                for (int i = 0; i < n; i++)
                {
                    // Advect particle position using current flow velocity (explicit Euler)
                    GetFlowFieldVector(_pxBuf[i], _pyBuf[i], out double vx, out double vy);
                    _pxBuf[i] = Wrap(_pxBuf[i] + vx * Dt, -DomainX, DomainX);
                    _pyBuf[i] = Wrap(_pyBuf[i] + vy * Dt, -DomainY, DomainY);

                    // Re-sample field at new position for the arrow direction
                    GetFlowFieldVector(_pxBuf[i], _pyBuf[i], out vx, out vy);
                    _dxsBuf[i] = vx * _vectorScale;
                    _dysBuf[i] = vy * _vectorScale;
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
                // No concurrent timer in this mode — update buf arrays then copy directly (no lock needed)
                OnUpdateTimer(null);
                Array.Copy(_pxBuf, _px, _px.Length);
                Array.Copy(_pyBuf, _py, _py.Length);
                Array.Copy(_dxsBuf, _dxs, _dxs.Length);
                Array.Copy(_dysBuf, _dys, _dys.Length);
                _dataSeries?.InvalidateParentSurface(RangeMode.None, DistributionArgs, hasDataChanges: true);
            }
            else
            {
                if (!_newDataReady) return;
                _newDataReady = false;
                lock (_dataLock)
                {
                    Array.Copy(_pxBuf, _px, _px.Length);
                    Array.Copy(_pyBuf, _py, _py.Length);
                    Array.Copy(_dxsBuf, _dxs, _dxs.Length);
                    Array.Copy(_dysBuf, _dys, _dys.Length);
                }
                _dataSeries.InvalidateParentSurface(RangeMode.None, DistributionArgs, hasDataChanges: true);
            }
        }

        private void OnStartClick(object sender, RoutedEventArgs e)
        {
            ChangeCountButton.IsEnabled = false;
            StartButton.IsEnabled = false;
            StartButton.IsChecked = true;
            StopButton.IsEnabled = true;
            StopButton.IsChecked = false;

            if (_dataSeries == null)
                InitializeParticles((int)ParticleCountSlider.Value);

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

            ChangeCountButton.IsEnabled = true;
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


        private void OnParticleCountUpdated(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Clear the existing data series so InitializeParticles creates a fresh one on next Start
            if (_dataSeries != null)
            {
                _dataSeries.Clear(true);
                _dataSeries.InvalidateParentSurface(RangeMode.None);
                _dataSeries = null;
            }
        }

        private void OnColorModeChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VectorSeries == null || ColorModeCombo.SelectedItem == null) return;
            var mode = (VectorColorMode)ColorModeCombo.SelectedItem;
            VectorSeries.ColorMode = mode;
            if (ColorMapCombo.SelectedItem != null)
                ApplyColorMapPreset((string)ColorMapCombo.SelectedItem);
            else
                UpdateColorMapRange(mode);
        }

        /// <summary>
        /// Adjusts the colour map range to match the selected colour mode.
        /// Direction mode maps [0, 2π]; magnitude mode maps [-1, 1] (normalised).
        /// </summary>
        private void UpdateColorMapRange(VectorColorMode mode)
        {
            if (VectorSeries?.ColorMap == null) return;

            VectorSeries.ColorMap.Minimum = mode == VectorColorMode.ByDirection ? 0 : -1;
            VectorSeries.ColorMap.Maximum = mode == VectorColorMode.ByDirection ? 2 * Math.PI : 1;
        }

        private void OnColorMapChanged(object sender, SelectionChangedEventArgs e)
        {
            if (VectorSeries == null || ColorMapCombo.SelectedItem == null) return;
            ApplyColorMapPreset((string)ColorMapCombo.SelectedItem);
        }

        /// <summary>
        /// Assigns the <see cref="HeatmapColorPalette"/> resource with the given preset name
        /// to the vector series, then updates the range for the current colour mode.
        /// Selects the circular variant when <c>ByDirection</c> is active so the hue wraps
        /// smoothly at 0°/360°; otherwise uses the linear variant.
        /// Palettes are declared as XAML resources in the view's resource dictionary.
        /// </summary>
        private void ApplyColorMapPreset(string name)
        {
            int idx = Array.IndexOf(PaletteNames, name);
            if (idx < 0) return;

            string key = VectorSeries.ColorMode == VectorColorMode.ByDirection
                ? CircularPaletteResourceKeys[idx]
                : PaletteResourceKeys[idx];
            VectorSeries.ColorMap = (HeatmapColorPalette)Resources[key];
            UpdateColorMapRange(VectorSeries.ColorMode);
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

    }
}
