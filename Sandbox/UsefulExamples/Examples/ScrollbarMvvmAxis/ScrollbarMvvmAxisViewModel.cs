using System;
using System.Collections.ObjectModel;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Data.Model;

namespace SciChart.Sandbox.Examples.ScrollbarMvvmAxis
{
    public class ScrollbarMvvmAxisViewModel : BindableObject
    {
        public ScrollbarMvvmAxisViewModel()
        {
            YAxes = new ObservableCollection<IAxisViewModel>();
            XAxes = new ObservableCollection<IAxisViewModel>();
            RenderableSeries = new ObservableCollection<IRenderableSeriesViewModel>();

            YAxes.Add(new NumericAxisViewModelWithScrollbar() { HasScrollbar = false, StyleKey = "ScrollbarAxisStyle" });
            XAxes.Add(new NumericAxisViewModelWithScrollbar() { HasScrollbar = true, StyleKey = "ScrollbarAxisStyle" });

            var xyData = new XyDataSeries<double>();
            for (int i = 0; i < 100; i++)
            {
                xyData.Append(i, Math.Sin(i*0.1));
            }

            RenderableSeries.Add(new LineRenderableSeriesViewModel() { DataSeries = xyData });
        }
        public ObservableCollection<IAxisViewModel> YAxes { get;  }
        public ObservableCollection<IAxisViewModel> XAxes { get; }
        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries { get; }
    }
}