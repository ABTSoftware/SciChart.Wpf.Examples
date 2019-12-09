using System.Windows;
using TicklinesUniformGridExample;

namespace SciChart.Sandbox.Examples.TicklinesUniformGrid
{
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
