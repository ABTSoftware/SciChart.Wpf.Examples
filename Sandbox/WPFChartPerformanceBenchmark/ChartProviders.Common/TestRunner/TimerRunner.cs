using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;

namespace ChartProviders.Common.TestRunner
{
    /// <summary>
    /// Executes a test each time a DispatcherTimer.Tick event fires
    /// </summary>
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

        /// <summary>
        /// Executes the given test each time the DispatcherTimer.Tick event fires. This
        /// is repeated for the given number of loops, with the result action being invoked to
        /// return the resultant framerate.
        /// </summary>
        public virtual void Run()
        {
            timer = new Timer();
            timer.Interval = 10; // 100Hz
            //timer.Interval = 1; // 100Hz

            ElapsedEventHandler tickHandler = null;
            tickHandler = (s, e) =>
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
                            double fps = _frameCount/_stopWatch.Elapsed.TotalSeconds;
                            Application.Current.Dispatcher.BeginInvoke(new Action(() => _completedCallback(fps)));
                        }
                    }
                };

            timer.Elapsed += tickHandler;
            _stopWatch = Stopwatch.StartNew();
            timer.Start();
        }

        public void OnSciChartRendered(object sender, EventArgs e)
        {
            _frameCount++;
        }

        public virtual void Dispose()
        {
            if (timer != null)
            {
                timer.Stop();
                timer = null;
            }
        }
    }
}