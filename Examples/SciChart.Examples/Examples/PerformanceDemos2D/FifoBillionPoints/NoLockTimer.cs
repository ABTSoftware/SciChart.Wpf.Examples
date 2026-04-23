// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// NoLockTimer.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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