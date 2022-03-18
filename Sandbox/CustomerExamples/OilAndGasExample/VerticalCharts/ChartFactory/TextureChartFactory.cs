using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;

namespace OilAndGasExample.VerticalCharts.ChartFactory
{
    public class TextureChartFactory : IChartFactory
    {
        public string Title => "Texture";

        public IAxisViewModel GetXAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "TextureChartXAxisStyle"
            };
        }

        public IAxisViewModel GetYAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "TextureChartYAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(2);

            var dataSeries1 = new XyDataSeries<double>();
            var dataSeries2 = new XyDataSeries<double>();

            using (var fileStream = File.OpenRead("../../Data/Texture.csv.gz"))
            using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gzStream))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');
                        var x = double.Parse(data[0], CultureInfo.InvariantCulture);

                        dataSeries1.Append(x, double.Parse(data[1], CultureInfo.InvariantCulture));
                        dataSeries2.Append(x, 0.0);
                    }

                    line = streamReader.ReadLine();
                }
            }

            var rangePaletteProvider = new RangeFillPaletteProvider(new[]
            {
                new PaletteRange(08, 08, Brushes.Goldenrod),
                new PaletteRange(18, 22, Brushes.DarkCyan),
                new PaletteRange(22, 25, Brushes.Goldenrod),
                new PaletteRange(25, 26, Brushes.DarkCyan),
                new PaletteRange(29, 29, Brushes.DarkCyan),
                new PaletteRange(40, 40, Brushes.Green),
                new PaletteRange(50, 55, Brushes.Green),
                new PaletteRange(55, 58, Brushes.DarkCyan),
                new PaletteRange(70, 75, Brushes.Goldenrod),
                new PaletteRange(75, 76, Brushes.Green),
                new PaletteRange(85, 97, Brushes.DarkCyan)
            });

            renderSeries.Add(new MountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                PaletteProvider = rangePaletteProvider,
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