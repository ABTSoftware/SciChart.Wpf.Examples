using System.Diagnostics;
using System.Windows;
using SciChart.Charting.Visuals;

namespace TouchInputSandboxExample
{
    /// <summary>
    /// Interaction logic for TouchInputSandbox.xaml
    /// </summary>
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
