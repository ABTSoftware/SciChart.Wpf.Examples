using System.Windows.Controls;
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
        }
    }
}
