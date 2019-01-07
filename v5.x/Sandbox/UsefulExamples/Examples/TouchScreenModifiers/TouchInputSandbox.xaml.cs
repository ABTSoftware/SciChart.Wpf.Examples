using System.Diagnostics;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Sandbox.Shared;

namespace SciChart.Sandbox.Examples.TouchScreenModifiers
{
    /// <summary>
    /// Interaction logic for TouchInputSandbox.xaml
    /// </summary>
    [TestCase("Touch Input Sandbox")]
    public partial class TouchInputSandbox : Window
    {
        public TouchInputSandbox()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
            {
                ((MainGrid) this.scs2D.RootGrid).TouchDown += (s, e) => Debug.WriteLine("Rootgrid.TouchDown");
                ((MainGrid)this.scs2D.RootGrid).TouchUp += (s, e) => Debug.WriteLine("Rootgrid.TouchUp");
            };
        }
    }
}
