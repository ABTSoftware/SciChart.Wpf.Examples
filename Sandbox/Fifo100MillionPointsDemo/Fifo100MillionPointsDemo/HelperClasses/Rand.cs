using System;

namespace Fifo100MillionPointsDemo.HelperClasses
{
    public class Rand
    {
        private static Random _random = new Random();

        public static float Next()
        {
            return (float)_random.NextDouble();
        }

        public static byte NextByte(int min = 0, int max = 255)
        {
            return (byte)_random.Next(min,max);
        }
    }
}