using System;
using SciChart.UI.Bootstrap.Utility;

namespace Fifo100MillionPointsDemo.HelperClasses
{
    public class Rand
    {
        private static readonly Random StaticRandom = new Random();
        private float _current = 0.0f;

        private readonly Random InstanceRandom = new Random();

        public Rand(int? seed = null)
        {
            if (seed.HasValue)
            {
                InstanceRandom = new Random(seed.Value);
            }
        }

        public static float Next()
        {
            return (float)StaticRandom.NextDouble();
        }

        public float NextWalk()
        {
            // Random walk
            _current += (float)((InstanceRandom.NextDouble() - 0.5) * 0.002);

            // Clamp to 0..1
            _current = Math.Max(Math.Min(_current, 1.0f), 0.0f);
            return _current;
        }

        public static byte NextByte(int min = 0, int max = 255)
        {
            return (byte)StaticRandom.Next(min,max);
        }
    }
}