using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using OilAndGasExample.Common;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;

namespace OilAndGasExample.VerticalCharts.ChartFactory
{
    public class SonicChartFactory : IChartFactory
    {
        public string Title => "Sonic";
        
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
                StyleKey = "SonicChartYAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(1);
            var heatmapData = new double[100, 1000];

            using (var fileStream = File.OpenRead("../../VerticalCharts/Data/Sonic.csv.gz"))
            using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gzStream))
            {
                var i = 0;
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        for (int j = 0; j < 1000; j++)
                        {
                            heatmapData[i, j] = double.Parse(data[j], CultureInfo.InvariantCulture);
                        }

                        i++;
                    }

                    line = streamReader.ReadLine();
                }
            }

            var dataSeries = new UniformHeatmapDataSeries<int, int, double>(heatmapData, 0, 1, 0, 1);

            renderSeries.Add(new UniformHeatmapRenderableSeriesViewModel
            {
                DataSeries = dataSeries,
                StyleKey = "SonicSeriesStyle"
            });

            return renderSeries;
        }
    }
}