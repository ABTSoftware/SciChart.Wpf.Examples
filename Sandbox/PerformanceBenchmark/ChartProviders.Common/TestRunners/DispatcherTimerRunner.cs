using System;
using System.Diagnostics;
using System.Windows.Threading;

namespace ChartProviders.Common.TestRunners
{
    /// <summary>
    /// Executes a test each time a DispatcherTimer.Tick event fires
    /// </summary>
    public class DispatcherTimerRunner : TimerRunner
    {
        public DispatcherTimerRunner(TimeSpan duration, Action testCallback, Action<double> completedCallback) : base(duration, testCallback, completedCallback)
        {
        }

        /// <summary>
        /// Executes the given test each time the DispatcherTimer.Tick event fires. This
        /// is repeated for the given number of loops, with the result action being invoked to
        /// return the resultant framerate.
        /// </summary>
        public override void Run()
        {
            // NOTE: Uncomment this line to execute the Test Timer on a background thread. It makes a big difference to test performance
            // Why? Because Append is slow. Also DispatcherTimer is on the UI. It cannot run while the render takes place. 
            // I will profile the app with DispatcherTimer to see if there are any improvements. 
            // Meanwhile, you test with Threading.Timer as it will give you more accurate results

            // base.Run(); return;

            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(0)
            };

            void tickHandler(object s, EventArgs e)
            {
                _testCallback();

                if (_stopWatch.ElapsedMilliseconds > _duration.TotalMilliseconds)
                {
                    _stopWatch.Stop();
                    timer.Tick -= tickHandler;
                    timer.Stop();
                    double fps = _frameCount / _stopWatch.Elapsed.TotalSeconds;
                    _completedCallback(fps);
                }
            }

            timer.Tick += tickHandler;
            _stopWatch = Stopwatch.StartNew();
            timer.Start();
        }
    }
}