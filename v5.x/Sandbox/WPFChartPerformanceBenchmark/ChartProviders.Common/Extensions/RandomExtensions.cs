using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChartProviders.Common.Extensions
{
    internal static class RandomExtensions
    {
        internal static double NextDouble(this Random random, double min, double max)
        {
            return random.NextDouble()*(max - min) + min;
        }
    }
}
