using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace SciChart.Examples.Demo
{
    public partial class Splash : UserControl
    {
        public Splash()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", Urls.ReleaseArticle);
        }
    }
}