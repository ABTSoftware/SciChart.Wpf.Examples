using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Windows.Media;
using OilAndGasExample.Common;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;

namespace OilAndGasExample.VerticalCharts.ChartFactory
{
    public class ShaleChartFactory : IChartFactory
    {
        public string Title => "Shale";
        
        public string StyleKey => "ShaleSurfaceStyle";

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

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(3);

            var dataSeries1 = new XyDataSeries<double>();
            var dataSeries2 = new XyDataSeries<double>();
            var dataSeries3 = new XyDataSeries<double>();

            var paletteProvider = new RangeFillPaletteProvider(new[]
            {
                new PaletteRange(000, 100, Brushes.Orange),
                new PaletteRange(150, 200, Brushes.Orange),
                new PaletteRange(220, 260, Brushes.Blue),
                new PaletteRange(260, 280, Brushes.Red),
                new PaletteRange(280, 350, Brushes.Orange),
                new PaletteRange(400, 420, Brushes.LimeGreen),
                new PaletteRange(480, 580, Brushes.Blue),
                new PaletteRange(600, 620, Brushes.Aqua),
                new PaletteRange(750, 800, Brushes.Orange),
                new PaletteRange(820, 840, Brushes.LimeGreen),
                new PaletteRange(900, 950, Brushes.Aqua)
            });

            using (var fileStream = File.OpenRead("../../VerticalCharts/Data/Shale.csv.gz"))
            using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gzStream))
            {
                var index = 0;
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');
                        var x = double.Parse(data[0], CultureInfo.InvariantCulture);
                        var metadata = paletteProvider.GetMetadataByIndex(index);

                        dataSeries1.Append(x, double.Parse(data[1], CultureInfo.InvariantCulture));
                        dataSeries2.Append(x, double.Parse(data[2], CultureInfo.InvariantCulture), metadata);
                        dataSeries3.Append(x, double.Parse(data[3], CultureInfo.InvariantCulture));

                        index++;
                    }

                    line = streamReader.ReadLine();
                }
            }

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                StyleKey = "GreenShaleSeriesStyle"
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries2,
                PaletteProvider = paletteProvider,
                StyleKey = "YellowShaleSeriesStyle"
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
