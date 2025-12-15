// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// TextureChartFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
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
    public class TextureChartFactory : IChartFactory
    {
        public string Title => "Texture";

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
                StyleKey = "TextureChartYAxisStyle"
            };
        }

        public async Task<IEnumerable<IRenderableSeriesViewModel>> GetSeriesAsync()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(2);

            var dataSeries1 = new XyDataSeries<double>();
            var dataSeries2 = new XyDataSeries<double>();

            var paletteProvider = new RangeFillPaletteProvider(new[]
            {
                new PaletteRange(08, 08, new SolidColorBrush(Color.FromArgb(128, 228, 245, 252))),
                new PaletteRange(18, 22, new SolidColorBrush(Color.FromArgb(145, 103, 189, 175))),
                new PaletteRange(22, 25, new SolidColorBrush(Color.FromArgb(128, 228, 245, 252))),
                new PaletteRange(25, 26, new SolidColorBrush(Color.FromArgb(145, 103, 189, 175))),
                new PaletteRange(29, 29, new SolidColorBrush(Color.FromArgb(145, 103, 189, 175))),
                new PaletteRange(40, 40, new SolidColorBrush(Color.FromArgb(128, 228, 245, 252))),
                new PaletteRange(50, 55, new SolidColorBrush(Color.FromArgb(128, 228, 245, 252))),
                new PaletteRange(55, 58, new SolidColorBrush(Color.FromArgb(145, 103, 189, 175))),
                new PaletteRange(70, 75, new SolidColorBrush(Color.FromArgb(128, 228, 245, 252))),
                new PaletteRange(85, 97, new SolidColorBrush(Color.FromArgb(145, 103, 189, 175)))
            });

            await Task.Run(() =>
            {
                var data = DataManager.Instance.LoadOilGasTextureData();

                for (int i = 0; i < data.Count; i++)
                {
                    var item = data[i];
                    var metadata = paletteProvider.GetMetadataByIndex(i);

                    dataSeries1.Append(item.X1, item.Y1, metadata);
                    dataSeries2.Append(item.X1, 0.0);
                }
            });

            renderSeries.Add(new MountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                PaletteProvider = paletteProvider,
                StyleKey = "TextureMountainSeriesStyle"
            });

            renderSeries.Add(new LineRenderableSeriesViewModel
            {
                DataSeries = dataSeries2,
                StyleKey = "TextureLineSeriesStyle"
            });

            return renderSeries;
        }
    }
}