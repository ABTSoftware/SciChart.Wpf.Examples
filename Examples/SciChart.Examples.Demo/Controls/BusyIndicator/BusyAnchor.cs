// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// BusyAnchor.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Media;
using Screen = System.Windows.Forms.Screen;

namespace SciChart.Examples.Demo.Controls.BusyIndicator
{
    /// <summary>
    /// An immutable snapshot of where the busy indicator should be drawn, in screen device pixels. This is the only data
    /// that crosses from the UI thread to the indicator thread, so it deliberately holds no WPF objects - anything owned
    /// by the UI thread would be illegal to touch from the indicator's own dispatcher.
    /// </summary>
    internal readonly struct BusyAnchor
    {
        public readonly int CentreXDevice;
        public readonly int CentreYDevice;
        public readonly double DpiScaleX;
        public readonly double DpiScaleY;

        public BusyAnchor(int centreXDevice, int centreYDevice, double dpiScaleX, double dpiScaleY)
        {
            CentreXDevice = centreXDevice;
            CentreYDevice = centreYDevice;
            DpiScaleX = dpiScaleX <= 0 ? 1.0 : dpiScaleX;
            DpiScaleY = dpiScaleY <= 0 ? 1.0 : dpiScaleY;
        }

        /// <summary>
        /// Captures the centre of the main window. MUST be called on the UI thread, before it starts the work that will
        /// block it. Uses PointToScreen rather than Window.Left/Top because the latter is unreliable when the window is
        /// maximised, whereas PointToScreen always yields the true on-screen rect of the monitor the window is on.
        /// </summary>
        public static BusyAnchor Capture()
        {
            try
            {
                var window = Application.Current?.MainWindow;

                if (window != null && window.IsLoaded && window.ActualWidth > 0 && window.ActualHeight > 0 &&
                    PresentationSource.FromVisual(window) != null)
                {
                    var topLeft = window.PointToScreen(new Point(0, 0));
                    var bottomRight = window.PointToScreen(new Point(window.ActualWidth, window.ActualHeight));
                    var dpi = VisualTreeHelper.GetDpi(window);

                    return new BusyAnchor(
                        (int)Math.Round((topLeft.X + bottomRight.X) / 2.0),
                        (int)Math.Round((topLeft.Y + bottomRight.Y) / 2.0),
                        dpi.DpiScaleX,
                        dpi.DpiScaleY);
                }
            }
            catch (Exception caught)
            {
                App.Log.Error("BusyIndicator: could not read the main window position, using the work area instead", caught);
            }

            return FromPrimaryWorkArea();
        }

        private static BusyAnchor FromPrimaryWorkArea()
        {
            var area = Screen.PrimaryScreen.WorkingArea;

            // Screen reports physical pixels, SystemParameters reports device-independent units, so their ratio is the
            // scale factor of the primary monitor
            var scale = SystemParameters.PrimaryScreenWidth > 0
                ? Screen.PrimaryScreen.Bounds.Width / SystemParameters.PrimaryScreenWidth
                : 1.0;

            return new BusyAnchor(area.X + area.Width / 2, area.Y + area.Height / 2, scale, scale);
        }
    }
}
