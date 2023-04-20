using System.Security.Cryptography;

namespace SciChart_DigitalAnalyzerPerformanceDemo
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
