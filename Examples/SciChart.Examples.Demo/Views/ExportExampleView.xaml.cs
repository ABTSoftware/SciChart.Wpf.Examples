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

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", Urls.GithubRootUrl);
        }
    }
}
