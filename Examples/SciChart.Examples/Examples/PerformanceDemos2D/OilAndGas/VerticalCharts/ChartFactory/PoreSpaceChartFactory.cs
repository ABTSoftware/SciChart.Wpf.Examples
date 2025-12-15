// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PoreSpaceChartFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Core.Extensions;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.VerticalCharts.ChartFactory
{
    public class PoreSpaceChartFactory : IChartFactory
    {
        public string Title => "Pore Space";

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
                StyleKey = "PoreSpaceChartYAxisStyle"
            };
        }

        public async Task<IEnumerable<IRenderableSeriesViewModel>> GetSeriesAsync()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(3);

            var dataSeries1 = new XyDataSeries<double>();
            var dataSeries2 = new XyDataSeries<double>();
            var dataSeries3 = new XyDataSeries<double>();

            await Task.Run(() =>
            {
                var data = DataManager.Instance.LoadOilGasPoreSpaceData();

                foreach (var item in data)
                {
                    if (!item.Y1.IsNaN()) dataSeries1.Append(item.X1, item.Y1);
                    if (!item.Y2.IsNaN()) dataSeries2.Append(item.X2, item.Y2);
                    if (!item.Y3.IsNaN()) dataSeries3.Append(item.X3, item.Y3);                   
                }
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                StyleKey = "BluePoreSpaceSeriesStyle"
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries2,
                StyleKey = "OlivePoreSpaceSeriesStyle"
            });

            renderSeries.Add(new XyScatterRenderableSeriesViewModel
            {
                DataSeries = dataSeries3,
                StyleKey = "ScatterPoreSpaceSeriesStyle"
            });

            return renderSeries;
        }
    }
}