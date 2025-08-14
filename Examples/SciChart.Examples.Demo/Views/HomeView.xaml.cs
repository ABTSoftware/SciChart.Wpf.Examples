using System.Windows.Controls;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Examples.Demo.Views
{
    public partial class HomeView : UserControl
    {
        private const double OffsetFactor = 250;

        public HomeView()
        {
            InitializeComponent();

            DataContext = ServiceLocator.Container.Resolve<IMainWindowViewModel>();
        }

        private void EverythingScrollViewer_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - (e.Delta / 120) * OffsetFactor);
                e.Handled = true;
            }
        }
    }
}