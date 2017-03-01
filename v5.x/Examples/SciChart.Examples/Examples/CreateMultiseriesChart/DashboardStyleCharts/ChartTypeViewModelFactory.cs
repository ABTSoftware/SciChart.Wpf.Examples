// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ChartTypeViewModelFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.DashboardStyleCharts
{
    public static class ChartTypeViewModelFactory
    {
        public static ChartTypeViewModel New(Type type, bool isOneHundredPercent, bool isSideBySide)
        {
            var xValues = DashboardDataHelper.GetXValues();
            var yValues = DashboardDataHelper.GetYValues();
            var columnStyleKeys = DashboardDataHelper.GetColumnStyleKeys();
            var mountainStyleKeys = DashboardDataHelper.GetMountainStyleKeys();

            var rSeriesViewModel = new List<IRenderableSeriesViewModel>();
            for (int i = 0; i < 5; i++)
            {
                var dataSeries = new XyDataSeries<double, double> {SeriesName = "Series " + (i + 1)};
                dataSeries.Append(xValues, yValues[i]);

                IRenderableSeriesViewModel seriesViewModel;
                if (type == typeof (IStackedColumnRenderableSeries))
                {
                    seriesViewModel = isSideBySide
                        ? GetStackedColumn(dataSeries, isOneHundredPercent, columnStyleKeys[i], i.ToString())
                        : GetStackedColumn(dataSeries, isOneHundredPercent, columnStyleKeys[i], "");
                }
                else
                {
                    seriesViewModel = GetStackedMountain(dataSeries, isOneHundredPercent, mountainStyleKeys[i]);
                }
                rSeriesViewModel.Add(seriesViewModel);
            }

            return new ChartTypeViewModel(rSeriesViewModel, type, isOneHundredPercent, isSideBySide);
        }

        private static IRenderableSeriesViewModel GetStackedColumn(IXyDataSeries dataSeries, bool isOneHundredPercent, string styleKey, string group)
        {
            return new StackedColumnRenderableSeriesViewModel
            {
                DataSeries = dataSeries,
                StyleKey = styleKey,
                IsOneHundredPercent = isOneHundredPercent,
                StackedGroupId = group,
            };
        }

        private static IRenderableSeriesViewModel GetStackedMountain(IXyDataSeries dataSeries, bool isOneHundredPercent, string styleKey)
        {
            return new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries,
                StyleKey = styleKey,
                IsOneHundredPercent = isOneHundredPercent,
            };
        }
    }
}