using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace ChartProviders.Common.DataProviders
{
    public class RandomLinesGenerator
    {
        private readonly Random _random;
        private int _i;

        public RandomLinesGenerator()
        {
            _random = new Random();
        }

        public RandomLinesGenerator(int seed)
        {
            _random = new Random(seed);
        }

        public XyData GetRandomLinesSeries(int count)
        {
            var doubleSeries = new XyData();
            doubleSeries.XData = new List<double>();
            doubleSeries.YData = new List<double>();

            // Generate a slightly positive biased random walk
            // y[i] = y[i-1] + random, 
            // where random is in the range -0.5, +0.5
            for (int i = 0; i < count; i++)
            {
                var next = Next();

                ((IList<double>)doubleSeries.XData).Add(next.X);
                ((IList<double>)doubleSeries.YData).Add(next.Y);
            }

            return doubleSeries;
        }

        public Point Next()
        {
            double next = _random.NextDouble();
            return new Point(_i++, next);
        }
    }
}
