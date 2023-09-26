using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace SciChart.Examples.Demo.Common
{
    public class PausableDispatcherTimer
    {
        private readonly Stopwatch _stopwatch;
        private readonly DispatcherTimer _timer;

        private readonly TimeSpan _interval;
        private readonly Action _tickAction;

        private bool _isTimerResumed;

        public bool IsPaused { get; private set; }

        public PausableDispatcherTimer(TimeSpan interval, Action tickAction)
        {
            _interval = interval;
            _tickAction = tickAction;

            _stopwatch = new Stopwatch();
            _timer = new DispatcherTimer { Interval = interval };
            _timer.Tick += OnTimerTick;
        }

        public void Start()
        {
            if (!_timer.IsEnabled)
            {
                IsPaused = false;

                _stopwatch.Reset();
                _stopwatch.Start();

                _timer.Start();
            }
        }

        public void Stop()
        {
            if (_timer.IsEnabled)
            {
                IsPaused = false;

                _stopwatch.Stop();
                _timer.Stop();
            }
        }

        public void Pause()
        {
            if (_timer.IsEnabled)
            {
                Stop();

                IsPaused = true;

                if (_interval.CompareTo(_stopwatch.Elapsed) > 0)
                {
                    _timer.Interval = _interval - _stopwatch.Elapsed;
                }
                else
                {
                    _timer.Interval = TimeSpan.FromMilliseconds(100);
                }
            }
        }

        public void Resume()
        {
            if (!_timer.IsEnabled && IsPaused)
            {
                _isTimerResumed = true;

                Start();
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            if (_isTimerResumed)
            {
                _isTimerResumed = false;
                Stop();

                _timer.Interval = _interval;
                Start();
            }

            _stopwatch.Reset();
            _stopwatch.Start();

            if (_timer.IsEnabled && !IsPaused)
            {
                _tickAction?.Invoke();
            }
        }
    }
}