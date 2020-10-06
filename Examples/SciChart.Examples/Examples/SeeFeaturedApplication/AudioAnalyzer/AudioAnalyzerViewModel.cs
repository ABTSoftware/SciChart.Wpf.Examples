// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using NAudio.CoreAudioApi;
using NAudio.Dsp;
using NAudio.Wave;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Core.Extensions;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AudioAnalyzer
{
    public class AudioAnalyzerViewModel : BaseViewModel
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private AutoResetEvent _processEvt = new AutoResetEvent(false);
        private int _fftWindowSize;
        private int _log2;
        private Complex[] _fftInput;
        private double[] _input;
        private double[] _inputBack;
        private double[] _dbValues;
        private double[,] _spectrogramBuffer;
        private const int SpectrogramFrameCount = 200;
        private IXyDataSeries<int, double> _dataSeries;
        private IXyDataSeries<int, double> _frequencyDataSeries;
        private IDataSeries _uniformHeatmapDataSeries;
        private int _fftDataPoints;
        private int _fftFrequencySpacing;
        private Dispatcher _dispatcher;

        //These are used to improve Append() perf
        private int[] _primaryIndices;
        private int[] _fftIndices;

        private Dictionary<string, MMDevice> _devices;
        private ObservableCollection<AudioEntry> _availableDevices;
        private MMDevice _selectedDevice;
        private string _selectedDeviceId;
        private WaveFormat _waveFormat;
        private WasapiCapture _capture;
        private SampleReader _reader;

        public AudioAnalyzerViewModel()
        {
            _dispatcher = Dispatcher.CurrentDispatcher;

            StartCommand = new ActionCommand(OnExampleEnter);
            StopCommand = new ActionCommand(OnExampleExit);
        }

        private static IEnumerable<MMDevice> GetCaptureDevices()
        {
            using (var enumerator = new MMDeviceEnumerator())
            { 
                var endpoints = enumerator.EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);
                foreach (var e in endpoints)
                {
                    yield return e;
                }
            }
        }

        private static MMDevice GetDefaultDevice()
        {
            using (var enumerator = new MMDeviceEnumerator())
            { 
                return enumerator.GetDefaultAudioEndpoint(DataFlow.Capture, Role.Communications);
            }
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

        public ObservableCollection<AudioEntry> AvailableDevices
        {
            get { return _availableDevices; }
            set
            {
                _availableDevices = value;
                OnPropertyChanged(nameof(AvailableDevices));
            }
        }

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

        private void CreateCapture()
        {
            _devices = GetCaptureDevices().ToDictionary(d => d.ID, d => d);

            AvailableDevices = new ObservableCollection<AudioEntry>(_devices.Values.Select(d => new AudioEntry(d)));

            string deviceId = null;
            var device = GetDefaultDevice();

            if (device != null)
            {
                deviceId = device.ID;
                device.Dispose();
            }

            if (deviceId == null)
            {
                deviceId = _devices.Values.FirstOrDefault()?.ID;
            }

            if (deviceId == null)
            {
                MessageBox.Show("Unable to find capture device, please make sure that you have connected mic");
                return;
            }

            // The actual process is handled in SelectedDeviceId setter
            SelectedDeviceId = deviceId;
        }

        private async void HandleInputDeviceChange(string newId)
        {
            await StopCapture(_capture);
            if (!_devices.TryGetValue(newId, out var device))
            {
                return;
            }

            _selectedDevice = device;
            _waveFormat = _selectedDevice.AudioClient.MixFormat;
            var capture = new WasapiCapture(_selectedDevice, false, 10) { WaveFormat = _waveFormat };
            capture.DataAvailable += DataAvailable;
            capture.RecordingStopped += RecordingStopped;

            _capture = capture;

            _reader = new SampleReader(_waveFormat);
            InitDataStorage();
            _capture.StartRecording();
            _ = Task.Run(ProcessData, _cts.Token);
        }

        private Task StopCapture(WasapiCapture capture)
        {
            if (capture == null || (capture.CaptureState == CaptureState.Stopping || capture.CaptureState == CaptureState.Stopped))
            {
                return Task.FromResult(true);
            }

            var tcs = new TaskCompletionSource<bool>();

            void Stopped(object sender, StoppedEventArgs e)
            {
                capture.RecordingStopped -= Stopped;
                tcs.SetResult(true);
            }

            capture.RecordingStopped += Stopped;
            capture.StopRecording();

            return tcs.Task;
        }

        private void InitDataStorage()
        {
            // On Windows, sample rate could be pretty much anything and FFT requires power-of-2 window size
            // So we need to pick sufficient window size based on sample rate

            var sampleRate = (double) _waveFormat.SampleRate;
            const double minLen = 0.05; // seconds

            var fftWindowSize = 512;
            _log2 = 9;

            while (fftWindowSize / sampleRate < minLen)
            {
                fftWindowSize *= 2;
                _log2 += 1;
            }

            _fftWindowSize = fftWindowSize;

            _fftInput = new Complex[_fftWindowSize];
            _fftDataPoints = fftWindowSize / 2;
            _fftFrequencySpacing = _waveFormat.SampleRate / fftWindowSize;

            _fftIndices = new int[_fftDataPoints];
            for(int i = 0; i < _fftDataPoints; i++)
            {
                _fftIndices[i] = i * _fftFrequencySpacing;
            }

            _spectrogramBuffer = new double[SpectrogramFrameCount, _fftDataPoints];
            for (int x = 0; x < SpectrogramFrameCount; x++)
            {
                for (int y = 0; y < _fftDataPoints; y++)
                {
                    _spectrogramBuffer[x, y] = double.MinValue;
                }
            }
            _dbValues = new double[_fftDataPoints];
            UniformHeatmapDataSeries = new UniformHeatmapDataSeries<int, int, double>(_spectrogramBuffer, 0, _fftFrequencySpacing, 0, 1);
            var windowSize = _waveFormat.SampleRate * 10; // 10 seconds
            _input = new double[windowSize];
            _inputBack = new double[windowSize];

            _primaryIndices = new int[windowSize];
            for (int i = 0; i < windowSize; i++)
            {
                _primaryIndices[i] = i;
            }

            var dataSeries = new XyDataSeries<int, double>(windowSize);
            dataSeries.Append(_primaryIndices, _input);
            DataSeries = dataSeries;

            var freqDataSeries = new XyDataSeries<int, double>(_fftDataPoints);
            freqDataSeries.Append(_fftIndices, _dbValues);
            FrequencyDataSeries = freqDataSeries;
        }

        private void RecordingStopped(object sender, StoppedEventArgs e)
        {
            var capture = sender as WasapiCapture;
            capture.Dispose();
            if (_cts.IsCancellationRequested)
            {
                CleanupDevices();
            }
        }

        private void CleanupDevices()
        {
            _devices.Values.ForEachDo(d => d.Dispose());
            _devices.Clear();
        }

        private void ProcessData()
        {
            while (!_cts.IsCancellationRequested)
            {
                if(_processEvt.WaitOne(100))
                {
                    lock (_input)
                    {
                        Array.Copy(_input, _inputBack, _input.Length);
                    }
                    ProcessDataInternal(_inputBack);
                }
            }
        }

        private void ProcessDataInternal(double[] input)
        {
            using (_dataSeries.SuspendUpdates())
            {
                _dataSeries.Clear();
                _dataSeries.Append(_primaryIndices, input);
            }

            var offset = input.Length - _fftWindowSize;
            for (int i = 0; i < _fftWindowSize; i++)
            {
                Complex c = new Complex();
                c.X = (float)(input[offset + i] * FastFourierTransform.BlackmannHarrisWindow(i, _fftWindowSize));
                c.Y = 0;
                _fftInput[i] = c;
            }

            FastFourierTransform.FFT(true, _log2, _fftInput);

            ComputeDbValues(_fftInput, _dbValues);

            using (_frequencyDataSeries.SuspendUpdates())
            {
                _frequencyDataSeries.Clear();
                _frequencyDataSeries.Append(_fftIndices, _dbValues);
            }

            using (_uniformHeatmapDataSeries.SuspendUpdates())
            {
                Array.Copy(_spectrogramBuffer, _fftDataPoints, _spectrogramBuffer, 0, (SpectrogramFrameCount - 1) * _fftDataPoints);
                for (var i = 0; i < _fftDataPoints; i++)
                {
                    _spectrogramBuffer[SpectrogramFrameCount - 1, i] = _dbValues[i];
                }
                _uniformHeatmapDataSeries.InvalidateParentSurface(RangeMode.None);
            }
        }

        private void DataAvailable(object sender, WaveInEventArgs e)
        {
            if(e.BytesRecorded == 0) return;

            // We need to get off this thread ASAP to avoid losing frames
            lock (_input)
            {
                _reader.ReadSamples(e.Buffer, e.BytesRecorded, _input);
                _processEvt.Set();
            }
        }

        private void ComputeDbValues(Complex[] compl, double[] tgt)
        {
            for (int i = 0; i < _fftDataPoints; i++)
            {
                var c = compl[i];
                double mag = Math.Sqrt(c.X * c.X + c.Y * c.Y);
                var db = 20 * Math.Log10(mag);
                tgt[i] = db;
            }
        }

        // These methods are just used to do tidy up when switching between examples
        public void OnExampleExit()
        {
            _cts.Cancel();
            _capture?.StopRecording();
            _dispatcher.ShutdownStarted -= DispatcherShutdownStarted;
        }

        public void OnExampleEnter()
        {
            // We must clean up the audio client (if any), otherwise it will block app shutdown
            _dispatcher.ShutdownStarted += DispatcherShutdownStarted;

            try
            {
                CreateCapture();
            }
            catch
            {
                MessageBox.Show("Unable to use selected audio device");
            }
        }

        private void DispatcherShutdownStarted(object sender, EventArgs e)
        {
            OnExampleExit();
        }

        private class SampleReader
        {
            private readonly int _bytesPerSample;
            private readonly int _bytesPerFrame;
            private readonly int _channels;

            public SampleReader(WaveFormat format)
            {
                _channels = format.Channels;
                _bytesPerSample = format.BitsPerSample / 8;
                _bytesPerFrame = format.Channels * _bytesPerSample;

                // We assume that Windows audio mixer is always using floats

                var standard = format.AsStandardWaveFormat();
                Debug.Assert(standard.Encoding == WaveFormatEncoding.IeeeFloat);
                Debug.Assert(standard.BitsPerSample == 32);
            }

            public int NumSamples(int numBytes)
            {
                return numBytes / _bytesPerFrame;
            }

            public void ReadSamples(byte[] data, int dataCount, double[] dest)
            {
                var size = dest.Length;
                var sampleCount = NumSamples(dataCount);
                sampleCount = Math.Min(size, sampleCount);

                var offset = size - sampleCount;

                Array.Copy(dest, sampleCount, dest, 0, offset);

                for (int i = 0; i < sampleCount; i++)
                {
                    dest[offset + i] = ReadSample(data, i);
                }
            }

            private double ReadSample(byte[] data, int idx)
            {
                double result = 0;
                var pos = idx * _bytesPerFrame;
                for (int i = 0; i < _channels; i++)
                {
                    var val = BitConverter.ToSingle(data, pos + _bytesPerSample * i);
                    result += val;
                }
                result /= _channels;
                return result;
            }
        }

        public class AudioEntry : BaseViewModel
        {
            private string _displayName;
            private string _id;

            public AudioEntry(MMDevice device)
            {
                _displayName = device.DeviceFriendlyName;
                _id = device.ID;
            }

            public string DisplayName
            {
                get { return _displayName; }
                set
                {
                    _displayName = value;
                    OnPropertyChanged(nameof(DisplayName));
                }
            }

            public string ID
            {
                get { return _id; }
                set
                {
                    _id = value;
                    OnPropertyChanged(nameof(ID));
                }
            }
        }
    }
}