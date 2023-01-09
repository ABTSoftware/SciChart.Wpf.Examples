using System;
using System.Windows;

namespace WPFChartPerformanceBenchmark
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.ToString());
            }
        }

        private void Run_Click(object sender, RoutedEventArgs e)
        {
            // start the tests
            ((MainViewModel)DataContext).LayoutRoot = this.LayoutRoot;
            ((MainViewModel)DataContext).RunNextTest();
        }
   
        private void Clipboard_Click(object sender, RoutedEventArgs e)
        {           
            dataGrid1.ClipboardCopyMode = System.Windows.Controls.DataGridClipboardCopyMode.IncludeHeader;
            dataGrid1.SelectAll();// SelectAllCells();

            System.Windows.Input.ApplicationCommands.Copy.Execute(null, dataGrid1);
            dataGrid1.UnselectAllCells();        
        }
    }
}
