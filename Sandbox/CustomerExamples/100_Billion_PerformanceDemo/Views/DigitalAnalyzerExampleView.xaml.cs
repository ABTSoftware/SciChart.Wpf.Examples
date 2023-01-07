using SciChart.Drawing.Common;
using System.Windows.Controls;

namespace SciChart_DigitalAnalyzerPerformanceDemo
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