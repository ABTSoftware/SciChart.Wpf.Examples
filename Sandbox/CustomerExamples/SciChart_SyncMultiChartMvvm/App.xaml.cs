using System.Windows;

namespace SciChart_SyncMultiChartMvvm
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

			 // TODO: Put your SciChart Runtime License Key here if needed
			 // SciChartSurface.SetRuntimeLicenseKey(@"{YOUR SCICHART WPF v8 RUNTIME LICENSE KEY}");
        }
    }
}
