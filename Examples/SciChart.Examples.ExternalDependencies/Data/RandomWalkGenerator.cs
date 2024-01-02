// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// RandomWalkGenerator.cs is part of SCICHART®, High Performance Scientific Charts
// For full terms and conditions of the license, see http://www.scichart.com/scichart-eula/
// 
// This source code is protected by international copyright law. Unauthorized
// reproduction, reverse-engineering, or distribution of all or any portion of
// this source code is strictly prohibited.
// 
// This source code contains confidential and proprietary trade secrets of
// SciChart Ltd., and should at no time be copied, transferred, sold,
// distributed or made available without express written permission.
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