// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// Rand.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class Rand
    {
        private static readonly Random StaticRandom = new Random();

        private float _current;

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