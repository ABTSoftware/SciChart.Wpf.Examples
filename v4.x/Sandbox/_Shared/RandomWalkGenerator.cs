using System;
using System.Text;

namespace SciChart.Sandbox.Shared
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
            for (int i = 0; i < count; i++)
            {
                double next = _last + (_random.NextDouble() - 0.5 + _bias);
                _last = next;

                doubleSeries.Add(new XYPoint() { X = _i++, Y = next });
            }

            return doubleSeries;
        }
    }
}
