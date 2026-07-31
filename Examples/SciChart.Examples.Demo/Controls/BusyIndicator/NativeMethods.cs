// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// NativeMethods.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Runtime.InteropServices;

namespace SciChart.Examples.Demo.Controls.BusyIndicator
{
    /// <summary>
    /// The minimal set of Win32 calls needed to place and style the busy indicator window. The indicator is positioned
    /// with SetWindowPos in device pixels rather than through Window.Left/Top, so that it lands correctly regardless of
    /// the per-monitor DPI of the screen the main window happens to be on.
    /// </summary>
    internal static class NativeMethods
    {
        public const int GWL_EXSTYLE = -20;

        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_TOOLWINDOW = 0x00000080;
        public const int WS_EX_NOACTIVATE = 0x08000000;

        public const uint SWP_NOACTIVATE = 0x0010;

        public static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int cx, int cy, uint flags);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(IntPtr hWnd, int index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(IntPtr hWnd, int index, int value);

        [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int index);

        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int index, IntPtr value);

        /// <summary>
        /// Adds the given bits to the window's extended style. GetWindowLongPtr / SetWindowLongPtr are only exported on
        /// 64-bit Windows, hence the split - the Demo ships as both x86 and x64.
        /// </summary>
        public static void AddExtendedStyle(IntPtr hWnd, int styleBits)
        {
            if (hWnd == IntPtr.Zero) return;

            if (IntPtr.Size == 8)
            {
                var style = GetWindowLongPtr64(hWnd, GWL_EXSTYLE).ToInt64();

                // Cast through uint so the style bits are not sign-extended into the upper 32 bits
                SetWindowLongPtr64(hWnd, GWL_EXSTYLE, new IntPtr(style | (uint)styleBits));
            }
            else
            {
                var style = GetWindowLong32(hWnd, GWL_EXSTYLE);
                SetWindowLong32(hWnd, GWL_EXSTYLE, style | styleBits);
            }
        }
    }
}
