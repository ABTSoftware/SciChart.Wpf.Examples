using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using SciChart.Core.Utility;

namespace SciChart.Sandbox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();            
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            this.DataContext = new MainViewModel();
        }

        private void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var chk = (ComboBox)sender;
            switch (chk.SelectedIndex)
            {
                case 0:
                    SciChartDebugLogger.Instance.SetLogger(null);
                    break;
                case 1:
                    SciChartDebugLogger.Instance.SetLogger(new ConsoleLogger());
                    break;
                case 2:
                    SciChartDebugLogger.Instance.SetLogger(new FileLogger());
                    break;
            }
        }

        private class ConsoleLogger : ISciChartLoggerFacade
        {
            public void Log(string formatString, params object[] args)
            {
                Console.WriteLine(formatString, args);
            }
        }

        private class FileLogger : ISciChartLoggerFacade
        {
            private const string LogFileName = "SciChart.log";
            private const int LogFileAppendMaxSize = 5<<20; // 5 Mb

            private bool _failure;

            public FileLogger()
            {
                try
                {
                    FileInfo fi = new FileInfo(LogFileName);
                    if (fi.Exists && fi.Length > LogFileAppendMaxSize)
                    {
                        File.Delete(LogFileName);
                    }
                }
                catch (Exception e)
                {
                    ReportFailure(e);
                }
            }

            private void ReportFailure(Exception e)
            {
                _failure = true;
                Debug.Assert(false, "An error has been occurred in FileLogger: " + e.Message);
            }

            public void Log(string formatString, params object[] args)
            {
                if (!_failure)
                {
                    try
                    {
                        File.AppendAllText(LogFileName, string.Format(formatString, args) + "\n");
                    }
                    catch (Exception e)
                    {
                        ReportFailure(e);
                    }
                }
            }
        }

        private void Control_OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var item = this.testCases.SelectedItem;
            var dataContext = this.DataContext as MainViewModel;
            if (item != null && dataContext != null)
            {
                dataContext.RunExampleCommand.Execute(null);
            }
        }

        private void GCCollect_Click(object sender, RoutedEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}
