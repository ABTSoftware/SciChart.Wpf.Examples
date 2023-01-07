using System.Windows;

namespace DoubleScaleDiscontinuousDateTimeAxisExample
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}
