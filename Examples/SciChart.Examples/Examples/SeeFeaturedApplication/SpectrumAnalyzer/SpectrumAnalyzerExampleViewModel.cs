// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
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
        private bool _isFrequencyDomain;
        private bool _isTimeDomain;

        private FFT2 _transform;
        private Random _random = new Random();

        private ICommand _startCommand;
        private ICommand _stopCommand;

        public SpectrumAnalyzerExampleViewModel()
        {
            _dataSeries = new XyDataSeries<double, double>();

            _transform = new FFT2();
            _transform.init(10);

            IsFrequencyDomain = true;

            _updateTimer.Elapsed += TimerElapsed;
            _updateTimer.AutoReset = true;

            _startCommand = new ActionCommand(OnExampleEnter);
            _stopCommand = new ActionCommand(OnExampleExit);
        }

        public ICommand StartCommand { get { return _startCommand; } }
        public ICommand StopCommand { get { return _stopCommand; } }

        public string YAxisTitle
        {
            get { return IsTimeDomain ? "Voltage (V)" : "FFT(Voltage) (dB)"; }
        }

        public DoubleRange YVisibleRange
        {
            get { return _yVisibleRange; }
            set
            {
                _yVisibleRange = value;
                OnPropertyChanged("YVisibleRange");
            }
        }

        public DoubleRange XVisibleRange
        {
            get { return _xVisibleRange; }
            set
            {
                _xVisibleRange = value;
                OnPropertyChanged("XVisibleRange");
            }
        }

        public IXyDataSeries<double, double> DataSeries
        {
            get { return _dataSeries; }
            set
            {
                _dataSeries = value;
                OnPropertyChanged("DataSeries");
            }
        }

        public bool IsFrequencyDomain
        {
            get { return _isFrequencyDomain; }
            set
            {
                if (_isFrequencyDomain == value)
                    return;

                _isFrequencyDomain = value;
                IsTimeDomain = !value;

                if (IsFrequencyDomain)
                {
                    UpdateData();
                    ZoomExtents();
                    XVisibleRange = new DoubleRange(0, (Count / 2) - 1);
                }

                OnPropertyChanged("IsFrequencyDomain");
                OnPropertyChanged("YAxisTitle");
            }
        }

        public bool IsTimeDomain
        {
            get { return _isTimeDomain; }
            set
            {
                if (_isTimeDomain == value)
                    return;
                _isTimeDomain = value;
                IsFrequencyDomain = !value;

                if (IsTimeDomain)
                {
                    UpdateData();
                    ZoomExtents();
                    XVisibleRange = new DoubleRange(0, Count - 1);
                }

                OnPropertyChanged("IsTimeDomain");
                OnPropertyChanged("YAxisTitle");
            }
        }

        private void ZoomExtents()
        {
            _dataSeries.InvalidateParentSurface(RangeMode.ZoomToFitY);
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            UpdateData();
        }

        private void UpdateData()
        {
            lock (this)
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

                _dataSeries.SeriesName = YAxisTitle;
                _dataSeries.Clear();
                _dataSeries.Append(_im, _re);
            }
        }

        
        // These methods are just used to do tidy up when switching between examples
        public void OnExampleExit()
        {
            if (_updateTimer != null)
            {
                _updateTimer.Stop();
            }
        }

        public void OnExampleEnter()
        {
            if (_updateTimer != null)
            {
                _updateTimer.Start();
            }
        }

    }
}