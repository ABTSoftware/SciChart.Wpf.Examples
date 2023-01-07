// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AudioAnalyzerViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AudioAnalyzer
{
    public class AudioAnalyzerViewModel : BaseViewModel
    {
        private IXyDataSeries<int, double> _dataSeries;
        private IXyDataSeries<int, double> _frequencyDataSeries;
        private IDataSeries _uniformHeatmapDataSeries;
        private Dispatcher _dispatcher;

        private string _selectedDeviceId;

        private readonly AudioDeviceSource _audioDeviceSource = new AudioDeviceSource();
        private AudioDeviceHandler _audioDeviceHandler;
        private AudioDataAnalyzer _audioDataAnalyzer;

        public AudioAnalyzerViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            StartCommand = new ActionCommand(OnExampleEnter);
            StopCommand = new ActionCommand(OnExampleExit);
        }

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }

        public IXyDataSeries<int, double> DataSeries
        {
            get { return _dataSeries; }
            set
            {
                _dataSeries = value;
                OnPropertyChanged(nameof(DataSeries));
            }
        }

        public IXyDataSeries<int, double> FrequencyDataSeries
        {
            get { return _frequencyDataSeries; }
            set
            {
                _frequencyDataSeries = value;
                OnPropertyChanged(nameof(FrequencyDataSeries));
            }
        }

        public IDataSeries UniformHeatmapDataSeries
        {
            get { return _uniformHeatmapDataSeries; }
            set
            {
                _uniformHeatmapDataSeries = value;
                OnPropertyChanged(nameof(UniformHeatmapDataSeries));
            }
        }

        public ObservableCollection<AudioDeviceInfo> AvailableDevices => _audioDeviceSource.Devices;

        public string SelectedDeviceId
        {
            get { return _selectedDeviceId; }
            set
            {
                _selectedDeviceId = value;
                OnPropertyChanged(nameof(SelectedDeviceId));
                HandleInputDeviceChange(value);
            }
        }

        private string FindDefaultDevice()
        {
            string deviceId = _audioDeviceSource.DefaultDevice;
            if (deviceId == null)
            {
                deviceId = AvailableDevices.FirstOrDefault()?.ID;
            }

            return deviceId;
        }

        private void Init()
        {
            // The actual process is handled in SelectedDeviceId setter
            SelectedDeviceId = FindDefaultDevice();
        }

        private void HandleInputDeviceChange(string newId)
        {
            _audioDeviceHandler?.Dispose();
            _audioDeviceHandler = null;
            if (_audioDataAnalyzer != null)
            {
                _audioDataAnalyzer.Update -= Update;
                _audioDataAnalyzer = null;
            }

            if(newId == null)
            {
                newId = FindDefaultDevice();

                if (newId == null)
                {
                    MessageBox.Show("Unable to find capture device, please make sure that you have connected mic");
                    return;
                }
                SelectedDeviceId = newId;
                return;
            }
            _audioDeviceHandler = _audioDeviceSource.CreateHandlder(newId);
            _audioDataAnalyzer = new AudioDataAnalyzer(_audioDeviceHandler);
            _audioDataAnalyzer.Update += Update;
            InitDataStorage(_audioDataAnalyzer, _audioDeviceHandler);
            _audioDeviceHandler.Start();
        }

        private void Update(object sender, EventArgs e)
        {
            Update((AudioDataAnalyzer)sender);
        }

        private void Update(AudioDataAnalyzer analyzer)
        {
            using (_dataSeries.SuspendUpdates())
            {
                _dataSeries.Clear();
                _dataSeries.Append(analyzer.PrimaryIndices, analyzer.Samples);
            }

            using (_frequencyDataSeries.SuspendUpdates())
            {
                _frequencyDataSeries.Clear();
                _frequencyDataSeries.Append(analyzer.FftIndices, analyzer.DbValues);
            }

            _uniformHeatmapDataSeries.InvalidateParentSurface(RangeMode.None);
        }

        private void InitDataStorage(AudioDataAnalyzer analyzer, AudioDeviceHandler handler)
        {
            UniformHeatmapDataSeries = new UniformHeatmapDataSeries<int, int, double>(analyzer.SpectrogramBuffer, 0, analyzer.FftFrequencySpacing, 0, 1);

            var dataSeries = new XyDataSeries<int, double>(handler.BufferSize);
            dataSeries.Append(analyzer.PrimaryIndices, analyzer.Samples);
            DataSeries = dataSeries;

            var freqDataSeries = new XyDataSeries<int, double>(analyzer.FftDataPoints);
            freqDataSeries.Append(analyzer.FftIndices, analyzer.DbValues);
            FrequencyDataSeries = freqDataSeries;
        }

        // These methods are just used to do tidy up when switching between examples
        public void OnExampleExit()
        {
            _audioDeviceHandler?.Dispose();
            _dispatcher.ShutdownStarted -= DispatcherShutdownStarted;
        }

        public void OnExampleEnter()
        {
            // We must clean up the audio client (if any), otherwise it will block app shutdown
            _dispatcher.ShutdownStarted += DispatcherShutdownStarted;

            try
            {
                Init();
            }
            catch
            {
                MessageBox.Show("Unable to use default audio device");
            }
        }

        private void DispatcherShutdownStarted(object sender, EventArgs e)
        {
            OnExampleExit();
        }
    }
}