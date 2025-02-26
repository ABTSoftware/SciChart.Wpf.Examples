// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SeriesBindingViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.UseSciChartWithMvvm.SeriesBinding
{
    public class SeriesBindingViewModel : BaseViewModel
    {
        private const int PointsCount = 50;

        public SeriesBindingViewModel()
        {
            var data = DataManager.Instance.GetSinewave(1.0, 0.5, PointsCount*10, 5);

            var lineDataSeries = new XyDataSeries<double, double>();
            lineDataSeries.Append(data.XData.Select(d => d * 5), data.YData);

            IEnumerable<BoxPoint> boxData = GetBoxPlotData().ToArray();

            var boxDataSeries = new BoxPlotDataSeries<double, double>();
            boxDataSeries.Append(boxData.Select(x => x.XValue),
                boxData.Select(x => x.Median),
                boxData.Select(x => x.Minimum),
                boxData.Select(x => x.LowerQuartile),
                boxData.Select(x => x.UpperQuartile),
                boxData.Select(x => x.Maximum));


            EllipsePointMarker epm = new EllipsePointMarker
            {
                Width = 9,
                Height = 9,
                Fill = Colors.Transparent,
                Stroke = Colors.White,
                StrokeThickness = 2
            };

            RenderableSeriesViewModels = new ObservableCollection<IRenderableSeriesViewModel>()
            {
                new LineRenderableSeriesViewModel {DataSeries = lineDataSeries, StyleKey = "LineSeriesStyle"},
                new XyScatterRenderableSeriesViewModel { DataSeries = lineDataSeries, PointMarker = epm},
                new BoxPlotRenderableSeriesViewModel{DataSeries = boxDataSeries}
            };
        }

        public ObservableCollection<IRenderableSeriesViewModel> RenderableSeriesViewModels { get; set; }

        private IEnumerable<BoxPoint> GetBoxPlotData()
        {
            var dates = Enumerable.Range(0, PointsCount).Select(i => i).ToArray();
            var medianValues = new RandomWalkGenerator(0).GetRandomWalkSeries(PointsCount).YData;
            var random = new Random(0);

            for (int i = 0; i < PointsCount; i++)
            {
                double med = medianValues[i];
                double min = med - random.NextDouble();
                double max = med + random.NextDouble();
                double lower = (med - min)*random.NextDouble() + min;
                double upper = (max - med)*random.NextDouble() + med;

                yield return new BoxPoint(dates[i], min, lower, med, upper, max);
            }
        }

        private struct BoxPoint
        {
            public readonly double XValue;
            public readonly double Minimum;
            public readonly double LowerQuartile;
            public readonly double Median;
            public readonly double UpperQuartile;
            public readonly double Maximum;

            public BoxPoint(double xValue, double min, double lower, double med, double upper, double max)
                : this()
            {
                XValue = xValue;
                Minimum = min;
                LowerQuartile = lower;
                Median = med;
                UpperQuartile = upper;
                Maximum = max;
            }
        }
    }
}