namespace ChartProviders.Common.DataProviders
{
    public class MovingAverage
    {
        private readonly int _length;
        private int _circIndex = -1;
        private bool _filled;
        private double _current = double.NaN;
        private readonly double _oneOverLength;
        private readonly double[] _circularBuffer;
        private double _total;

        public int Length => _length;
        public double Current => _current;

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
            _total += value;
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
            _total += value;
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
    }
}