using System.Windows;

namespace MultiLineDateTimeAxisLabelsExample
{
    public partial class MultiLineDateTimeAxisLabels : Window
    {
        public MultiLineDateTimeAxisLabels()
        {
            InitializeComponent();

            xAxis.TextFormatting = "dd MMM yyyy\r\nHH:mm:ss";
        }
    }
}
