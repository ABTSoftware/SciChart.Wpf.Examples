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
                Process.Start(NavigateUri);
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