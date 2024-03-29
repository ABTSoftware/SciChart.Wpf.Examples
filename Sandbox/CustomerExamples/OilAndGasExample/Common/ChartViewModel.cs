﻿using System.Collections.ObjectModel;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Core.Extensions;
using SciChart.Core.Framework;
using SciChart.Data.Model;

namespace OilAndGasExample.Common
{
    public class ChartViewModel : BindableObject
    {
        private ISuspendable _suspendable;

        public string ChartTitle { get; }

        public IChartFactory ChartFactory { get; }

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
                        ChartFactory?.GetSeries().ForEachDo(RenderableSeries.Add);
                    }
                }
            }
        }

        public ChartViewModel(IChartFactory chartFactory)
        {
            ChartFactory = chartFactory;
            ChartTitle = chartFactory.Title;

            XAxes.Add(chartFactory.GetXAxis());
            YAxes.Add(chartFactory.GetYAxis());
        }
    }
}