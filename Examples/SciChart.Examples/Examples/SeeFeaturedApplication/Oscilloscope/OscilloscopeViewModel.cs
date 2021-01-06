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

        private double _phase0 = 0.0;
        private double _phase1 = 0.0;
        private double _phaseIncrement;

        private ResamplingMode _resamplingMode;
        private IXyDataSeries<double, double> _series0;
        private string _selectedDataSource;

        private bool _isDigitalLine = true;

        private ModifierType _chartModifier;
        private bool _canExecuteRollover;

        private Timer _timer;
        private const double TimerIntervalMs = 20;

        private ICommand _startCommand;
        private ICommand _stopCommand;

        public OscilloscopeViewModel()
        {
            // For chart data setup, see  OnExampleEnter()
            _startCommand = new ActionCommand(OnExampleEnter);
            _stopCommand = new ActionCommand(OnExampleExit);
        }

        public ICommand StartCommand { get { return _startCommand; } }
        public ICommand StopCommand { get { return _stopCommand; } }

        public ActionCommand SetRolloverModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.Rollover)); } }
        public ActionCommand SetCursorModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.CrosshairsCursor)); } }
        public ActionCommand SetNullModifierCommand { get { return new ActionCommand(() => SetModifier(ModifierType.Null)); } }
        public ActionCommand SetDigitalLineCommand { get { return new ActionCommand(() => IsDigitalLine = !IsDigitalLine); } }

        public bool IsRolloverSelected { get { return ChartModifier == ModifierType.Rollover; }}
        public bool IsCursorSelected { get { return ChartModifier == ModifierType.CrosshairsCursor; } }

        public bool CanExecuteRollover
        {
            get { return _canExecuteRollover; }
            set
            {
                if (_canExecuteRollover == value) return;
                _canExecuteRollover = value;
                OnPropertyChanged("CanExecuteRollover");
            }
        }

        public IXyDataSeries<double, double> ChartData
        {
            get { return _series0; }
            set
            {
                _series0 = value;
                OnPropertyChanged("ChartData");
            }
        }

        public string SelectedDataSource
        {
            get { return _selectedDataSource; }
            set
            {
                if (_selectedDataSource == value) return;
                _selectedDataSource = value;

                lock (this)
                {
                    if (_selectedDataSource == "Lissajous")
                    {
                        // For Lissajous plots, we must use an UnsortedXyDataSeries 
                        // and we cannot use the Rollover. Currently HitTest/Rollover is not implemented
                        // for UnsortedXyDataSeries. Also this series type does not currently support resampling
                        _phaseIncrement = Math.PI * 0.02;
                        _series0 = new XyDataSeries<double, double> {AcceptsUnsortedData = true};
                        IsDigitalLine = false;
                        if (ChartModifier == ModifierType.Rollover)
                            SetModifier(ModifierType.CrosshairsCursor);
                        SeriesResamplingMode = ResamplingMode.None;                        
                    }
                    else
                    {
                        // For FourierSeries plots, we can use the faster sorted XyDataSeries, 
                        // which supports the Rollover, HitTest and Resamplingd
                        _phaseIncrement = Math.PI * 0.1;
                        _series0 = new XyDataSeries<double, double>();
                        IsDigitalLine = true;
                        CanExecuteRollover = true;
                        SeriesResamplingMode = ResamplingMode.MinMax;
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
                OnPropertyChanged("SelectedDataSource");
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

            XVisibleRange = (DoubleRange) XLimit.Clone();
            YVisibleRange = (DoubleRange) YLimit.Clone();
        }

        public bool IsDigitalLine
        {
            get { return _isDigitalLine; }
            set
            {
                if (_isDigitalLine == value) return;

                _isDigitalLine = value;
                OnPropertyChanged("IsDigitalLine");
            }
        }

        public DoubleRange XVisibleRange
        {
            get { return _xVisibleRange; }
            set 
            { 
                if (_xVisibleRange == value) return;

                _xVisibleRange = value;
                OnPropertyChanged("XVisibleRange");
            }
        }

        public DoubleRange YVisibleRange
        {
            get { return _yVisibleRange; }
            set
            {
                if (_yVisibleRange == value) return;

                _yVisibleRange = value;
                OnPropertyChanged("YVisibleRange");
            }
        }

        public ModifierType ChartModifier
        {
            get
            {
                return _chartModifier;
            }
            set
            {
                _chartModifier = value;
                OnPropertyChanged("ChartModifier");
                OnPropertyChanged("IsRolloverSelected");
                OnPropertyChanged("IsCursorSelected");
            }
        }

        public ResamplingMode SeriesResamplingMode
        {
            get { return _resamplingMode; }
            set
            {
                if (_resamplingMode == value) return;
                _resamplingMode = value;
                OnPropertyChanged("SeriesResamplingMode");
            }
        }
        

        private void SetModifier(ModifierType modifierType)
        {
            ChartModifier = modifierType;
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
            SetModifier(ModifierType.CrosshairsCursor);

            _timer = new Timer(TimerIntervalMs) { AutoReset = true };
            _timer.Elapsed += OnTick;
            _timer.Start();
        }

        private void OnTick(object sender, EventArgs e)
        {
            lock (this)
            {
                // Generate data at this phase depending on data source type
                var dataSource = SelectedDataSource == "Lissajous"
                                     ? DataManager.Instance.GetLissajousCurve(0.12, _phase1, _phase0, 2500)
                                     : DataManager.Instance.GetFourierSeries(2.0, _phase0, 1000);

                _phase0 += _phaseIncrement;
                _phase1 += _phaseIncrement*0.005;

                // Lock the data-series and clear / re-add new data
                using (this.ChartData.SuspendUpdates())
                {
                    _series0.Clear();
                    _series0.Append(dataSource.XData, dataSource.YData);
                }
            }
        }

        public DoubleRange XLimit
        {
            get { return _xLimit; }
            set
            {
                if (_xLimit == value) return;
                _xLimit = value;
                OnPropertyChanged("XLimit");
            }
        }

        public DoubleRange YLimit
        {
            get { return _yLimit; }
            set
            {
                if (_yLimit == value) return;
                _yLimit = value;
                OnPropertyChanged("YLimit");
            }
        }
    }
}
