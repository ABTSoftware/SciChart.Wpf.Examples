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
            var dataSeries = new UniformXyDataSeries<float>(xStart, xStep, analogData);

            return new ChannelViewModel(dataSeries, new DoubleRange(-1.5, 1.5), index, $"Channel {index}") { IsDigital = false };
        }
    }
}