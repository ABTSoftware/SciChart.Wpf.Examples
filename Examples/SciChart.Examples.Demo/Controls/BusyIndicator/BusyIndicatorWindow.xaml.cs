// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// BusyIndicatorWindow.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Animation;

namespace SciChart.Examples.Demo.Controls.BusyIndicator
{
    /// <summary>
    /// The rotating ring shown while the application is busy. Created on, and only ever touched from, the busy indicator
    /// thread - its animation is therefore driven by that thread's dispatcher and keeps running while the UI thread is
    /// blocked.
    /// </summary>
    public partial class BusyIndicatorWindow : Window
    {
        private readonly Storyboard _spinStoryboard;

        public BusyIndicatorWindow()
        {
            InitializeComponent();

            _spinStoryboard = (Storyboard)Resources["SpinStoryboard"];
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);

            // Transparent so clicks pass through to whatever is underneath, NoActivate so it never takes focus from the
            // main window, ToolWindow so it stays out of Alt-Tab
            NativeMethods.AddExtendedStyle(new WindowInteropHelper(this).Handle,
                NativeMethods.WS_EX_TRANSPARENT | NativeMethods.WS_EX_NOACTIVATE | NativeMethods.WS_EX_TOOLWINDOW);
        }

        public void StartSpin()
        {
            // isControllable: true is what makes the matching Stop() call legal
            _spinStoryboard.Begin(this, true);
        }

        public void StopSpin()
        {
            _spinStoryboard.Stop(this);
        }
    }
}
