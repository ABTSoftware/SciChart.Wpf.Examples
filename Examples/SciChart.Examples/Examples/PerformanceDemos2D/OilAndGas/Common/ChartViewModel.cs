// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ChartViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.ObjectModel;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Core.Extensions;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common
{
    public class ChartViewModel : BindableObject
    {
        public ChartViewModel(IChartFactory chartFactory)
        {
            ChartFactory = chartFactory;
            ChartTitle = chartFactory.Title;

            XAxes.Add(chartFactory.GetXAxis());
            YAxes.Add(chartFactory.GetYAxis());

            GetRenderableSeriesAsync();
        }

        public string ChartTitle { get; }

        public IChartFactory ChartFactory { get; }

        public ObservableCollection<IAxisViewModel> XAxes { get; } = new ObservableCollection<IAxisViewModel>();

        public ObservableCollection<IAxisViewModel> YAxes { get; } = new ObservableCollection<IAxisViewModel>();

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeries { get; } = new ObservableCollection<IRenderableSeriesViewModel>();

        public async void GetRenderableSeriesAsync()
        {
            var renderableSeries = await ChartFactory.GetSeriesAsync();
            renderableSeries.ForEachDo(RenderableSeries.Add);
        }
    }
}