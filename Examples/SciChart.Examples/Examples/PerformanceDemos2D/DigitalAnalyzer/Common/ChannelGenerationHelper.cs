// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ChannelGenerationHelper.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
{
    public class ChannelGenerationHelper
    {
        public static ChannelGenerationHelper Instance = new ChannelGenerationHelper();
        RandomGenerator _rand = new RandomGenerator();

        private ChannelGenerationHelper() { }

        public ChannelViewModel GenerateDigitalChannel(double xStart, double xStep, byte[] digitalData, int index = 0)
        {
            _rand.GenerateRandomBytes(digitalData);
            var count = digitalData.Length;

            for (int i = 0; i < count; i += 2)
            {
                // Get random bits by using first bit
                var bit = (byte)(digitalData[i] >> 7);

                digitalData[i] = bit;
                digitalData[i + 1 >= count ? i : i + 1] = bit;
            }

            // Provide additional info about expected data to avoid runtime checks
            // There are frequent peaks at 0,1
            var args = new UniformDataDistributionArgs<byte>(false, 0, 1);
            var dataSeries = new UniformXyDataSeries<byte>(xStart, xStep, digitalData, args);

            return new ChannelViewModel(dataSeries, new DoubleRange(-0.5, 1.5), index, $"Channel {index}") { IsDigital = true };
        }

        public ChannelViewModel GenerateAnalogChannel(double xStart, double xStep, float[] analogData, int index = 0)
        {
            UniformDataManager.GenerateAnalogData(analogData);

            // Provide additional info about expected data to avoid runtime checks
            // There are frequent peaks at 0,1
            var args = new UniformDataDistributionArgs<float>(false, -1f, 1f);
            var dataSeries = new UniformXyDataSeries<float>(xStart, xStep, analogData, args);

            return new ChannelViewModel(dataSeries, new DoubleRange(-1.5, 1.5), index, $"Channel {index}") { IsDigital = false };
        }
    }
}