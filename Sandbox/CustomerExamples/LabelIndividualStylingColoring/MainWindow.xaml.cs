using System.Windows;
using System.Windows.Media;

namespace LabelIndividualStylingColoring
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            xAxis.LabelProvider = new ColorLabelProvider(new ColorGenerator(Colors.GreenYellow, Colors.Aqua, 0, 10));
            yAxis.LabelProvider = new ColorLabelProvider(new ColorGenerator(Colors.OrangeRed, Colors.BlueViolet, 0, 10));
        }
    }
}