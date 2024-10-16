using System.Collections.Generic;

namespace ChartProviders.Common.DataProviders
{
    public class BrownianMotionPointsGenerator : RandomPointsGenerator
    {
        private XyData _xyData;

        public BrownianMotionPointsGenerator(double xMin, double xMax, double yMin, double yMax) : base(xMin, xMax, yMin, yMax)
        {
        }

        public BrownianMotionPointsGenerator(int seed, double xMin, double xMax, double yMin, double yMax) : base(seed, xMin, xMax, yMin, yMax)
        {
        }

        public override XyData GetRandomPoints(int count)
        {
            if (_xyData == null)
            {
                _xyData = base.GetRandomPoints(count);
                return _xyData;
            }

            IList<double> xData = _xyData.XData;
            IList<double> yData = _xyData.YData;

            for (int i = 0; i < xData.Count; i++)
            {
                xData[i] = xData[i] + _random.NextDouble() - 0.5;
                yData[i] = yData[i] + _random.NextDouble() - 0.5;
            }

            return _xyData;
        }
    }
}
