using System;
using System.Collections.ObjectModel;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace ChartPrintingMultiPaneChartsExample
{
    public class ChartPrintingMultiPaneChartsViewModel : BindableObject
    {
        private readonly ObservableCollection<LineChartViewModel> _chartViewModels = new ObservableCollection<LineChartViewModel>();

        public ChartPrintingMultiPaneChartsViewModel()
        {
            var data0 = new XyDataSeries<double, double>();
            var data1 = new XyDataSeries<double, double>();
            var data2 = new XyDataSeries<double, double>();

            _chartViewModels.Add(new LineChartViewModel() { ChartData = data0 });
            _chartViewModels.Add(new LineChartViewModel() { ChartData = data1 });
            _chartViewModels.Add(new LineChartViewModel() { ChartData = data2 });

            var random = new Random(0);
            for (int i = 0; i < 100; i++)
            {
                data0.Append(i, random.NextDouble());
                data1.Append(i, random.NextDouble());
                data2.Append(i, random.NextDouble());
            }
        }

        public ObservableCollection<LineChartViewModel> ChartViewModels
        {
            get { return _chartViewModels; }
        }
    }
}