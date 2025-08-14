// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ShaleChartFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.VerticalCharts.ChartFactory
{
    public class ShaleChartFactory : IChartFactory
    {
        public string Title => "Shale";

        public IAxisViewModel GetXAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "ShaleChartXAxisStyle"
            };
        }

        public IAxisViewModel GetYAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "ShaleChartYAxisStyle"
            };
        }

        public async Task<IEnumerable<IRenderableSeriesViewModel>> GetSeriesAsync()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(3);

            var dataSeries1 = new XyDataSeries<double>();
            var dataSeries2 = new XyDataSeries<double>();
            var dataSeries3 = new XyDataSeries<double>();

            var paletteProvider = new RangeFillPaletteProvider(new[]
            {
                new PaletteRange(100, 180, new SolidColorBrush(Color.FromArgb(128, 000, 120, 120))),
                new PaletteRange(220, 260, new SolidColorBrush(Color.FromArgb(128, 000, 080, 220))),
                new PaletteRange(480, 580, new SolidColorBrush(Color.FromArgb(128, 000, 080, 220))),
                new PaletteRange(600, 640, new SolidColorBrush(Color.FromArgb(128, 000, 120, 120))),
                new PaletteRange(900, 950, new SolidColorBrush(Color.FromArgb(128, 000, 120, 120)))
            });

            await Task.Run(() =>
            {
                var data = DataManager.Instance.LoadOilGasShaleData();

                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    var metadata = paletteProvider.GetMetadataByIndex(i);

                    dataSeries1.Append(item.X1, item.Y1);
                    dataSeries2.Append(item.X2, item.Y2, metadata);
                    dataSeries3.Append(item.X3, item.Y3);
                }
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                StyleKey = "GreenShaleSeriesStyle"
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries2,
                PaletteProvider = paletteProvider,
                StyleKey = "BlueShaleSeriesStyle"
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries3,
                StyleKey = "RedShaleSeriesStyle"
            });

            return renderSeries;
        }
    }
}