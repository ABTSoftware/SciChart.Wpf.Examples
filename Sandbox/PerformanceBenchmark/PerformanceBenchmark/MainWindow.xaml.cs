using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace PerformanceBenchmark
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
#if XPF
                Title = "Performance Benchmark - SciChart Avalonia XPF";
                Icon = BitmapFrame.Create(new Uri("pack://application:,,,/SciChartXpf256x256.ico", UriKind.Absolute));
#else
                Title = "Performance Benchmark - SciChart WPF";
                Icon = BitmapFrame.Create(new Uri("pack://application:,,,/SciChartWin256x256.ico", UriKind.Absolute));
#endif
                DataContext = new MainViewModel();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is MainViewModel viewModel)
            {
                viewModel.LayoutRoot = testCaseRoot;
                viewModel.RunNextTest();
            }
        }

        private void Clipboard_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ClipboardCopyMode = DataGridClipboardCopyMode.IncludeHeader;
            dataGrid.SelectAll();

            ApplicationCommands.Copy.Execute(null, dataGrid);
            dataGrid.UnselectAllCells();
        }
    }
}
