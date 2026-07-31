// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// BusyIndicatorService.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SciChart.Examples.Demo.Controls.BusyIndicator
{
    internal static class BusyIndicatorSettings
    {
        /// <summary>How long an operation must run before the indicator appears at all.</summary>
        public static readonly TimeSpan ShowDelay = TimeSpan.FromMilliseconds(250);

        /// <summary>How long the indicator stays up once shown, so that it never appears as a flash.</summary>
        public static readonly TimeSpan MinVisible = TimeSpan.FromMilliseconds(300);

        /// <summary>
        /// Fail-safe: hides the indicator if a completion hook never runs, which happens when a Show() is not followed by
        /// an actual navigation - for example when the user re-selects the example that is already open. Kept comfortably
        /// longer than any real example load, but short enough that a missed hook is not left on screen.
        /// </summary>
        public static readonly TimeSpan Watchdog = TimeSpan.FromSeconds(8);

        public static readonly TimeSpan ForegroundPollInterval = TimeSpan.FromMilliseconds(250);

        /// <summary>Size of the ring, in device-independent pixels.</summary>
        public const double RingSize = 64.0;
    }

    /// <summary>
    /// Shows a rotating busy indicator while the application is doing work that blocks the UI thread, such as loading or
    /// switching examples.
    /// <para>
    /// The indicator runs on its own STA thread with its own <see cref="Dispatcher"/>. This is the whole point: WPF
    /// animation clocks are driven per dispatcher, so an indicator hosted in the main visual tree would freeze at exactly
    /// the moment it is needed. Calls from the UI thread are always posted with BeginInvoke and never block.
    /// </para>
    /// </summary>
    public sealed class BusyIndicatorService
    {
        private static readonly BusyIndicatorService _instance = new BusyIndicatorService();

        private readonly object _syncRoot = new object();

        private volatile Dispatcher _dispatcher;
        private volatile bool _isShutdown;
        private Thread _thread;

        // Owned by the indicator thread
        private BusyIndicatorController _controller;

        private BusyIndicatorService()
        {
            IsEnabled = true;
        }

        public static BusyIndicatorService Instance => _instance;

        /// <summary>
        /// Gets or sets whether the indicator is used at all. Disabled under UI automation, where an extra topmost window
        /// would interfere with the tests.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Starts the indicator thread ahead of time, so that the first <see cref="Show"/> does not have to. Idempotent.
        /// </summary>
        public void Initialize()
        {
            if (!IsEnabled) return;
            EnsureStarted();
        }

        /// <summary>
        /// Shows the indicator if the current operation lasts longer than <see cref="BusyIndicatorSettings.ShowDelay"/>.
        /// Must be called on the UI thread, since it reads the main window's position.
        /// </summary>
        public void Show()
        {
            if (!IsEnabled) return;

            var anchor = BusyAnchor.Capture();
            Post(controller => controller.Show(anchor));
        }

        /// <summary>Hides the indicator, honouring the minimum visible time.</summary>
        public void Hide()
        {
            Post(controller => controller.Hide(false));
        }

        /// <summary>Hides the indicator at once, for error and shutdown paths.</summary>
        public void HideImmediately()
        {
            Post(controller => controller.Hide(true));
        }

        /// <summary>
        /// Hides the indicator once the new content has been laid out and rendered.
        /// <para>
        /// Frame.Navigated fires before the new example has been measured, arranged and rendered, so hiding there
        /// directly would drop the indicator while the application is still frozen. <see cref="DispatcherPriority.Loaded"/>
        /// is processed after layout and render have finished but before input is serviced, so the indicator survives
        /// until the example is actually on screen and then goes immediately.
        /// </para>
        /// <para>
        /// Note this deliberately does not use ApplicationIdle: TransitioningFrame starts a 500ms transition animation
        /// when the content changes, and the render work that animation queues starves the idle priorities - which left
        /// the indicator up for a second or two after the example had already appeared.
        /// </para>
        /// </summary>
        public void HideWhenRendered()
        {
            var uiDispatcher = Application.Current?.Dispatcher;
            if (uiDispatcher == null)
            {
                Hide();
                return;
            }

            uiDispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(Hide));
        }

        public void Shutdown()
        {
            _isShutdown = true;

            var dispatcher = _dispatcher;
            var thread = _thread;

            _dispatcher = null;
            if (dispatcher == null) return;

            try
            {
                // The indicator thread never blocks, so this returns immediately; the timeout is purely insurance
                dispatcher.Invoke(new Action(() => _controller?.Close()),
                    DispatcherPriority.Send, CancellationToken.None, TimeSpan.FromSeconds(1));
            }
            catch (Exception caught)
            {
                App.Log.Error("BusyIndicator: failed to close the indicator window", caught);
            }

            dispatcher.InvokeShutdown();
            thread?.Join(TimeSpan.FromSeconds(2));
        }

        private void Post(Action<BusyIndicatorController> action)
        {
            if (!IsEnabled) return;

            var dispatcher = EnsureStarted();
            if (dispatcher == null || _isShutdown) return;

            try
            {
                dispatcher.BeginInvoke(DispatcherPriority.Send, new Action(() =>
                {
                    var controller = _controller;
                    if (controller != null)
                    {
                        action(controller);
                    }
                }));
            }
            catch (InvalidOperationException)
            {
                // The dispatcher has already been shut down
            }
        }

        private Dispatcher EnsureStarted()
        {
            var dispatcher = _dispatcher;
            if (dispatcher != null || _isShutdown) return dispatcher;

            lock (_syncRoot)
            {
                if (_dispatcher != null || _isShutdown) return _dispatcher;

                Dispatcher created = null;

                using (var ready = new ManualResetEventSlim(false))
                {
                    var thread = new Thread(() =>
                    {
                        try
                        {
                            var self = Dispatcher.CurrentDispatcher;

                            SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(self));
                            self.UnhandledException += OnDispatcherUnhandledException;

                            created = self;

                            // Release the caller before building the window, so the UI thread only waits for the thread
                            // itself to spin up
                            ready.Set();

                            self.BeginInvoke(DispatcherPriority.Send,
                                new Action(() => _controller = new BusyIndicatorController()));

                            Dispatcher.Run();
                        }
                        catch (Exception caught)
                        {
                            App.Log.Error("BusyIndicator: the indicator thread terminated unexpectedly", caught);
                            ready.Set();
                        }
                    })
                    {
                        Name = "SciChart BusyIndicator UI",
                        IsBackground = true,

                        // Must be able to win time slices while the UI thread is saturated, otherwise the ring stutters
                        Priority = ThreadPriority.AboveNormal
                    };

                    thread.SetApartmentState(ApartmentState.STA);
                    thread.Start();

                    if (!ready.Wait(TimeSpan.FromSeconds(2)))
                    {
                        App.Log.Debug("BusyIndicator: the indicator thread did not start within 2 seconds");
                    }

                    _thread = thread;
                    _dispatcher = created;

                    return _dispatcher;
                }
            }
        }

        private static void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            // The indicator is cosmetic; never let a failure in it take the application down
            App.Log.Error("BusyIndicator: unhandled exception on the indicator thread", e.Exception);
            e.Handled = true;
        }
    }
}
