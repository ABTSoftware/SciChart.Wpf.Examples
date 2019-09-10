using System;
using System.Collections.Generic;
using System.Windows;

namespace ChartProviders.Common.DataProviders
{
    public class RandomWalkGenerator
    {
        private readonly Random _random;
        private double _last;
        private int _i;
        private const double Bias = 0;//0.001;

        public RandomWalkGenerator()
        {
            _random = new Random();
        }

        public RandomWalkGenerator(int seed)
        {
            _random = new Random(seed);
        }

        public XyData GetRandomWalkSeries(int count)
        {
            var doubleSeries = new XyData();
            var xData = new List<double>(count);
            var yData = new List<double>(count);

            doubleSeries.XData = xData;
            doubleSeries.YData = yData;

            // Generate a slightly positive biased random walk
            // y[i] = y[i-1] + random, 
            // where random is in the range -0.5, +0.5
            for (int i = 0; i < count; i++)
            {
                var next = Next();

                xData.Add(next.X);
                yData.Add(next.Y);
            }

            return doubleSeries;
        }

        public Point Next()
        {
            double next = _last + (_random.NextDouble() - 0.5 + Bias);
            _last = next;
            return new Point(_i++, next);
        }
    }
}