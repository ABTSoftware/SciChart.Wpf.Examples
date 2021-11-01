// *************************************************************************************
// SCICHART� Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// EEGExampleViewModel.cs is part of the SCICHART� Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART� examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Timers;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.Common.Helpers;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateRealtimeChart.EEGChannelsDemo
{
    public class EEGExampleViewModel : BaseViewModel
    {
        private ObservableCollection<EEGChannelViewModel> _channelViewModels;

        private readonly IList<Color> _colors = new[] 
        {
            Colors.White, Colors.Yellow, Color.FromArgb(255, 0, 128, 128), Color.FromArgb(255, 176, 196, 222), 
            Color.FromArgb(255, 255, 182, 193), Colors.Purple, Color.FromArgb(255, 245, 222, 179),Color.FromArgb(255, 173, 216, 230), 
            Color.FromArgb(255, 250, 128, 114), Color.FromArgb(255, 144, 238, 144), Colors.Orange, Color.FromArgb(255, 192, 192, 192), 
            Color.FromArgb(255, 255, 99, 71), Color.FromArgb(255, 205, 133, 63), Color.FromArgb(255, 64, 224, 208), Color.FromArgb(255, 244, 164, 96)
        };

        private readonly Random _random = new Random();
        private volatile int _currentSize;

        private const int ChannelCount = 50; // Number of channels to render
        private const int Size = 1000;       // Size of each channel in points (FIFO Buffer)

        private uint _timerInterval = 20; // Interval of the timer to generate data in ms        
        private int _bufferSize = 15;     // Number of points to append to each channel each timer tick

        private Timer _timer;        
        private readonly object _syncRoot = new object();

        private bool _running;
        private bool _isReset;

        private readonly ActionCommand _startCommand;
        private readonly ActionCommand _stopCommand;
        private readonly ActionCommand _resetCommand;

        public EEGExampleViewModel()
        {
            _startCommand = new ActionCommand(Start, () => !IsRunning);
            _stopCommand = new ActionCommand(Stop, () => IsRunning);
            _resetCommand = new ActionCommand(Reset, () => !IsRunning && !IsReset);         
        }

        public ObservableCollection<EEGChannelViewModel> ChannelViewModels
        {
            get => _channelViewModels;
            set 
            { 
                _channelViewModels = value;
                OnPropertyChanged("ChannelViewModels");
            }
        }        

        public ICommand StartCommand => _startCommand;
        public ICommand StopCommand => _stopCommand;
        public ICommand ResetCommand => _resetCommand;

        public int PointCount => _currentSize * ChannelCount;

        public double TimerInterval
        {
            get => _timerInterval;
            set
            {
                _timerInterval = (uint)value;
                OnPropertyChanged("TimerInterval");
                Stop();
            }
        }

        public double BufferSize
        {
            get => _bufferSize;
            set
            {
                _bufferSize = (int)value;
                OnPropertyChanged("BufferSize");
                Stop();
            }
        }

        public bool IsReset
        {
            get => _isReset;
            set
            {
                _isReset = value;

                _startCommand.RaiseCanExecuteChanged();
                _stopCommand.RaiseCanExecuteChanged();
                _resetCommand.RaiseCanExecuteChanged();

                OnPropertyChanged("IsReset");
            }
        }

        public bool IsRunning
        {
            get => _running;
            set 
            { 
                _running = value;

                _startCommand.RaiseCanExecuteChanged();
                _stopCommand.RaiseCanExecuteChanged();
                _resetCommand.RaiseCanExecuteChanged();

                OnPropertyChanged("IsRunning");                
            }
        }

        private void Start()
        {
            if (_channelViewModels == null || _channelViewModels.Count == 0)
            {
                Reset();
            }

            if (!IsRunning)
            {
                IsRunning = true;
                IsReset = false;

                _timer = new Timer(_timerInterval);
                _timer.Elapsed += OnTick;
                _timer.AutoReset = true;
                _timer.Start();
            }
        }

        private void Stop()
        {
            if (IsRunning)
            {
                _timer.Stop();
                IsRunning = false;
            }
        }

        private void Reset()
        {
            Stop();

            // Initialize N EEGChannelViewModels. Each of these will be represented as a single channel
            // of the EEG on the view. One channel = one SciChartSurface instance
            ChannelViewModels = new ObservableCollection<EEGChannelViewModel>();

            for (int i = 0; i < ChannelCount; i++)
            {
                var channelViewModel = new EEGChannelViewModel(Size, _colors[i % 16]) {ChannelName = "Channel " + i};
                ChannelViewModels.Add(channelViewModel);
            }

            IsReset = true;
        }

        private void OnTick(object sender, EventArgs e)
        {
            // Ensure only one timer Tick processed at a time
            lock (_syncRoot)
            {
                foreach (var channel in _channelViewModels)
                {
                    var dataSeries = channel.ChannelDataSeries;

                    using (dataSeries.SuspendUpdates())
                    {
                        // Add points 10 at a time for efficiency
                        for (int j = 0; j < BufferSize; j++)
                        {
                            // Append a new Y value in the random walk
                            dataSeries.Append(_random.NextDouble());
                        }
                    }

                    // For reporting current size to GUI
                    _currentSize = dataSeries.Count;
                }
            }
        }
    }
}