// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// BusyIndicatorController.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace SciChart.Examples.Demo.Controls.BusyIndicator
{
    /// <summary>
    /// Owns the busy indicator window and decides when it is actually on screen. Constructed on, and only ever touched
    /// from, the busy indicator dispatcher - which is why none of its state needs synchronization.
    /// </summary>
    internal sealed class BusyIndicatorController
    {
        private enum State
        {
            Hidden,
            Pending,
            Visible,
            MinVisibleHold
        }

        private const int OffScreenPosition = -30000;

        private readonly BusyIndicatorWindow _window;
        private readonly DispatcherTimer _showDelayTimer;
        private readonly DispatcherTimer _minVisibleTimer;
        private readonly DispatcherTimer _watchdogTimer;
        private readonly DispatcherTimer _foregroundTimer;
        private readonly Stopwatch _visibleFor = new Stopwatch();
        private readonly int _processId;

        private State _state = State.Hidden;
        private BusyAnchor _anchor;

        public BusyIndicatorController()
        {
            _processId = Process.GetCurrentProcess().Id;

            // Create the window (and therefore its HWND and layered surface) up front and park it off screen, so that
            // the first Show() does not have to pay for BAML parsing at the exact moment the app is about to freeze
            _window = new BusyIndicatorWindow { Left = OffScreenPosition, Top = OffScreenPosition };
            _window.Show();
            _window.Hide();

            _showDelayTimer = CreateTimer(BusyIndicatorSettings.ShowDelay, OnShowDelayElapsed);
            _minVisibleTimer = CreateTimer(BusyIndicatorSettings.MinVisible, OnMinVisibleElapsed);
            _watchdogTimer = CreateTimer(BusyIndicatorSettings.Watchdog, OnWatchdogElapsed);
            _foregroundTimer = CreateTimer(BusyIndicatorSettings.ForegroundPollInterval, OnForegroundPoll);
        }

        public void Show(BusyAnchor anchor)
        {
            _anchor = anchor;
            Restart(_watchdogTimer);

            switch (_state)
            {
                case State.Hidden:
                    // Do not show yet - most examples load fast enough that a spinner would only flicker
                    _state = State.Pending;
                    Restart(_showDelayTimer);
                    break;

                case State.Pending:
                    // Already counting down; keep the original deadline
                    break;

                case State.MinVisibleHold:
                    // A hide was pending, but we are busy again
                    _minVisibleTimer.Stop();
                    _state = State.Visible;
                    PositionWindow();
                    break;

                case State.Visible:
                    PositionWindow();
                    break;
            }
        }

        public void Hide(bool immediate)
        {
            switch (_state)
            {
                case State.Pending:
                    // Never became visible, so there is nothing to flicker
                    _showDelayTimer.Stop();
                    _watchdogTimer.Stop();
                    _state = State.Hidden;
                    break;

                case State.Visible:
                    var remaining = BusyIndicatorSettings.MinVisible - _visibleFor.Elapsed;
                    if (immediate || remaining <= TimeSpan.Zero)
                    {
                        HideNow();
                    }
                    else
                    {
                        // Keep it up briefly, so a spinner that just appeared does not vanish as a flash
                        _state = State.MinVisibleHold;
                        _minVisibleTimer.Interval = remaining;
                        Restart(_minVisibleTimer);
                    }
                    break;

                case State.MinVisibleHold:
                    if (immediate)
                    {
                        _minVisibleTimer.Stop();
                        HideNow();
                    }
                    break;

                case State.Hidden:
                    _watchdogTimer.Stop();
                    break;
            }
        }

        public void Close()
        {
            _showDelayTimer.Stop();
            _minVisibleTimer.Stop();
            _watchdogTimer.Stop();
            _foregroundTimer.Stop();

            _window.StopSpin();
            _window.Close();
        }

        private void OnShowDelayElapsed(object sender, EventArgs e)
        {
            _showDelayTimer.Stop();
            if (_state != State.Pending) return;

            _state = State.Visible;

            // Position while still hidden, so the ring never appears at a stale location first
            PositionWindow();

            _window.Show();
            _window.StartSpin();
            _visibleFor.Restart();

            Restart(_foregroundTimer);
        }

        private void OnMinVisibleElapsed(object sender, EventArgs e)
        {
            _minVisibleTimer.Stop();
            if (_state == State.MinVisibleHold)
            {
                HideNow();
            }
        }

        private void OnWatchdogElapsed(object sender, EventArgs e)
        {
            App.Log.DebugFormat("BusyIndicator: watchdog fired after {0}s, forcing the indicator to hide. " +
                                "A navigation completion hook probably did not run.",
                BusyIndicatorSettings.Watchdog.TotalSeconds);

            Hide(true);
        }

        /// <summary>
        /// The indicator window is topmost, so it would otherwise float over other applications if the user switches away
        /// while an example is loading. Both calls are non-blocking queries that never message the busy UI thread.
        /// </summary>
        private void OnForegroundPoll(object sender, EventArgs e)
        {
            if (_state != State.Visible && _state != State.MinVisibleHold) return;

            NativeMethods.GetWindowThreadProcessId(NativeMethods.GetForegroundWindow(), out var foregroundProcessId);

            _window.Visibility = foregroundProcessId == _processId ? Visibility.Visible : Visibility.Hidden;
        }

        private void HideNow()
        {
            _window.StopSpin();
            _window.Hide();

            _visibleFor.Reset();
            _foregroundTimer.Stop();
            _watchdogTimer.Stop();

            _state = State.Hidden;
        }

        private void PositionWindow()
        {
            var hwnd = new WindowInteropHelper(_window).Handle;
            if (hwnd == IntPtr.Zero) return;

            // Size in device pixels for the DPI of the monitor the main window is on, so that the content stays at the
            // intended device-independent size after WPF handles the DPI change
            var width = (int)Math.Round(BusyIndicatorSettings.RingSize * _anchor.DpiScaleX);
            var height = (int)Math.Round(BusyIndicatorSettings.RingSize * _anchor.DpiScaleY);

            NativeMethods.SetWindowPos(hwnd, NativeMethods.HWND_TOPMOST,
                _anchor.CentreXDevice - width / 2,
                _anchor.CentreYDevice - height / 2,
                width, height,
                NativeMethods.SWP_NOACTIVATE);
        }

        private static DispatcherTimer CreateTimer(TimeSpan interval, EventHandler onTick)
        {
            var timer = new DispatcherTimer { Interval = interval };
            timer.Tick += onTick;
            return timer;
        }

        private static void Restart(DispatcherTimer timer)
        {
            timer.Stop();
            timer.Start();
        }
    }
}
