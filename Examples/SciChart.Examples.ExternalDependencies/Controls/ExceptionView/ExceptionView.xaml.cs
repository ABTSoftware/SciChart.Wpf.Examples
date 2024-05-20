using System;
using System.Diagnostics;
using System.Windows;
using SciChart.Charting.Visuals;

namespace SciChart.Examples.ExternalDependencies.Controls.ExceptionView
{
    /// <summary>
    /// Interaction logic for ExceptionView.xaml
    /// </summary>
    public partial class ExceptionView : Window
    {
        public ExceptionView(Exception exception)
        {
            InitializeComponent();

            LogException(exception);
        }

        private void LogException(Exception exception)
        {
            if (exception == null) return;

            exceptionViewer.Text += exception.GetType().Name + ": " + exception.Message + Environment.NewLine;
            exceptionViewer.Text += "-------------------------------------------" + Environment.NewLine + Environment.NewLine;
            exceptionViewer.Text += "Stack Trace: " + Environment.NewLine;
            exceptionViewer.Text += exception.StackTrace + Environment.NewLine + Environment.NewLine;            

            LogException(exception.InnerException);
        }

        private void CopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetData(DataFormats.Text, FormatEmail());
        }

        private void EmailSupport_Click(object sender, RoutedEventArgs e)
        {
            string email = "mailto:support@scichart.com?subject=Unhandled%20Exception&body=" + Uri.EscapeDataString(FormatEmail());
            try
            {
                var procStartInfo = new ProcessStartInfo(email) { UseShellExecute = true };
                Process.Start(procStartInfo);
            }
            catch (Exception)
            {
                MessageBox.Show(
                    "We have not detected an email client on your PC!\r\nPlease email support@scichart.com with the exception message.");
            }
        }

        private string FormatEmail()
        {
            return
                string.Format(
                    "Dear Support, \r\n\r\nI was running the SciChart {0} examples and saw this Unhandled Exception. \r\n\r\nCan you help? \r\n\r\nThank you!\r\n\r\n\r\n{1}",
                    SciChartSurface.VersionInfo, exceptionViewer.Text);
        }
    }
}
