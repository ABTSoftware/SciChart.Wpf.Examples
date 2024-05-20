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
