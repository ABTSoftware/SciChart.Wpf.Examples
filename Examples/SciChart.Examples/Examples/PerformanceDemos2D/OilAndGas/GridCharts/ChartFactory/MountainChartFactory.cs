// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// MountainChartFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Core.Extensions;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.GridCharts.ChartFactory
{
    public class MountainChartFactory : IChartFactory
    {
        private readonly string _dataFileName;

        public MountainChartFactory(string dataFileName)
        {
            _dataFileName = dataFileName;

            Title = "Mountain";
        }

        public string Title { get; }

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
                StyleKey = "SharedGrowByTopYAxisStyle"
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
                var index = int.Parse(_dataFileName.Last().ToString(), CultureInfo.InvariantCulture);
                var data = DataManager.Instance.LoadOilGasGridData(index);

                foreach (var item in data)
                {
                    if (!item.X1.IsNaN()) dataSeries1.Append(item.X1, item.Y1);
                    if (!item.X2.IsNaN()) dataSeries2.Append(item.X2, item.Y2);
                    if (!item.X3.IsNaN()) dataSeries3.Append(item.X3, item.Y3);
                }
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                StyleKey = "RedStackedMountainSeriesStyle"
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries2,
                StyleKey = "GreenStackedMountainSeriesStyle"
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries3,
                StyleKey = "BlueStackedMountainSeriesStyle"
            });

            return renderSeries;
        }
    }
}