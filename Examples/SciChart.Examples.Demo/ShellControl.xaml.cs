// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ShellControl.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SciChart.Examples.Demo
{
    public partial class ShellControl : UserControl
    {
        public ShellControl()
        {
            InitializeComponent();

            SearchBoxWrapper.SizeChanged += (s, e) =>
            {
                if (e.WidthChanged)
                {
                    if (SearchBoxWrapper.ActualWidth <= 400)
                    {
                        SearchBox.Width = double.NaN;
                        SearchBox.HorizontalAlignment = HorizontalAlignment.Stretch;
                    }
                    else
                    {
                        SearchBox.Width = 400;
                        SearchBox.HorizontalAlignment = HorizontalAlignment.Right;
                    }
                }
            };
        }

        private void OnSciChartLogoMouseDown(object sender, MouseButtonEventArgs e)
        {
            var procStartInfo = new ProcessStartInfo(Urls.SciChartWebSite) { UseShellExecute = true };

            Process.Start(procStartInfo);
        }
    }
}