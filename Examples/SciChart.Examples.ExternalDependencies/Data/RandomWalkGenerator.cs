// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// RandomWalkGenerator.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.Visuals.RenderableSeries.Animations;

namespace SciChart.Examples.ExternalDependencies.Data
{
    public class RandomWalkGenerator
    {
        private int _index;
        private double _last;

        private readonly Random _random = new Random();
        private readonly double _bias = 0.01;

        public RandomWalkGenerator(double bias = 0.01)
        {
            _bias = bias;

            if (!SeriesAnimationBase.GlobalEnableAnimations)
            {
                _random = new Random(0);
            }
        }

        public RandomWalkGenerator(int seed)
        {
            _random = new Random(seed);
        }

        public void Reset()
        {
            _index = 0;
            _last = 0;
        }

        public DoubleSeries GetRandomWalkSeries(int count)
        {
            var doubleSeries = new DoubleSeries(count);

            // Generate a slightly positive biased random walk
            // y[i] = y[i-1] + random, 
            // where random is in the range -0.5, +0.5
            for(int i = 0; i < count; i++)
            {
                double next = _last + (_random.NextDouble() - 0.5 + _bias);
                doubleSeries.Add(new XYPoint { X = _index++, Y = next});
                _last = next;
            }

            return doubleSeries;
        }

        public double[] GetRandomWalkYData(int count)
        {
            var doubleYData = new double[count];

            // Generate a slightly positive biased random walk
            // y[i] = y[i-1] + random, 
            // where random is in the range -0.5, +0.5
            for (int i = 0; i < count; i++)
            {
                double next = _last + (_random.NextDouble() - 0.5 + _bias);
                doubleYData[i] = next;
                _last = next;
            }

            return doubleYData;
        }

        public double GetRandomDouble()
        {
            return _random.NextDouble();
        }

        public int Next(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue);
        }
    }
}