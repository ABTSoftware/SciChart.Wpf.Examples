using System.Collections.ObjectModel;
using OilAndGasExample.VerticalCharts.ChartTypes;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Core.Extensions;
using SciChart.Core.Framework;
using SciChart.Data.Model;

namespace OilAndGasExample.VerticalCharts
{
    public class VerticalChartViewModel : BindableObject
    {
        private ISuspendable _suspendable;

        public string ChartTitle { get; }

        public IChartInitializer ChartInitializer { get; }

        public ObservableCollection<IAxisViewModel> XAxes { get; } = new ObservableCollection<IAxisViewModel>();

        public ObservableCollection<IAxisViewModel> YAxes { get; } = new ObservableCollection<IAxisViewModel>();

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

        public ISuspendable Suspendable
        {
            get => _suspendable;
            set
            {
                if (!ReferenceEquals(_suspendable, value))
                {
                    _suspendable = value;

                    using (_suspendable.SuspendUpdates())
                    {
                        ChartInitializer?.GetSeries().ForEachDo(RenderableSeries.Add);
                    }
                }
            }
        }

        public VerticalChartViewModel(IChartInitializer chartInitializer)
        {
            ChartInitializer = chartInitializer;
            ChartTitle = chartInitializer.ChartTitle;

            XAxes.Add(chartInitializer.GetXAxis());
            YAxes.Add(chartInitializer.GetYAxis());
        }
    }
}