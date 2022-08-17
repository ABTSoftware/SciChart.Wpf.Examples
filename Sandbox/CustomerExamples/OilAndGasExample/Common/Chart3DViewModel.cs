using System.Collections.ObjectModel;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Core.Framework;
using SciChart.Data.Model;

namespace OilAndGasExample.Common
{
    public class Chart3DViewModel : BindableObject
    {
        private ISuspendable _suspendable;

        public string ChartTitle { get; }

        public IChart3DFactory ChartFactory { get; }

        public ObservableCollection<IAxis3DViewModel> XAxes { get; } = new ObservableCollection<IAxis3DViewModel>();

        public ObservableCollection<IAxis3DViewModel> YAxes { get; } = new ObservableCollection<IAxis3DViewModel>();

        public ObservableCollection<IAxis3DViewModel> ZAxes { get; } = new ObservableCollection<IAxis3DViewModel>();

        public ObservableCollection<IRenderableSeries3DViewModel> RenderableSeries { get; } = new ObservableCollection<IRenderableSeries3DViewModel>();

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
                        ChartFactory?.GetSeries().ForEachDo(RenderableSeries.Add);
                    }
                }
            }
        }

        public Chart3DViewModel(IChart3DFactory chartFactory)
        {
            ChartFactory = chartFactory;
            ChartTitle = chartFactory.Title;

            XAxes.Add(chartFactory.GetXAxis());
            YAxes.Add(chartFactory.GetYAxis());
            ZAxes.Add(chartFactory.GetZAxis());
        }
    }
}
