using System.Windows.Controls;
using SciChart.Charting.Visuals;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for SciChartContent.xaml
    /// </summary>
    public partial class SciChartContent : UserControl
    {
        public SciChartContent()
        {
            InitializeComponent();

            this.VersionTextBlock.Text =
                $"SciChart WPF {SciChartSurface.VersionAndLicenseInfo}";

            // Debug licensing info
            this.DebugInfoTextBox.Text = SciChartSurface.DumpInfo();
        }
    }
}
