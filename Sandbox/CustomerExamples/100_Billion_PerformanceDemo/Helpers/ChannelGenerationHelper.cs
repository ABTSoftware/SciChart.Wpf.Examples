using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace SciChart_DigitalAnalyzerPerformanceDemo
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

            var args = new UniformDataDistributionArgs<byte>(false, 0, 1);
            var dataSeries = new UniformXyDataSeries<byte>(xStart, xStep, digitalData, args);

            return new ChannelViewModel(dataSeries, new DoubleRange(-0.5, 1.5), index, $"Channel {index}") { IsDigital = true };
        }
    }
}