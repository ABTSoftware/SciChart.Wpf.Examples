using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace SciChart.Sandbox.Examples.MultiLineDateTimeAxisLabels
{
    [TestCase("Multi Line DateTimeAxis Labels")]
    public partial class MultiLineDateTimeAxisLabels : Window
    {
        public MultiLineDateTimeAxisLabels()
        {
            InitializeComponent();

            xAxis.TextFormatting = "dd MMM yyyy\r\nHH:mm:ss";
        }
    }
}
