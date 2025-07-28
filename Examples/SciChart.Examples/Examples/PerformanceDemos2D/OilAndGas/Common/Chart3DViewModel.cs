// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// Chart3DViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.ObjectModel;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Data.Model;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common
{
    public class Chart3DViewModel : BindableObject
    {
        public Chart3DViewModel(IChart3DFactory chartFactory)
        {
            ChartFactory = chartFactory;
            ChartTitle = chartFactory.Title;

            XAxes.Add(chartFactory.GetXAxis());
            YAxes.Add(chartFactory.GetYAxis());
            ZAxes.Add(chartFactory.GetZAxis());

            GetRenderableSeriesAsync();
        }

        public string ChartTitle { get; }

        public IChart3DFactory ChartFactory { get; }

        public ObservableCollection<IAxis3DViewModel> XAxes { get; } = new ObservableCollection<IAxis3DViewModel>();

        public ObservableCollection<IAxis3DViewModel> YAxes { get; } = new ObservableCollection<IAxis3DViewModel>();

        public ObservableCollection<IAxis3DViewModel> ZAxes { get; } = new ObservableCollection<IAxis3DViewModel>();

        public ObservableCollection<IRenderableSeries3DViewModel> RenderableSeries { get; } = new ObservableCollection<IRenderableSeries3DViewModel>();

        public async void GetRenderableSeriesAsync()
        {
            var renderableSeries = await ChartFactory.GetSeriesAsync();
            renderableSeries.ForEachDo(RenderableSeries.Add);
        }
    }
}