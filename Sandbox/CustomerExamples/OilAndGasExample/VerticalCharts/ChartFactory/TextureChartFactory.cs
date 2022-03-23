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
    public class TextureChartFactory : IChartFactory
    {
        public string Title => "Texture";
        
        public string StyleKey => null;

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

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(2);

            var dataSeries1 = new XyDataSeries<double>();
            var dataSeries2 = new XyDataSeries<double>();

            var paletteProvider = new RangeFillPaletteProvider(new[]
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

            using (var fileStream = File.OpenRead("../../VerticalCharts/Data/Texture.csv.gz"))
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

                        dataSeries1.Append(x, double.Parse(data[1], CultureInfo.InvariantCulture), metadata);
                        dataSeries2.Append(x, 0.0);
                        
                        index++;
                    }

                    line = streamReader.ReadLine();
                }
            }
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