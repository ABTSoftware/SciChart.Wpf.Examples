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

            string runtimeLicenseKey = "rNgIBKipsrb7fpO7YZRMtF9xg21BrQYO64gZPAwPOO9tYgvTmwYmn7DFZvBckZk4c1mA10d7A+So+YzzxFMLl9fEvBpiWv5Ppucrv6jmDqYCNmgE7QBaO3rR6rKUz0uq4UU9bYOkKrYKSY742YGnAqu2vZhNwPjxfY60nxRUscNdlm8aVusmVta3oJlU2MHvP50tR1ymZtkvmGcuw+v55GGEEomwQWRdMXJOGnKBD3kkudDOnbp3TMR9/6Wtn4KvKOv/1yuvfmK2mGdNtDwbEyHY9cLvWH9JRhS9awIOHfDsfv4xW36p8xBt2z/C4zwsci+GLZ+sOKFXL7G3zbHC1k/P7yHbSI9po8PYRONUtJLIU6vHFkGR+JbgNPHAaTMddlb/xWLsQ5M50rPgPtvHQY+aDZGCSE9c159n6i0gsskopfYtNomteHzocSm3RbOOt1knwSSKU05Osxz+WLGdqYwhkVk4cwUtcAonlTPj+gougDvelP1EyPeNYMpP+GGLrx3Y4wueCw82EZwqKnGM8nJWlndjGmePk9fXlQ3ogiqySnORxuVg4+TaVBKbIZ5I7ZoT+zzKgw==";
            string tempDirectory = null;

            var initTask = SciChart2D3DInitializer.LoadLibrariesAndLicenseAsync(runtimeLicenseKey, tempDirectory);

            // You can either await initTask or use SciChart2D3DInitializer.Awaiter
            // to do the same thing later on (see MainWindow.xaml.cs)
        }
    }
}
