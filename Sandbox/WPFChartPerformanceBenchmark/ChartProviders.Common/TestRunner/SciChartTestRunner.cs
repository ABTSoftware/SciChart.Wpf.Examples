using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ChartProviders.Common.TestRunner
{
    public interface ITestRunner : IDisposable
    {
        void Run();
        void OnSciChartRendered(object sender, EventArgs e);
    }

    public class SciChartTestRunner : ITestRunner
    {
        private Stopwatch _stopWatch;
        private int _frameCount;
        private TimeSpan _duration;
        private Action _testCallback;
        private Action<double> _completedCallback;

        public SciChartTestRunner(TimeSpan duration, Action testCallback, Action<double> completedCallback)
        {
            _duration = duration;
            _testCallback = testCallback;
            _completedCallback = completedCallback;
        }

        public void Run()
        {
            _stopWatch = Stopwatch.StartNew();
            RunNext(_duration, _testCallback, _completedCallback);
        }

        public void OnSciChartRendered(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RunNext(TimeSpan duration, Action testCallback, Action<double> completedCallback)
        {
            if (_stopWatch.ElapsedMilliseconds > duration.TotalMilliseconds)
            {
                _stopWatch.Stop();

                double fps = _frameCount/_stopWatch.Elapsed.TotalSeconds;
                completedCallback(fps);
                return;
            }

            _frameCount++;
            testCallback();
        }

        public void OnChartRendered(object sender, EventArgs e)
        {
            Debug.WriteLine("Chart was rendered");
            RunNext(_duration, _testCallback, _completedCallback);
        }

        public virtual void Dispose()
        {            
        }
    }
}
