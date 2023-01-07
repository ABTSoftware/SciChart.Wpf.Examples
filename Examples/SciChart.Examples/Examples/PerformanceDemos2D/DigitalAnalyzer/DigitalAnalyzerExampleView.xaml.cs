using System.Windows.Controls;
using SciChart.Drawing.Common;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer
{
    public partial class DigitalAnalyzerExampleView : UserControl
    {
        public DigitalAnalyzerExampleView()
        {
            InitializeComponent();

            Loaded += (s, e) => RenderSurfaceBase.UseThreadedRenderTimer = true;
            Unloaded += (s, e) => RenderSurfaceBase.UseThreadedRenderTimer = false;
        }
    }
}