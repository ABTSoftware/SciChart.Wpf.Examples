using System.Windows;
using System.Windows.Input;

namespace WpfApp31
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

        private void Control_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("Double Click! " + sender.GetType().Name);
            e.Handled = true;
        }
    }
}
