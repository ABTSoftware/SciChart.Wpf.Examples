using System.Windows;
using SciChart.Charting3D;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.Loaded += MainWindowLoaded;
        }

        private async void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            // Wait for the license initialization we triggered in App.xaml.cs
            await SciChart2D3DInitializer.Awaiter;

            ((LoadingContent) this.root.Children[0]).StopTimer();

            // Now swap out content for SciChartSurfaces once licenses loaded 
            this.root.Children.Clear();
            this.root.Children.Add(new SciChartContent());
        }
    }
}
