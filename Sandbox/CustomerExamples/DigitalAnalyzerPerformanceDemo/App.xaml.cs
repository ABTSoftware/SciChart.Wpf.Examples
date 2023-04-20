using System.Windows;
using System.Windows.Threading;
using SciChart.Examples.ExternalDependencies.Controls.ExceptionView;

namespace SciChart_DigitalAnalyzerPerformanceDemo
{
    public partial class App : Application
    {
        public App()
        {
		     DispatcherUnhandledException += OnDispatcherUnhandledException;

			 InitializeComponent();

			 // TODO: Put your SciChart License Key here if needed
			 // SciChartSurface.SetRuntimeLicenseKey(@"{YOUR SCICHART WPF v6 LICENSE KEY}");
        }

		private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {            
            var exceptionView = new ExceptionView(e.Exception)
            {
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
            };

            exceptionView.ShowDialog();

            e.Handled = true;
        }
    }
}