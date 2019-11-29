// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HistogramViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Globalization;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.Histogram
{
    public class HistogramViewModel : BaseViewModel
    {
        private static readonly double[] BlueSeriesData = {1, 1, 0, 2, 3, 1, 1, 6, 3, 12, 15, 6, 10, 4, 8, 5, 3, 2, 3, 2};
        private static readonly double[] RedSeriesData = {0, 0, 1, 2, 3, 1, 2, 6, 9, 9, 10, 6, 5, 13, 4, 8, 8, 4, 3, 4};
        private static readonly double[] GreenSeriesData = {1, 2, 4, 4, 5, 8, 7, 10, 10, 6, 8, 6, 11, 3, 7, 4, 1, 0, 0, 1};

        public HistogramViewModel()
        {
            ChartLabels = new List<HistogramLabelViewModel>();

            // StyleKey in RenderableSeriesViewModel must match style resource keys in XAML 
            RenderableSeriesViewModels = new List<IRenderableSeriesViewModel>
            {
                GenerateColumn(-2.5, BlueSeriesData, "BlueColumnStyle"),
                GenerateAvarage(-2.5, BlueSeriesData, "BlueLineStyle"),
                GenerateColumn(0, RedSeriesData, "RedColumnStyle"),
                GenerateAvarage(0, BlueSeriesData, "RedLineStyle"),
                GenerateColumn(2.5, GreenSeriesData, "GreenColumnStyle"),
                GenerateAvarage(2.5, BlueSeriesData, "GreenLineStyle"),
            };
        }

        public List<IRenderableSeriesViewModel> RenderableSeriesViewModels { get; set; }

        public List<HistogramLabelViewModel> ChartLabels { get; set; }

        private IRenderableSeriesViewModel GenerateColumn(double startsAt, double[] data, string styleKey)
        {
            var dataSeries = new XyDataSeries<double, double>();
            
            var xValues = GeneratexValues(startsAt, data.Length);
            dataSeries.Append(xValues, data);

            // Annotations for text labels
            for (var i = 0; i < data.Length; i++)
            {
                ChartLabels.Add(new HistogramLabelViewModel(xValues[i], data[i],data[i].ToString(CultureInfo.InvariantCulture)));
            }

            return new ColumnRenderableSeriesViewModel
            {
                DataSeries = dataSeries,
                StyleKey = styleKey,
            };
        }

        private IRenderableSeriesViewModel GenerateAvarage(double startsAt, double[] data, string styleKey)
        {
            var dataSeries = new XyDataSeries<double, double>();

            var xValues = GeneratexValues(startsAt, data.Length);
            dataSeries.Append(xValues, data.MovingAverage(3));

            return new LineRenderableSeriesViewModel
            {
                DataSeries = dataSeries,
                StyleKey = styleKey,
            };
        }

        private double[] GeneratexValues(double startsAt, int count)
        {
            var xValues = new double[count];
            for (var i = 0; i < count; i++)
            {
                xValues[i] = startsAt;
                startsAt += 0.25;
            }

            return xValues;
        }
    }
}