using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using ChartProviders.Common.Extensions;

namespace ChartProviders.Common.DataProviders
{
    public class RandomPointsGenerator
    {
        protected readonly double _xMin;
		protected readonly double _xMax;
		protected readonly double _yMin;
		protected readonly double _yMax;
        protected readonly Random _random;
        private int _i;

        public RandomPointsGenerator(double xMin, double xMax, double yMin, double yMax)
        {
            _xMin = xMin;
            _xMax = xMax;
            _yMin = yMin;
            _yMax = yMax;
            _random = new Random();
        }

        public RandomPointsGenerator(int seed, double xMin, double xMax, double yMin, double yMax)
        {
            _xMin = xMin;
            _xMax = xMax;
            _yMin = yMin;
            _yMax = yMax;
            _random = new Random(seed);
        }

        public virtual XyData GetRandomPoints(int count)
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
            double nextX = _random.NextDouble(_xMin, _xMax);
            double nextY = _random.NextDouble(_yMin, _yMax);
            return new Point(nextX, nextY);
        }
    }
}
