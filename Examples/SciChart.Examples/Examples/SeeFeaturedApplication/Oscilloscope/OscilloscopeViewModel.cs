// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// OscilloscopeViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Timers;
using System.Windows.Input;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;
using SciChart.Data.Numerics;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Oscilloscope
{
    public class OscilloscopeViewModel : BaseViewModel
    {
        private DoubleRange _xVisibleRange;
        private DoubleRange _yVisibleRange;

        private DoubleRange _xLimit;
        private DoubleRange _yLimit;

        private double _phase0;
        private double _phase1;
        private double _phaseIncrement;

        private bool _isRolloverSelected;
        private bool _isCursorSelected;

        private ResamplingMode _resamplingMode;
        private IXyDataSeries<double, double> _series0;
        private string _selectedDataSource;

        private bool _isDigitalLine = true;
        private bool _canExecuteRollover;

        private Timer _timer;
        private const double TimerIntervalMs = 20;

        public OscilloscopeViewModel()
        {
            // For chart data setup, see OnExampleEnter()
            StartCommand = new ActionCommand(OnExampleEnter);
            StopCommand = new ActionCommand(OnExampleExit);

            SetDigitalLineCommand = new ActionCommand(() => IsDigitalLine = !IsDigitalLine);
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public ICommand SetDigitalLineCommand { get; }

        public bool IsRolloverSelected
        {
            get => _isRolloverSelected;
            set
            {
                if (_isRolloverSelected != value)
                {
                    _isRolloverSelected = value;
                    OnPropertyChanged(nameof(IsRolloverSelected));
                }
            }
        }
        public bool IsCursorSelected
        {
            get => _isCursorSelected;
            set
            {
                if (_isCursorSelected != value)
                {
                    _isCursorSelected = value;
                    OnPropertyChanged(nameof(IsCursorSelected));
                }
            }
        }

        public bool CanExecuteRollover
        {
            get => _canExecuteRollover;
            set
            {
                if (_canExecuteRollover != value)
                {
                    _canExecuteRollover = value;
                    OnPropertyChanged(nameof(CanExecuteRollover));
                }
            }
        }

        public IXyDataSeries<double, double> ChartData
        {
            get => _series0;
            set
            {
                _series0 = value;
                OnPropertyChanged(nameof(ChartData));
            }
        }

        public string SelectedDataSource
        {
            get => _selectedDataSource;
            set
            {
                if (_selectedDataSource == value)
                {
                    return;
                }

                _selectedDataSource = value;

                lock (this)
                {
                    if (_selectedDataSource == "Lissajous")
                    {
                        // For Lissajous plots, we must use an UnsortedXyDataSeries 
                        // and we cannot use the Rollover. Currently HitTest/Rollover is not implemented
                        // for UnsortedXyDataSeries. Also this series type does not currently support resampling
                        _phaseIncrement = Math.PI * 0.02;
                        _series0 = new XyDataSeries<double, double>
                        {
                            AcceptsUnsortedData = true
                        };

                        IsDigitalLine = false;
                        SeriesResamplingMode = ResamplingMode.None;

                        CanExecuteRollover = false;
                        IsRolloverSelected = false;
                    }
                    else
                    {
                        // For FourierSeries plots, we can use the faster sorted XyDataSeries, 
                        // which supports the Rollover, HitTest and Resamplingd
                        _phaseIncrement = Math.PI * 0.1;
                        _series0 = new XyDataSeries<double, double>();

                        IsDigitalLine = true;
                        SeriesResamplingMode = ResamplingMode.MinMax;

                        CanExecuteRollover = true;
                    }

                    // Setup the Zoom Limit (affects double click to zoom extents)
                    ResetZoom();

                    // Add the new dataseries and reset counters. See OnTick where data is appended
                    _series0.SeriesName = _selectedDataSource;
                    _series0.Clear();

                    ChartData = _series0;

                    _phase0 = 0;
                    _phase1 = 0.15;
                }

                OnPropertyChanged(nameof(SelectedDataSource));
            }
        }

        private void ResetZoom()
        {
            if (_selectedDataSource == "Lissajous")
            {
                XLimit = new DoubleRange(-1.2, 1.2);
                YLimit = new DoubleRange(-1.2, 1.2);
            }
            else
            {
                XLimit = new DoubleRange(2.5, 4.5);
                YLimit = new DoubleRange(-12.5, 12.5);
            }

            XVisibleRange = (DoubleRange)XLimit.Clone();
            YVisibleRange = (DoubleRange)YLimit.Clone();
        }

        public bool IsDigitalLine
        {
            get => _isDigitalLine;
            set
            {
                if (_isDigitalLine != value)
                {
                    _isDigitalLine = value;
                    OnPropertyChanged(nameof(IsDigitalLine));
                }
            }
        }

        public DoubleRange XVisibleRange
        {
            get => _xVisibleRange;
            set
            {
                if (_xVisibleRange != value)
                {
                    _xVisibleRange = value;
                    OnPropertyChanged(nameof(XVisibleRange));
                }
            }
        }

        public DoubleRange YVisibleRange
        {
            get => _yVisibleRange;
            set
            {
                if (_yVisibleRange != value)
                {
                    _yVisibleRange = value;
                    OnPropertyChanged(nameof(YVisibleRange));
                }
            }
        }

        public DoubleRange XLimit
        {
            get => _xLimit;
            set
            {
                if (_xLimit != value)
                {
                    _xLimit = value;
                    OnPropertyChanged(nameof(XLimit));
                }
            }
        }

        public DoubleRange YLimit
        {
            get => _yLimit;
            set
            {
                if (_yLimit != value)
                {
                    _yLimit = value;
                    OnPropertyChanged(nameof(YLimit));
                }
            }
        }

        public ResamplingMode SeriesResamplingMode
        {
            get => _resamplingMode;
            set
            {
                if (_resamplingMode != value)
                {
                    _resamplingMode = value;
                    OnPropertyChanged(nameof(SeriesResamplingMode));
                }
            }
        }

        // Reset state when example exits
        public void OnExampleExit()
        {
            lock (this)
            {
                if (_timer != null)
                {
                    _timer.Stop();
                    _timer.Elapsed -= OnTick;
                    _timer = null;
                }

                // Null to clear memory 
                _xVisibleRange = null;
                _yVisibleRange = null;

                ChartData = null;
            }
        }

        // Setup start condition when the example enters
        public void OnExampleEnter()
        {
            ChartData = new XyDataSeries<double, double>();
            SelectedDataSource = "Fourier Series";
            IsCursorSelected = true;

            _timer = new Timer(TimerIntervalMs) { AutoReset = true };
            _timer.Elapsed += OnTick;
            _timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            lock (this)
            {
                // Generate data at this phase depending on data source type
                DoubleSeries dataSource = SelectedDataSource == "Lissajous"
                    ? DataManager.Instance.GetLissajousCurve(0.12, _phase1, _phase0, 2500)
                    : DataManager.Instance.GetFourierSeries(2.0, _phase0, 1000);

                _phase0 += _phaseIncrement;
                _phase1 += _phaseIncrement * 0.005;

                // Lock the data-series and clear / re-add new data
                using (ChartData.SuspendUpdates())
                {
                    _series0.Clear();
                    _series0.Append(dataSource.XData, dataSource.YData);
                }
            }
        }
    }
}