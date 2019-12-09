using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace ZoomExtentsOnVisibilityChangedExample
{
    public class ZoomExtentsOnVisibilityChangedViewModel : BindableObject
    {
        private ObservableCollection<IRenderableSeriesViewModel> _series = new ObservableCollection<IRenderableSeriesViewModel>();
        public ObservableCollection<IRenderableSeriesViewModel> Series => _series;

        public ZoomExtentsOnVisibilityChangedViewModel()
        {
            _series.Add(new LineRenderableSeriesViewModel() { DataSeries = GetData(5,0.01), Stroke = Colors.LightSteelBlue});
            _series.Add(new LineRenderableSeriesViewModel() { DataSeries = GetData(2, 0.2), Stroke= Colors.Orange });
        }

        private IDataSeries GetData(double amplitude, double damping)
        {
            var xyDataSeries = new XyDataSeries<double>();
            for (int i = 0; i < 1000; i++)
            {
                xyDataSeries.Append(i, Math.Sin(i * 0.1) * amplitude);
                amplitude *= (1.0 - damping);
            }

            return xyDataSeries;
        }
    }
}