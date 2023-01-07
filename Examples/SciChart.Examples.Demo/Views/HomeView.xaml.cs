using System.Windows;
using System.Windows.Controls;
using SciChart.Core.Utility;
using Unity;
using SciChart.UI.Bootstrap;

namespace SciChart.Examples.Demo.Views
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Container.Resolve<IMainWindowViewModel>();

//            this.Loaded += (s, e) =>
//            {
//                TimedMethod.Invoke(() => CoverFlowControl.Visibility = Visibility.Visible).After(2000).Go();
//            };
        }
    }
}
