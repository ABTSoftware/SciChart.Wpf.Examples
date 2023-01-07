﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChartProviders.Common.DataProviders
{
	public class CorrelatedDataPointsGenerator : RandomPointsGenerator
	{
		private XyData _xyData;

		public CorrelatedDataPointsGenerator(double xMin, double xMax, double yMin, double yMax)
			: base(xMin, xMax, yMin, yMax)
		{
		}

		public CorrelatedDataPointsGenerator(int seed, double xMin, double xMax, double yMin, double yMax)
			: base(seed, xMin, xMax, yMin, yMax)
		{
		}

		double MapX(double v1, double v2)
		{
			return _xMin + (_xMax - _xMin) * v1;
		}
		double MapY(double v1, double v2)
		{
			return _yMin + (_yMax - _yMin) * (v1 * 100 + v2*0.02/ (v1*10+0.1)) * 0.01;
		}

		public override XyData GetRandomPoints(int count)
		{
			if (_xyData == null)
				_xyData = base.GetRandomPoints(count);
			
			

			var xData = (IList<double>)_xyData.XData;
			var yData = (IList<double>)_xyData.YData;
			for (int i = 0; i < xData.Count; i++)
			{
				double v1 = _random.NextDouble(), v2 = _random.NextDouble();
				xData[i] = MapX(v1, v2);
				yData[i] = MapY(v1, v2);
			}

			return _xyData;
		}
	}
}
