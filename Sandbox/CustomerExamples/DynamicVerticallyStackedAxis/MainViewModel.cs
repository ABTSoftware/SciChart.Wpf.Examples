using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using SciChart.Charting.Common.Helpers;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Data.Model;

namespace WpfApp33
{
    public class MainViewModel : BindableObject
    {
        public MainViewModel()
        {
            
        }

        public ICommand AddSeriesCommand => new ActionCommand(() =>
        {
            var axisId = Guid.NewGuid().ToString();
            Series.Add(new LineRenderableSeriesViewModel() { DataSeries = GetData(), YAxisId = axisId});
            // important, left alignment has been templated as vertically stacked axis
            YAxis.Add(new NumericAxisViewModel() { Id = axisId, AxisAlignment = AxisAlignment.Left}); 
        });

        public ICommand RemoveSeriesCommand => new ActionCommand(() =>
        {
            if (Series.Count > 1)
            {
                Series.Remove(Series.Last());
                YAxis.Remove(YAxis.Last());
            }
        });

        public ObservableCollection<IAxisViewModel> YAxis { get; } = new ObservableCollection<IAxisViewModel>();

        public ObservableCollection<IRenderableSeriesViewModel> Series { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

        private IDataSeries GetData()
        {
            var xyDataSeries = new XyDataSeries<double>();
            var random = new Random((int)DateTime.UtcNow.Ticks);
            var phase = (random.NextDouble() + 0.5) * 10;
            for (int i = 0; i < 100; i++)
            {
                xyDataSeries.Append(i, Math.Sin(i * phase));
            }

            return xyDataSeries;
        }
    }
}
