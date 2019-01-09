using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Sandbox.Examples.TruePolar
{
    // Work in progress. Attempting to create a polar chart which wraps around 2x ... 
    // SciChart one does not 
    [TestCase("True polar chart")]
    public partial class TruePolarChart : Window
    {
        public TruePolarChart()
        {
            InitializeComponent();

            var xyData = new XyDataSeries<double>();
            for (int i = 0; i < 720; i++)
            {
                xyData.Append(i, i);
            }
            this.scs.RenderableSeries.Add(new FastLineRenderableSeries() { DataSeries = xyData});
        }
    }
}
