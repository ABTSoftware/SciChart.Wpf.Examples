using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SciChart.Examples.Demo.Views
{
    /// <summary>
    /// Interaction logic for TipsView.xaml
    /// </summary>
    public partial class SourceTipsView : UserControl
    {
        public SourceTipsView()
        {
            InitializeComponent();
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", Urls.GithubRootUrl);
        }
    }
}
