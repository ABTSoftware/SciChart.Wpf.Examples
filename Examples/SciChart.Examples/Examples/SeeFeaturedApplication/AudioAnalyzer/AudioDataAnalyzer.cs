// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// AudioDataAnalyzer.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using NAudio.Dsp;
using System;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AudioAnalyzer
{
    class AudioDataAnalyzer
    {
        private readonly AudioDeviceHandler _handler;

        private int _fftWindowSize;
        private int _log2;
        private Complex[] _fftInput;
        public double[] DbValues { get; }
        public double[,] SpectrogramBuffer { get; }
        public const int SpectrogramFrameCount = 200;

        public int FftDataPoints { get; }
        public int FftFrequencySpacing { get; }

        //These are used to improve Append() perf
        public int[] PrimaryIndices { get; }
        public int[] FftIndices { get; }

        public double[] Samples => _handler.Samples;
        public double[] CurrentSamples => _handler.CurrentBuffer;

        public event EventHandler Update;

        public AudioDataAnalyzer(AudioDeviceHandler handler)
        {
            _handler = handler;

            // On Windows, sample rate could be pretty much anything and FFT requires power-of-2 window size
            // So we need to pick sufficient window size based on sample rate

            var sampleRate = (double)_handler.SamplesPerSecond;
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
            FftDataPoints = fftWindowSize / 2;
            FftFrequencySpacing = _handler.SamplesPerSecond / fftWindowSize;

            FftIndices = new int[FftDataPoints];
            for (int i = 0; i < FftIndices.Length; i++)
            {
                FftIndices[i] = i * FftFrequencySpacing;
            }

            SpectrogramBuffer = new double[SpectrogramFrameCount, FftDataPoints];
            for (int x = 0; x < SpectrogramFrameCount; x++)
            {
                for (int y = 0; y < FftDataPoints; y++)
                {
                    SpectrogramBuffer[x, y] = double.MinValue;
                }
            }
            DbValues = new double[FftDataPoints];

            PrimaryIndices = new int[handler.BufferSize];
            for (int i = 0; i < PrimaryIndices.Length; i++)
            {
                PrimaryIndices[i] = i;
            }

            handler.DataReceived += DataReceived;
        }

        private void DataReceived(object sender, EventArgs e)
        {
            ProcessData(_handler.Samples);
        }

        private void ProcessData(double[] input)
        {
            var offset = input.Length - _fftWindowSize;
            for (int i = 0; i < _fftWindowSize; i++)
            {
                Complex c = new Complex();
                c.X = (float)(input[offset + i] * FastFourierTransform.BlackmannHarrisWindow(i, _fftWindowSize));
                c.Y = 0;
                _fftInput[i] = c;
            }

            FastFourierTransform.FFT(true, _log2, _fftInput);

            ComputeDbValues(_fftInput, DbValues);

            Array.Copy(SpectrogramBuffer, FftDataPoints, SpectrogramBuffer, 0, (SpectrogramFrameCount - 1) * FftDataPoints);
            for (var i = 0; i < FftDataPoints; i++)
            {
                SpectrogramBuffer[SpectrogramFrameCount - 1, i] = DbValues[i];
            }

            Update?.Invoke(this, EventArgs.Empty);
        }

        private void ComputeDbValues(Complex[] compl, double[] tgt)
        {
            for (int i = 0; i < FftDataPoints; i++)
            {
                var c = compl[i];
                double mag = Math.Sqrt(c.X * c.X + c.Y * c.Y);
                var db = 20 * Math.Log10(mag);
                tgt[i] = db;
            }
        }
    }
}
