// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomOverviewViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ZoomAndPanAChart.CustomOverview
{
    public class CustomOverviewViewModel: BaseViewModel
    {
        private const int SeriesCount = 25;
        private const int PointCount = 500;

        public CustomOverviewViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            RenderableSeriesViewModels = new ObservableCollection<IRenderableSeriesViewModel>();
            var random = new Random();
            var generator = new RandomWalkGenerator();

            for (int i = 0; i < SeriesCount; i++)
            {
                var dataSeries = new XyDataSeries<double, double>();
                var someData = generator.GetRandomWalkSeries(PointCount);
                generator.Reset();
                dataSeries.Append(someData.XData, someData.YData);

                var rgb = new byte[3];
                random.NextBytes(rgb);

                RenderableSeriesViewModels.Add(new LineRenderableSeriesViewModel
                {
                    DataSeries = dataSeries,
                    AntiAliasing = false,
                    Stroke = Color.FromArgb(255, rgb[0], rgb[1], rgb[2]),
                });
            }
        }

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeriesViewModels { get; set; }
    }
}