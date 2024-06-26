﻿using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace SciChart.Examples.Demo
{
    /// <summary>
    /// Interaction logic for ShellControl.xaml
    /// </summary>
    public partial class ShellControl : UserControl
    {
        public ShellControl()
        {
            InitializeComponent();           
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var procStartInfo = new ProcessStartInfo(Urls.SciChartWebSite) { UseShellExecute = true };
            Process.Start(procStartInfo);
        }
    }
}