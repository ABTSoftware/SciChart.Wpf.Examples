using System;
using System.Timers;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public class NoLockTimer : IDisposable
    {
        private Timer _timer;
        private Action _callback;

        public NoLockTimer(TimeSpan interval, Action callback)
        {
            _timer = new Timer { AutoReset = false, Interval = interval.TotalMilliseconds };
            _callback = callback;

            _timer.Elapsed += this.InternalCallback;
        }

        private void InternalCallback(object sender, ElapsedEventArgs e)
        {
            if (_callback != null)
            {
                _callback();
                _timer.Start();
            }
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
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Elapsed -= this.InternalCallback;
                _callback = null;
                _timer.Dispose();
                _timer = null;
            }
        }
    }
}