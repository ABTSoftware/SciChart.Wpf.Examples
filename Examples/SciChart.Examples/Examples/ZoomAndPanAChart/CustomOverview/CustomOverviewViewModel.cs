// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
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

        private readonly SeriesStrokeProvider _seriesStrokeProvider = new SeriesStrokeProvider()
        {
            StrokePalette = new[]
            {
                Color.FromArgb(0xAA, 0x27, 0x4b, 0x92),
                Color.FromArgb(0xAA, 0x47, 0xbd, 0xe6),
                Color.FromArgb(0xAA, 0xa3, 0x41, 0x8d),
                Color.FromArgb(0xAA, 0xe9, 0x70, 0x64),
                Color.FromArgb(0xAA, 0x68, 0xbc, 0xae),
                Color.FromArgb(0xAA, 0x63, 0x4e, 0x96),
            }
        };

        public CustomOverviewViewModel()
        {
            Initialize();
        }

        private void Initialize()
        {
            RenderableSeriesViewModels = new ObservableCollection<IRenderableSeriesViewModel>();
            var random = new Random(0);
            var generator = new RandomWalkGenerator();

            for (int i = 0; i < SeriesCount; i++)
            {
                var dataSeries = new UniformXyDataSeries<double>();
                var someData = generator.GetRandomWalkYData(PointCount);
                
                generator.Reset();
                dataSeries.Append(someData);

                var rgb = new byte[3];
                random.NextBytes(rgb);

                RenderableSeriesViewModels.Add(new LineRenderableSeriesViewModel
                {
                    DataSeries = dataSeries,
                    AntiAliasing = false,
                    StrokeThickness = 2,
                    Stroke = _seriesStrokeProvider.GetStroke(i, SeriesCount)
                });
            }
        }

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeriesViewModels { get; set; }
    }
}