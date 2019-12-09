using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;

namespace TruePolarChartExample
{
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
