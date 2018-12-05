using System;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.Model.DataSeries.Waterfall;
using SciChart.Core.Helpers;
using SciChart.Examples.ExternalDependencies.Data;
using SciChart.Examples.ExternalDependencies.Helpers;

namespace SciChart.Examples.Examples.Charts3D.CreateRealtime3DCharts
{
    /// <summary>
    /// Interaction logic for RealtimeWaterfall3DChart.xaml
    /// </summary>
    public partial class RealtimeWaterfall3DChart : UserControl
    {
        // Data Sample Rate (sec) 
        private double dt = 0.02;

        // The current time
        private double t;

        private int _tick;
        private int _step = 3;

        private WaterfallDataSeries3D<double> _waterfallDataSeries;

        private readonly Random _random = new Random();

        private readonly FFT2 _transform;
        private int _transformSize;
        private double[] _real;
        private double[] _imaginary;

        private Timer _timer;
        private int _ticks = 16;

        private int _pointsPerSlice = 1024;
        private int _maxSliceCount = 20;

        private int _iteration = 0;
        private bool _isUpward = false;
        private DispatcherTimer _timerNewDataUpdate;

        public RealtimeWaterfall3DChart()
        {
            InitializeComponent();

            _random = new Random();
            _transform = new FFT2();            

            Loaded += OnLoaded;
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            var generatedRow = GenerateDataRow();

            _waterfallDataSeries.PushRow(generatedRow);
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            // Initialize FFT
            _transform.init((uint) Math.Log(_pointsPerSlice, 2));
            _transformSize = _pointsPerSlice * 2;
            _real = new double[_transformSize];
            _imaginary = new double[_transformSize];

            // Initialize WaterfallDataSeries3D
            _waterfallDataSeries = new WaterfallDataSeries3D<double>(_pointsPerSlice, _maxSliceCount);
            _waterfallDataSeries.StartX = 10;
            _waterfallDataSeries.StepX = 1;

            _waterfallDataSeries.StartZ = 25;
            _waterfallDataSeries.StepZ = 10;

            WaterfallSeries.DataSeries = _waterfallDataSeries;

            MeshSeries.DataSeries = (UniformGridDataSeries3D<double, double, double>) _waterfallDataSeries;
        }

        // NOTE: The purpose of this function is to simply generate some random data to display in the waterfall chart
        // Each row is _pointsPerSlice wide and there are _maxSliceCount slices in the waterfall 
        private double[] GenerateDataRow()
        {
            var generatedRow = new double[_pointsPerSlice];

            // Randomly introduce changes in amplitude
            _tick++;
            if (_tick == 2)
            {
                _tick = 0;
                _step = _random.Next(10, 20);
            }

            for (int i = 0; i < _transformSize; i++)
            {
                double noise = _random.Next(-100, 100) * 0.002;               

                // Compute a sinusoidal based waveform with some varying frequency and random amplitude 
                double y = _step * 2 * Math.Sin(((2 * Math.PI) *0.1) * t) + noise;
                y += Math.Sin(((2 * Math.PI) * 0.2) * t);

                _real[i] = y;
                _imaginary[i] = 0;
                t += dt;
            }

            // Do an FFT
            _transform.run(_real, _imaginary);

            // Convert FFT back to magnitude (required for a meaninful output in our test data)
            for (int i = 0; i < _pointsPerSlice; i++)
            {
                double magnitude = Math.Sqrt(_real[i] * _real[i] + _imaginary[i] * _imaginary[i]);
                generatedRow[i] = Math.Log10(magnitude);               
            }

            return generatedRow;
        }

        private void OnExampleLoaded(object sender, RoutedEventArgs e)
        {
            if (_timerNewDataUpdate == null)
            {
                _timerNewDataUpdate = new DispatcherTimer(DispatcherPriority.Render);
                _timerNewDataUpdate.Tick += OnTimerTick;
                _timerNewDataUpdate.Interval = TimeSpan.FromMilliseconds(25);
                _timerNewDataUpdate.Start();
            }
        }

        private void OnExampleUnloaded(object sender, RoutedEventArgs e)
        {
            if (_timerNewDataUpdate != null)
            {
                _timerNewDataUpdate.Stop();
                _timerNewDataUpdate = null;
            }
        }
    
        private void SelectedTypeChanged(object sender, SelectionChangedEventArgs e)
        {
            var isWaterfallRunning =  e.AddedItems[0].Equals("Waterfall");
            if (WaterfallSeries != null && MeshSeries != null)
            {
                WaterfallSeries.IsVisible = isWaterfallRunning;
                MeshSeries.IsVisible = !isWaterfallRunning;
                CTRLButton.IsEnabled = isWaterfallRunning;
            }
        }
    }
}
