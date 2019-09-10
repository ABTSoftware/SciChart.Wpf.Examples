using System.Windows;
using System.Windows.Media;
using Microsoft.Practices.Unity;
using SciChart.Examples.Demo.ViewModels;
using SciChart.Wpf.UI.Bootstrap;

namespace SciChart.Examples.Demo.Views
{
    /// <summary>
    /// Interaction logic for ExampleView.xaml
    /// </summary>
    public partial class ExampleView
    {
        public ExampleView()
        {
            InitializeComponent();
            DataContext = ServiceLocator.Container.Resolve<ExampleViewModel>();
        }

        private void OnExamplesPopupSizeChanged(object sender, SizeChangedEventArgs e)
        {
            var rectangleGeometry = new RectangleGeometry
            {
                Rect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height)
            };
            ((UIElement)sender).Clip = rectangleGeometry;
        }
    }
}
