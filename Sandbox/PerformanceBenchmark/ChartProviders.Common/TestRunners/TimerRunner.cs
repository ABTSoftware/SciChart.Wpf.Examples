using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;

namespace ChartProviders.Common.TestRunners
{
    public class TimerRunner : ITestRunner
    {
        protected TimeSpan _duration;
        protected Action _testCallback;
        protected Action<double> _completedCallback;
        protected Stopwatch _stopWatch;
        protected int _frameCount;
        private bool _stopped;
        private Timer timer;

        public TimerRunner(TimeSpan duration, Action testCallback, Action<double> completedCallback)
        {
            _duration = duration;
            _testCallback = testCallback;
            _completedCallback = completedCallback;
        }

        public virtual void Run()
        {
            timer = new Timer
            {
                Interval = 10 // 100Hz
            };

            void tickHandler(object s, ElapsedEventArgs e)
            {
                _testCallback();

                lock (this)
                {
                    if (_stopped) return;
                    if (_stopWatch.ElapsedMilliseconds > _duration.TotalMilliseconds)
                    {
                        _stopWatch.Stop();
                        _stopped = true;
                        timer.Elapsed -= tickHandler;
                        timer.Stop();
                        double fps = _frameCount / _stopWatch.Elapsed.TotalSeconds;
                        Application.Current.Dispatcher.BeginInvoke(new Action(() => _completedCallback(fps)));
                    }
                }
            }

            timer.Elapsed += tickHandler;
            _stopWatch = Stopwatch.StartNew();
            timer.Start();
        }

        public void OnSciChartRendered(object sender, EventArgs e)
        {
            _frameCount++;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                timer?.Stop();
                timer = null;
            }
        }
    }
}