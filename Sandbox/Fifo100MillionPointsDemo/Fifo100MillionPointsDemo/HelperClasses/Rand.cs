using System;
using SciChart.UI.Bootstrap.Utility;

namespace Fifo100MillionPointsDemo.HelperClasses
{
    public class Rand
    {
        private static readonly Random Random = new Random();
        private float _current = 0.0f;

        public static float Next()
        {
            return (float)Random.NextDouble();
        }

        public float NextWalk()
        {
            // Random walk
            _current += (float)((Random.NextDouble() - 0.5) * 0.002);

            // Clamp to 0..1
            _current = Math.Max(Math.Min(_current, 1.0f), 0.0f);
            return _current;
        }

        public static byte NextByte(int min = 0, int max = 255)
        {
            return (byte)Random.Next(min,max);
        }
    }
}