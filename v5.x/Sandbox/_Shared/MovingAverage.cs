using System;
using System.Collections.Generic;
using System.Linq;

namespace SciChart.Sandbox._Shared
{
    public static class MovingAverageExtensions
    {
        /// <summary>
        /// Fast MovingAverage LINQ Extension method provided for example purposes on an AS IS BASIS ONLY. ACCURACY OF CALCULATION NOT GAURANTEED
        /// </summary>
        public static IEnumerable<double> MovingAverage<T>(this IEnumerable<T> inputStream, Func<T, double> selector, int period)
        {
            var ma = new MovingAverage(period);
            foreach (var item in inputStream)
            {
                ma.Push(selector(item));
                yield return ma.Current;
            }
        }

        /// <summary>
        /// Fast MovingAverage LINQ Extension method provided for example purposes on an AS IS BASIS ONLY. ACCURACY OF CALCULATION NOT GAURANTEED
        /// </summary>
        public static IEnumerable<double> MovingAverage(this IEnumerable<double> inputStream, int period)
        {
            var ma = new MovingAverage(period);
            foreach (var item in inputStream)
            {
                ma.Push(item);
                yield return ma.Current;
            }
        }

        /// <summary>
        /// Fast Macd LINQ Extension method provided for example purposes on an AS IS BASIS ONLY. ACCURACY OF CALCULATION NOT GAURANTEED
        /// </summary>
        public static IEnumerable<MacdPoint> Macd(this IEnumerable<double> inputStream, int slow, int fast, int signal)
        {
            var maSlow = new MovingAverage(slow);
            var maFast = new MovingAverage(fast);
            var maSignal = new MovingAverage(signal);

            foreach (var item in inputStream)
            {
                double macd = maSlow.Push(item).Current - maFast.Push(item).Current;
                double signalLine = double.IsNaN(macd) ? double.NaN : maSignal.Push(macd).Current;
                double divergence = double.IsNaN(macd) || double.IsNaN(signalLine) ? double.NaN : macd - signalLine;

                yield return new MacdPoint() { Macd = macd, Signal = signalLine, Divergence = divergence };
            }
        }

        /// <summary>
        /// Fast Rsi LINQ Extension method provided for example purposes on an AS IS BASIS ONLY. ACCURACY OF CALCULATION NOT GAURANTEED
        /// </summary>
        public static IEnumerable<double> Rsi(this IEnumerable<PriceBar> inputStream, int period)
        {
            var averageGain = new MovingAverage(period);
            var averageLoss = new MovingAverage(period);

            var previous = inputStream.First();
            foreach (var item in inputStream.Skip(1))
            {
                double gain = item.Close > previous.Close ? item.Close - previous.Close : 0.0;
                double loss = previous.Close > item.Close ? previous.Close - item.Close : 0.0;

                averageGain.Push(gain);
                averageLoss.Push(loss);

                double relativeStrength = double.IsNaN(averageGain.Current) || double.IsNaN(averageLoss.Current) ? double.NaN : averageGain.Current / averageLoss.Current;

                previous = item;
                yield return double.IsNaN(relativeStrength) ? double.NaN : 100.0 - (100.0 / (1.0 + relativeStrength));
            }
        }
    }

    public struct MacdPoint
    {
        public double Macd;
        public double Signal;
        public double Divergence;
    }

    /// <summary>
    /// Fast Moving Average class provided for example purposes on an AS IS BASIS ONLY. ACCURACY OF CALCULATION NOT GAURANTEED
    /// </summary>
    public class MovingAverage
    {
        private readonly int _length;
        private int _circIndex = -1;
        private bool _filled;
        private double _current = double.NaN;
        private readonly double _oneOverLength;
        private readonly double[] _circularBuffer;
        private double _total;

        public MovingAverage(int length)
        {
            _length = length;
            _oneOverLength = 1.0 / length;
            _circularBuffer = new double[length];
        }

        public MovingAverage Update(double value)
        {
            double lostValue = _circularBuffer[_circIndex];
            _circularBuffer[_circIndex] = value;

            // Maintain totals for Push function
            // skip NaN 
            _total += double.IsNaN(value) ? 0d : value;
            _total -= lostValue;

            // If not yet filled, just return. Current value should be double.NaN
            if (!_filled)
            {
                _current = double.NaN;
                return this;
            }

            // Compute the average
            double average = 0.0;
            for (int i = 0; i < _circularBuffer.Length; i++)
            {
                average += _circularBuffer[i];
            }

            _current = average * _oneOverLength;

            return this;
        }

        public MovingAverage Push(double value)
        {
            // Apply the circular buffer
            if (++_circIndex == _length)
            {
                _circIndex = 0;
            }

            double lostValue = _circularBuffer[_circIndex];
            _circularBuffer[_circIndex] = value;

            // Compute the average
            // Skip NaN 
            _total += double.IsNaN(value) ? 0d : value;
            _total -= lostValue;

            // If not yet filled, just return. Current value should be double.NaN
            if (!_filled && _circIndex != _length - 1)
            {
                _current = double.NaN;
                return this;
            }
            else
            {
                // Set a flag to indicate this is the first time the buffer has been filled
                _filled = true;
            }

            _current = _total * _oneOverLength;

            return this;
        }

        public int Length { get { return _length; } }
        public double Current { get { return _current; } }
    }


    public class PriceBar
    {
        public PriceBar()
        {
        }

        public PriceBar(DateTime date, double open, double high, double low, double close, long volume)
        {
            DateTime = date;
            Open = open;
            High = high;
            Low = low;
            Close = close;
            Volume = volume;
        }

        public DateTime DateTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public long Volume { get; set; }
    }
}
