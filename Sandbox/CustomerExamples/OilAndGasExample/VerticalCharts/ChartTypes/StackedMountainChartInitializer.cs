using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;

namespace OilAndGasExample.VerticalCharts.ChartTypes
{
    public class StackedMountainChartInitializer : IVerticalChartInitializer
    {
        public string ChartTitle => "Shale";

        public IAxisViewModel GetXAxis()
        {
            return new NumericAxisViewModel
            {
                AxisAlignment = AxisAlignment.Left,
                StyleKey = "VerticalChartAxisStyle"
            };
        }

        public IAxisViewModel GetYAxis()
        {
            return new NumericAxisViewModel
            {
                FlipCoordinates = true,
                AxisAlignment = AxisAlignment.Bottom,
                StyleKey = "VerticalChartAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(3);

            var dataSeries1 = new XyDataSeries<double>();
            var dataSeries2 = new XyDataSeries<double>();
            var dataSeries3 = new XyDataSeries<double>();

            using (var gz = new GZipStream(File.OpenRead("../../Data/Shale.csv.gz"), CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gz))
            {
                string line = streamReader.ReadLine();

                while (line != null)
                {
                    var data = line.Split(';');
                    var x = double.Parse(data[0], CultureInfo.InvariantCulture);

                    dataSeries1.Append(x, double.Parse(data[1], CultureInfo.InvariantCulture));
                    dataSeries2.Append(x, double.Parse(data[2], CultureInfo.InvariantCulture));
                    dataSeries3.Append(x, double.Parse(data[3], CultureInfo.InvariantCulture));

                    line = streamReader.ReadLine();
                }
            }

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                StyleKey = "GreenStackedMountainStyle",
                IsOneHundredPercent = true
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries2,
                PaletteProvider = new VerticalChartPaletteProvider(new[]
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
                }),
                StyleKey = "YellowStackedMountainStyle",
                IsOneHundredPercent = true
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries3,
                StyleKey = "RedStackedMountainStyle",
                IsOneHundredPercent = true
            });

            return renderSeries;
        }
    }
}
