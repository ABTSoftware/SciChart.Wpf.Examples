// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
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

namespace SciChart.Examples.ExternalDependencies.Data
{
    public class RandomWalkGenerator
    {
        private readonly Random _random;
        private double _last;
        private int _i;        
        private readonly double _bias = 0.01;

        public RandomWalkGenerator(double bias = 0.01)
        {
            _bias = bias;
            _random = new Random();
        }

        public RandomWalkGenerator(int seed)
        {
            _random = new Random(seed);
        }

        public void Reset()
        {
            _i = 0;
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
                _last = next;
                
                doubleSeries.Add(new XYPoint() { X = _i++, Y = next});
            }

            return doubleSeries;
        }
    }
}