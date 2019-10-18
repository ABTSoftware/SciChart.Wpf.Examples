using System;
using System.Timers;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class NoLockTimer : IDisposable
    {
        private readonly Timer _timer;

        public NoLockTimer(TimeSpan interval, Action callback)
        {
            _timer = new Timer { AutoReset = false, Interval = interval.TotalMilliseconds };

            _timer.Elapsed += delegate
            {
                callback();

                _timer.Start();
            };
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}