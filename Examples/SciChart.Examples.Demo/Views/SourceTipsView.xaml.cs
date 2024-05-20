using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo.Views
{
    public partial class SourceTipsView : UserControl
    {
        public SourceTipsView()
        {
            InitializeComponent();
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var procStartInfo = new ProcessStartInfo(Urls.GithubRootUrl) { UseShellExecute = true };
            Process.Start(procStartInfo);
        }
    }
}
