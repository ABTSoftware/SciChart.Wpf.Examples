using System.Security.Cryptography;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
{
    public class RandomGenerator
    {
        readonly RNGCryptoServiceProvider csp;

        public RandomGenerator()
        {
            csp = new RNGCryptoServiceProvider();
        }

        public void GenerateRandomBytes(byte[] buffer)
        {
            if (buffer != null && buffer.Length > 0)
            {
                csp.GetBytes(buffer);
            }
        }
    }
}
