// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SpectrumAnalyzerExampleViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.SpectrumAnalyzer
{
    public class SpectrumAnalyzerExampleViewModel : BaseViewModel
    {
        private const int Count = 1024;

        private readonly double[] _re = new double[1024];
        private readonly double[] _im = new double[1024];

        private IXyDataSeries<double, double> _dataSeries;

        private DoubleRange _xVisibleRange;
        private DoubleRange _yVisibleRange;

        private Timer _updateTimer = new Timer(10);
        private readonly object _updateLocker = new object();

        private bool _isFrequencyDomain;
        private bool _isTimeDomain;

        private FFT2 _transform;
        private Random _random = new Random();

        private ICommand _startCommand;
        private ICommand _stopCommand;

        public SpectrumAnalyzerExampleViewModel()
        {
            _dataSeries = new XyDataSeries<double, double>();

            for (int i = 0; i < Count; i++)
                _im[i] = i;

            _dataSeries.Append(_im, _re);

            _transform = new FFT2();
            _transform.init(10);

            IsFrequencyDomain = true;

            _updateTimer.Elapsed += TimerElapsed;
            _updateTimer.AutoReset = true;

            _startCommand = new ActionCommand(OnExampleEnter);
            _stopCommand = new ActionCommand(OnExampleExit);
        }

        public ICommand StartCommand => _startCommand;
        public ICommand StopCommand => _stopCommand;

        public string YAxisTitle => IsTimeDomain ? "Voltage (V)" : "FFT(Voltage) (dB)";

        public DoubleRange YVisibleRange
        {
            get => _yVisibleRange;
            set
            {
                _yVisibleRange = value;
                OnPropertyChanged(nameof(YVisibleRange));
            }
        }

        public DoubleRange XVisibleRange
        {
            get => _xVisibleRange;
            set
            {
                _xVisibleRange = value;
                OnPropertyChanged(nameof(XVisibleRange));
            }
        }

        public IXyDataSeries<double, double> DataSeries
        {
            get => _dataSeries;
            set
            {
                _dataSeries = value;
                OnPropertyChanged(nameof(DataSeries));
            }
        }

        public bool IsFrequencyDomain
        {
            get => _isFrequencyDomain;
            set
            {
                if (_isFrequencyDomain == value)
                    return;

                _isFrequencyDomain = value;
                IsTimeDomain = !value;

                if (IsFrequencyDomain)
                {
                    // UpdateData() suspends updates internally
                    UpdateData();
                    ZoomExtentsY();
                    XVisibleRange = new DoubleRange(0, (Count / 2) - 1);
                }

                OnPropertyChanged(nameof(IsFrequencyDomain));
                OnPropertyChanged(nameof(YAxisTitle));
            }
        }

        public bool IsTimeDomain
        {
            get => _isTimeDomain;
            set
            {
                if (_isTimeDomain == value)
                    return;
                _isTimeDomain = value;
                IsFrequencyDomain = !value;

                if (IsTimeDomain)
                {
                    // UpdateData() suspends updates internally
                    UpdateData();
                    ZoomExtentsY();
                    XVisibleRange = new DoubleRange(0, Count - 1);
                }

                OnPropertyChanged(nameof(IsTimeDomain));
                OnPropertyChanged(nameof(YAxisTitle));
            }
        }

        private void ZoomExtentsY()
        {
            _dataSeries.InvalidateParentSurface(RangeMode.ZoomToFitY);
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            lock (_updateLocker)
            {                
                for (int i = 0; i < Count; i++)
                {
                    _re[i] = 2.0 * Math.Sin(2 * Math.PI * i / 20) +
                            5.0 * Math.Sin(2 * Math.PI * i / 10) +
                            2.0 * _random.NextDouble();
                    _im[i] = IsFrequencyDomain ? 0.0 : i;
                }

                if (IsFrequencyDomain)
                {
                    _transform.run(_re, _im);
                    for (int i = 0; i < Count; i++)
                    {
                        double mag = Math.Sqrt(_re[i] * _re[i] + _im[i] * _im[i]);
                        _re[i] = 20 * Math.Log10(mag / Count);
                        _im[i] = i;
                    }
                }

                using (_dataSeries.SuspendUpdates())
                {
                    _dataSeries.SeriesName = YAxisTitle;
                    var yValues = _dataSeries.YValues;
                    for (int i = 0; i < Count; i++)
                        yValues[i] = _re[i];
                }
            }
        }

        
        // These methods are just used to do tidy up when switching between examples
        public void OnExampleExit()
        {
            _updateTimer?.Stop();
        }

        public void OnExampleEnter()
        {
            _updateTimer?.Start();
        }

    }
}