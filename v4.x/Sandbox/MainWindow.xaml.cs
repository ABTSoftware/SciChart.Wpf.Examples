using System;
using System.Windows;
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

        private void EnableLoggerChecked(object sender, RoutedEventArgs e)
        {
            var chk = (ToggleButton) sender;
            if (chk.IsChecked == true)
            {
                SciChartDebugLogger.Instance.SetLogger(new ConsoleLogger());
            }
            else
            {
                SciChartDebugLogger.Instance.SetLogger(null);
            }
        }

        private class ConsoleLogger : ISciChartLoggerFacade
        {
            public void Log(string formatString, params object[] args)
            {
                Console.WriteLine(formatString, args);
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
