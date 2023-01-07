using System;
using System.Threading.Tasks;
using System.Windows;
using SciChart.Charting.Visuals;
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

            // Dumps licensing info to console
            Console.WriteLine(SciChartSurface.DumpInfo());

            ((LoadingContent) this.root.Children[0]).StopTimer();

            // SciChart 6.2.0's license init is so fast that we put this awaiter in just so you can see the loading screen with time to initialize
            // Remove it in your app, its not necessary 
            await Task.Delay(TimeSpan.FromMilliseconds(500));

            // Now swap out content for SciChartSurfaces once licenses loaded 
            this.root.Children.Add(new SciChartContent());
            this.root.Children.RemoveAt(0);
        }
    }
}
