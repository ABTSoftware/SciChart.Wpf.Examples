// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SampleReader.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Diagnostics;
using NAudio.Wave;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.AudioAnalyzer
{
    class SampleReader
    {
        private readonly int _bytesPerSample;
        private readonly int _bytesPerFrame;
        private readonly int _channels;

        public SampleReader(WaveFormat format)
        {
            // We assume that Windows audio mixer is always using floats

            Debug.Assert(format.Encoding == WaveFormatEncoding.IeeeFloat);
            Debug.Assert(format.BitsPerSample == 32);

            _channels = format.Channels;
            _bytesPerSample = format.BitsPerSample / 8;
            _bytesPerFrame = format.Channels * _bytesPerSample;
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
}