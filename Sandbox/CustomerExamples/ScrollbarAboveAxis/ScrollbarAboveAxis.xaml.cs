using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;

namespace ScrollbarAboveAxisExample
{
    public partial class ScrollbarAboveAxis : Window
    {
        public ScrollbarAboveAxis()
        {
            InitializeComponent();

            primaryChartSurface.RenderableSeries.Add(new FastLineRenderableSeries()
            {
                DataSeries=GetData(),
            });
        }

        private IDataSeries GetData()
        {
            var data = new XyDataSeries<double>();
            data.Append(0,0);
            data.Append(1, 1);
            data.Append(2, 2);
            return data;
        }
    }
}
