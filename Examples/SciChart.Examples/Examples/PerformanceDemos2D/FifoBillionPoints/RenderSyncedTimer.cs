using System;
using System.Timers;
using SciChart.Drawing.Common;

namespace SciChart.Examples.Examples.PerformanceDemos2D.FifoBillionPoints
{
    public sealed class RenderSyncedTimer : IDisposable
    {
        private readonly Timer _timer;
        private readonly Action _callback;

        private readonly IRenderSurface _renderSurface;
        private readonly object _syncLock = new object();

        private bool _isStopped = true;
        private bool _isRendering;

        public bool InvalidateAfterCallback { get; set; }

        public RenderSyncedTimer(TimeSpan interval, IRenderSurface surface, Action callback)
        {
            _timer = new Timer
            {
                AutoReset = false,
                Interval = interval.TotalMilliseconds
            };

            _callback = callback;
            _renderSurface = surface;

            _timer.Elapsed += OnElapsed;
        }

        private void OnElapsed(object sender, ElapsedEventArgs e)
        {
            lock (_syncLock)
            {
                if (_isRendering) return;

                _isRendering = true;

                _callback?.Invoke();

                if (InvalidateAfterCallback)
                {
                    _renderSurface.InvalidateElement();
                }
            }
        }

        private void OnRendered(object sender, RenderedEventArgs e)
        {
            lock (_syncLock)
            {
                _isRendering = false;

                if (!_isStopped)
                {
                    _timer.Start();
                }
            }
        }

        public void Start()
        {
            lock (_syncLock)
            {
                if (_isStopped)
                {
                    _renderSurface.Rendered += OnRendered;
                    _isStopped = false;
                    _timer.Start();
                }
            }
        }

        public void Stop()
        {
            lock (_syncLock)
            {
                if (!_isStopped)
                {
                    _renderSurface.Rendered -= OnRendered;
                    _isStopped = true;
                    _timer.Stop();
                }
            }
        }

        public void Dispose()
        {
            _renderSurface.Rendered -= OnRendered;
            _timer.Elapsed -= OnElapsed;
            _timer.Dispose();
        }
    }
}