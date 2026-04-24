// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// HyperlinkButtonCompatible.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SciChart.Examples.Demo.Controls
{
    public class HyperlinkButtonCompatible : Button, INotifyPropertyChanged
    {
        private string _navigateUri;

        public string NavigateUri
        {
            get { return _navigateUri; }
            set
            {
                _navigateUri = value;
                OnPropertyChanged("NavigateUri");
            }
        }

        public HyperlinkButtonCompatible()
        {
            Click += OnClick;
            Cursor = Cursors.Hand;
        }

        private void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
#if !SILVERLIGHT
            if(!string.IsNullOrWhiteSpace(NavigateUri))
            {
                var procStartInfo = new ProcessStartInfo(NavigateUri) { UseShellExecute = true };
                Process.Start(procStartInfo);
            }
#else
            if (!string.IsNullOrWhiteSpace(NavigateUri))
            {
                System.Windows.Browser.HtmlPage.Window.Navigate(new Uri(NavigateUri), "_blank");
            }
#endif
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}