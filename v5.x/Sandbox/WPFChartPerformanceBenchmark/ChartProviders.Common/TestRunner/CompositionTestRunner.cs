using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace ChartProviders.Common.TestRunner
{
    public class CompositionTestRunner : TimerRunner
    {
        public CompositionTestRunner(
            TimeSpan duration, 
            Action testCallback, 
            Action<double> completedCallback) 
            : base(duration, testCallback, completedCallback)
        {
        }

        public override void Run()
        {
            EventHandler tickHandler = null;
            tickHandler = (s, e) =>
                 {
                     _testCallback();

                     if (_stopWatch.ElapsedMilliseconds > _duration.TotalMilliseconds)
                     {
                         _stopWatch.Stop();
                         CompositionTarget.Rendering -= tickHandler;
                         double fps = _frameCount / _stopWatch.Elapsed.TotalSeconds;
                         _completedCallback(fps);
                     }
                 };
            _stopWatch = Stopwatch.StartNew();
            CompositionTarget.Rendering += tickHandler;
        }
    }
}
