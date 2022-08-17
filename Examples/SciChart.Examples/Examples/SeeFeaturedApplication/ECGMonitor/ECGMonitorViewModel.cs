// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ECGMonitorViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
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

namespace SciChart.Examples.Examples.SeeFeaturedApplication.ECGMonitor
{
    public class ECGMonitorViewModel : BaseViewModel
    {
        private Timer _timer;
        private readonly object _timerLock = new object();

        private IUniformXyDataSeries<double> _dataSeries;
        private readonly double[] _sourceData;

        private int _currentIndex;
        private double _totalTime;

        private DoubleRange _xVisibleRange;
        private DoubleRange _yVisibleRange;

        private int _heartRate;
        private bool _isBeat;
        private bool _lastBeat;
        private DateTime _lastBeatTime;

        private const double WindowSize = 5d;
        private const int TimerInterval = 20;
        private const int SampleRate = 400;

        public ECGMonitorViewModel()
        {
            // Create a data series. We use FIFO series as we
            // want to discard old data after 5000pts
            // At the sample rate of ~500Hz and 5 seconds
            // visible range we'll need 2500 points in the FIFO. 
            // We set 5000 so no data gets discarded while still in view
            _dataSeries = new UniformXyDataSeries<double>(0d, 1d / SampleRate) { FifoCapacity = 5000 };

            // Simulate waveform
            _sourceData = DataManager.Instance.LoadWaveformData();

            XVisibleRange = new DoubleRange(0d, WindowSize);
            YVisibleRange = new DoubleRange(-0.5, 1.5);

            StartCommand = new ActionCommand(OnExampleEnter);
            StopCommand = new ActionCommand(OnExampleExit);
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        /// <summary>
        /// SciChartSurface.DataSet binds to this
        /// </summary>
        public IUniformXyDataSeries<double> EcgDataSeries
        {
            get => _dataSeries;
            set
            {
                _dataSeries = value;
                OnPropertyChanged(nameof(EcgDataSeries));
            }
        }

        /// <summary>
        /// SciChartSurface.YAxis.VisibleRange binds to this
        /// </summary>
        public DoubleRange YVisibleRange
        {
            get => _yVisibleRange;
            set
            {
                _yVisibleRange = value;
                OnPropertyChanged(nameof(YVisibleRange));
            }
        }

        /// <summary>
        /// SciChartSurface.XAxis.VisibleRange binds to this
        /// </summary>
        public DoubleRange XVisibleRange
        {
            get => _xVisibleRange;
            set
            {
                if (!value.Equals(_xVisibleRange))
                {
                    _xVisibleRange = value;
                    OnPropertyChanged(nameof(XVisibleRange));
                }
            }
        }

        /// <summary>
        /// The heartbeat graphic binds to this, and changes its scale on heartbeat 
        /// </summary>
        public bool IsBeat
        {
            get => _isBeat;
            set
            {
                if (_isBeat != value)
                {
                    _isBeat = value;
                    OnPropertyChanged(nameof(IsBeat));
                }
            }
        }

        /// <summary>
        /// The heartrate textblock binds to this
        /// </summary>
        public int HeartRate
        {
            get => _heartRate;
            set
            {
                _heartRate = value;
                OnPropertyChanged(nameof(HeartRate));
            }
        }

        // These methods are just used to do tidy up when switching between examples
        public void OnExampleExit()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= TimerElapsed;
                _timer = null;
            }
        }

        public void OnExampleEnter()
        {
            _timer = new Timer(TimerInterval) { AutoReset = true };
            _timer.Elapsed += TimerElapsed;
            _timer.Start();
        }

        private void TimerElapsed(object sender, EventArgs e)
        {
            lock (_timerLock)
            {
                // As timer cannot tick quicker than ~20ms, we append 10 points
                // per tick to simulate a sampling frequency of 500Hz (e.g. 2ms per sample)
                for (int i = 0; i < 10; i++)
                {
                    AppendPoint();
                }

                // Assists heartbeat - it must show for 120ms before being deactivated
                if ((DateTime.Now - _lastBeatTime).TotalMilliseconds < 120d)
                {
                    return;
                }

                // Threshold the ECG voltage to determine if a heartbeat peak occurred
                IsBeat = _dataSeries.YValues[_dataSeries.Count - 3] > 0.5 ||
                         _dataSeries.YValues[_dataSeries.Count - 5] > 0.5 ||
                         _dataSeries.YValues[_dataSeries.Count - 8] > 0.5;

                // If so, compute the heart rate, update the last beat time
                if (IsBeat && !_lastBeat)
                {
                    HeartRate = (int)(60d / (DateTime.Now - _lastBeatTime).TotalSeconds);
                    _lastBeatTime = DateTime.Now;
                }
            }
        }

        private void AppendPoint()
        {
            if (_currentIndex >= _sourceData.Length)
            {
                _currentIndex = 0;
            }

            // Get the next voltage and time, and append to the chart
            double voltage = _sourceData[_currentIndex];
            _dataSeries.Append(voltage);

            // Calculate the next visible range
            ComputeXAxisRange(_totalTime);

            _lastBeat = IsBeat;
            _currentIndex++;
            _totalTime += 1d / SampleRate;
        }

        private void ComputeXAxisRange(double time)
        {
            if (time >= XVisibleRange.Max)
            {
                // Calculates a visible range. When the trace touches the right edge of the chart
                // (governed by WindowSize), shift the entire range 50% so that the trace is in the 
                // middle of the chart 
                double fractionSize = WindowSize * 0.5;
                double newMin = fractionSize * Math.Floor((time - fractionSize) / fractionSize);
                double newMax = newMin + WindowSize;

                XVisibleRange = new DoubleRange(newMin, newMax);
            }
        }
    }
}