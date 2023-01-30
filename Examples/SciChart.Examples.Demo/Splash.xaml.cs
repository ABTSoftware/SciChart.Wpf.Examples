using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SciChart.Examples.Demo
{
    public partial class Splash : UserControl
    {
        public Splash()
        {
            InitializeComponent();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            Process.Start("explorer.exe", Urls.ReleaseArticle);
        }
    }
}