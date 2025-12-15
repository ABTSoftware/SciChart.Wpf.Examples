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