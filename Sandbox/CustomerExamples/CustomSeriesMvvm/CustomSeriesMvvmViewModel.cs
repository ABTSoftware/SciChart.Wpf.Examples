using System.Collections.ObjectModel;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace CustomSeriesMvvmExample
{
    public class CustomSeriesMvvmViewModel : BindableObject
    {
        private ObservableCollection<IRenderableSeriesViewModel> _series = new ObservableCollection<IRenderableSeriesViewModel>();

        public CustomSeriesMvvmViewModel()
        {
            _series.Add(new LineRenderableSeriesViewModelEx() { DataSeries = GetData()});
        }

        private XyDataSeries<double> GetData()
        {
            var xyDataSeries = new XyDataSeries<double>();
            xyDataSeries.Append(0,0);
            xyDataSeries.Append(1, 1);
            xyDataSeries.Append(2, 2);
            return xyDataSeries;
        }

        public ObservableCollection<IRenderableSeriesViewModel> Series => _series;
    }
}