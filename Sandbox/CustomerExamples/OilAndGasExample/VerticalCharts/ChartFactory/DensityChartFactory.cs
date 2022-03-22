using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;

namespace OilAndGasExample.VerticalCharts.ChartFactory
{
    public class DensityChartFactory : IChartFactory
    {
        public string Title => "Density";

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
                StyleKey = "DensityChartYAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(1);
            var dataSeries = new XyyDataSeries<double>();

            using (var fileStream = File.OpenRead("../../Data/Density.csv.gz"))
            using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gzStream))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        dataSeries.Append(double.Parse(data[0], CultureInfo.InvariantCulture),
                            double.Parse(data[1], CultureInfo.InvariantCulture),
                            double.Parse(data[2], CultureInfo.InvariantCulture));
                    }

                    line = streamReader.ReadLine();
                }
            }

            renderSeries.Add(new BandRenderableSeriesViewModel
            {
                DataSeries = dataSeries,
                StyleKey = "DensitySeriesStyle"
            });

            return renderSeries;
        }
    }
}