// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ExportExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace SciChart.Examples.Demo.Views
{
    /// <summary>
    /// Interaction logic for ExportExampleView.xaml
    /// </summary>
    public partial class ExportExampleView : UserControl
    {
        public ExportExampleView()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var procStartInfo = new ProcessStartInfo(Urls.GithubRootUrl) { UseShellExecute = true };
            Process.Start(procStartInfo);
        }
    }
}
