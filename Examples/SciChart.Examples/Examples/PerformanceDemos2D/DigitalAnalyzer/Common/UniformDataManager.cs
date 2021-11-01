using System;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
{
    public static class UniformDataManager
    {
        public static void GenerateAnalogData(double[] buffer)
        {
            var count = buffer.Length;
            var freq = count / 100;

            var amp = 1d;
            var phase = 0d;

            for (int i = 0, j = 0; i < count; i++, j++)
            {
                var wn = 2 * Math.PI / (count / (double)freq);
                buffer[i] = amp * Math.Sin(j * wn + phase);
            }
        }
    }
}