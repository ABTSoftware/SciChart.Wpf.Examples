using System.Windows.Controls;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Examples.Demo.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();

            DataContext = ServiceLocator.Container.Resolve<IMainWindowViewModel>();
        }
    }
}