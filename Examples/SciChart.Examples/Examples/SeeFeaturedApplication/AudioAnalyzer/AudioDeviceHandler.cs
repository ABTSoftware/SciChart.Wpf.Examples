// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AudioDeviceHandler.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using NAudio.CoreAudioApi;
using NAudio.Wave;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AudioAnalyzer
{
    class AudioDeviceHandler : IDisposable
    {
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private CancellationToken _token;
        private AutoResetEvent _processEvt = new AutoResetEvent(false);
        private WasapiCapture _capture;
        private MMDevice _device;
        private WaveFormat _waveFormat;
        private SampleReader _reader;
        private readonly double[] _input;
        private readonly double[] _inputBack;
        private double[] _currentBuffer;

        public int SamplesPerSecond { get; }
        public int BufferSize { get; }

        public double[] Samples => _inputBack;
        public double[] CurrentBuffer => _currentBuffer;

        public event EventHandler DataReceived;

        public AudioDeviceHandler(MMDevice device)
        {
            _token = _cts.Token;
            _device = device;

            _waveFormat = device.AudioClient.MixFormat.AsStandardWaveFormat();

            SamplesPerSecond = _waveFormat.SampleRate;

            BufferSize = SamplesPerSecond * 10;

            _input = new double[BufferSize];
            _inputBack = new double[BufferSize];
            _currentBuffer = new double[(int)(SamplesPerSecond * 0.1)];

            var capture = new WasapiCapture(device, false, 10) { WaveFormat = _waveFormat };
            capture.DataAvailable += DataAvailable;
            capture.RecordingStopped += RecordingStopped;

            _capture = capture;

            _reader = new SampleReader(_waveFormat);

            _ = Task.Run(ProcessData, _cts.Token);
        }

        public void Start()
        {
            _capture.StartRecording();
        }

        public void Stop()
        {
            _capture.StopRecording();
        }

        private void ProcessData()
        {
            while (!_token.IsCancellationRequested)
            {
                if (_processEvt.WaitOne(100))
                {
                    lock (_input)
                    {
                        Array.Copy(_input, _inputBack, _input.Length);
                        Array.Copy(_input, _input.Length - _currentBuffer.Length - 1, _currentBuffer, 0, _currentBuffer.Length);
                    }
                    DataReceived?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void RecordingStopped(object sender, StoppedEventArgs e)
        {
            if (_token.IsCancellationRequested)
            {
                _capture.Dispose();
                _device.Dispose();
            }
        }

        private void DataAvailable(object sender, WaveInEventArgs e)
        {
            if (e.BytesRecorded == 0) return;

            // We need to get off this thread ASAP to avoid losing frames
            lock (_input)
            {
                _reader.ReadSamples(e.Buffer, e.BytesRecorded, _input);
                _processEvt.Set();
            }
        }

        public void Dispose()
        {
            if (_capture.CaptureState == CaptureState.Stopped)
            {
                _capture.Dispose();
                _device.Dispose();
            }
            else
            {
                Stop();
            }
            _cts.Cancel();
            _cts.Dispose();

        }
    }
}
