using System.Windows;
using System.Windows.Media;
using SciChart.Examples.Demo.ViewModels;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Examples.Demo.Views
{
    public partial class ExampleView
    {
        public ExampleView()
        {
            InitializeComponent();

            DataContext = ServiceLocator.Container.Resolve<IExampleViewModel>();

            BreadcrumbWrapper.SizeChanged += (s, e) =>
            {
                if (e.WidthChanged)
                {
                    if (BreadcrumbWrapper.ActualWidth <= 120)
                    {
                        BreadcrumbChain.Visibility = Visibility.Collapsed;
                        BreadcrumbButton.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        BreadcrumbChain.Visibility = Visibility.Visible;
                        BreadcrumbButton.Visibility = Visibility.Collapsed;
                    }
                }
            };
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