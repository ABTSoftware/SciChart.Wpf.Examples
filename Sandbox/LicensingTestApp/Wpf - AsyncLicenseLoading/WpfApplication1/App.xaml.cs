using System.Windows;
using SciChart.Charting3D;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App() 
        {
            InitializeComponent();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Async load libraries and apply license 
            // 
            // This code should be called and awaited before any SciChartSurface or SciChart3DSurface is shown
            // 
            // Use SciChart.Charting3D.SciChart2D3DInitializer if you use both 2D & 3D charts in your app. Note this also initializes 2D libraries 
            // Use SciChart.Charting.Visuals.SciChart2DInitializer if you use only 2D charts in your app
            // 
            // Options: runtimeLicenseKey should be set to your license key
            //          tempDirectory is an optional temp directory for loading native libraries. Leave null for the default 

            string runtimeLicenseKey = "your license keycode here";
            string tempDirectory = null;

            var initTask = SciChart2D3DInitializer.LoadLibrariesAndLicenseAsync(runtimeLicenseKey, tempDirectory);

            // You can either await initTask or use SciChart2D3DInitializer.Awaiter
            // to do the same thing later on (see MainWindow.xaml.cs)
        }
    }
}
