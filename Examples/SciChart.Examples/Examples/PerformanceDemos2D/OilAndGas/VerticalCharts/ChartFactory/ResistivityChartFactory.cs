// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ResistivityChartFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Threading.Tasks;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.VerticalCharts.ChartFactory
{
    public class ResistivityChartFactory : IChartFactory
    {
        public string Title => "Resistivity";

        public IAxisViewModel GetXAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "SharedXAxisStyle"
            };
        }

        public IAxisViewModel GetYAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "ResistivityChartYAxisStyle"
            };
        }

        public async Task<IEnumerable<IRenderableSeriesViewModel>> GetSeriesAsync()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(2);

            var dataSeries = new XyDataSeries<double>();

            await Task.Run(() =>
            {
                var data = DataManager.Instance.LoadOilGasResistivityData();

                foreach (var item in data)
                {
                    dataSeries.Append(item.X1, item.Y1);
                }
            });

            renderSeries.Add(new LineRenderableSeriesViewModel
            {
                DataSeries = dataSeries,
                StyleKey = "ResistivitySeriesStyle"
            });

            renderSeries.Add(new LineRenderableSeriesViewModel
            {
                DataSeries = dataSeries.ToMovingAverage(40),
                StyleKey = "ResistivityAverageSeriesStyle"
            });

            return renderSeries;
        }
    }
}