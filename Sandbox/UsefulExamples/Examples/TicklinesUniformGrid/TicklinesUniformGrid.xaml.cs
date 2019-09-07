using System.Windows;

namespace SciChart.Sandbox.Examples.TicklinesUniformGrid
{
    /// <summary>
    /// Demonstrates tick lines / grid lines on screen at actual millimeter spacings, according to device DPI
    /// </summary>
    [TestCase("Ticklines with Uniform Grid (5mm spacing)")]
    public partial class TicklinesUniformGrid : Window
    {
        public TicklinesUniformGrid()
        {
            InitializeComponent();

            xAxis.TickProvider = new MillimeterDivisionsTickProvider(25, 5);
            yAxis.TickProvider = new MillimeterDivisionsTickProvider(25, 5);
        }
    }
}
