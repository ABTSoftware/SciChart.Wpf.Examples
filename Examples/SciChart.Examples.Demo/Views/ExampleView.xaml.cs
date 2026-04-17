using System.Windows;
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

        private void DescriptionBox_OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
    }
}
