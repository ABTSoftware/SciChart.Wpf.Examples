using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SciChart.Sandbox.Shared;

namespace SciChart.Sandbox.Shared
{
    public class DataManager
    {
        private static readonly DataManager _instance = new DataManager();

        public static DataManager Instance
        {
            get { return _instance; }
        }

        public DoubleSeries GetDampedSinewave(double amplitude, double dampingFactor, int pointCount, int freq = 10)
        {
            return GetDampedSinewave(0, amplitude, 0.0, dampingFactor, pointCount, freq);
        }

        public DoubleSeries GetDampedSinewave(int pad, double amplitude, double phase, double dampingFactor, int pointCount, int freq = 10)
        {
            var doubleSeries = new DoubleSeries();

            for (int i = 0; i < pad; i++)
            {
                double time = 10 * i / (double)pointCount;
                doubleSeries.Add(new XYPoint() { X = time });
            }

            for (int i = pad, j = 0; i < pointCount; i++, j++)
            {
                var xyPoint = new XYPoint();

                double time = 10 * i / (double)pointCount;
                double wn = 2 * Math.PI / (pointCount / (double)freq);

                xyPoint.X = time;
                xyPoint.Y = amplitude * Math.Sin(j * wn + phase);
                doubleSeries.Add(xyPoint);

                amplitude *= (1.0 - dampingFactor);
            }

            return doubleSeries;
        }
    }
}
