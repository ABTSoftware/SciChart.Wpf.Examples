using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using OilAndGasExample.Common;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;

namespace OilAndGasExample.GridCharts.ChartFactory
{
    public class ScatterChartFactory : IChartFactory
    {
        private readonly string _dataFileName;

        public string Title { get; }

        public string StyleKey => null;

        public ScatterChartFactory(string dataFileName)
        {
            _dataFileName = dataFileName;

            Title = dataFileName.Substring(0, dataFileName.IndexOf('.'));
        }

        public IAxisViewModel GetXAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "SharedGrowByXAxisStyle"
            };
        }

        public IAxisViewModel GetYAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "SharedGrowByYAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(3);

            var dataSeries1 = new XyDataSeries<double> { AcceptsUnsortedData = true };
            var dataSeries2 = new XyDataSeries<double> { AcceptsUnsortedData = true };
            var dataSeries3 = new XyDataSeries<double> { AcceptsUnsortedData = true };

            var dataSeries = new[] { dataSeries1, dataSeries2, dataSeries3 };

            using (var fileStream = File.OpenRead(Path.Combine("../../GridCharts/Data", _dataFileName)))
            using (var gzStream = new GZipStream(fileStream, CompressionMode.Decompress))
            using (var streamReader = new StreamReader(gzStream))
            {
                var line = streamReader.ReadLine();

                while (!string.IsNullOrEmpty(line))
                {
                    if (!line.StartsWith("/"))
                    {
                        var data = line.Split(';');

                        for (int i = 0, j = 0; i < data.Length; i += 2, j++)
                        {
                            if (data[i] != "-")
                            {
                                dataSeries[j].Append(double.Parse(data[i], CultureInfo.InvariantCulture),
                                    double.Parse(data[i + 1], CultureInfo.InvariantCulture));
                            }
                        }
                    }

                    line = streamReader.ReadLine();
                }
            }

            renderSeries.Add(new XyScatterRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                StyleKey = "RedScatterSeriesStyle"
            });

            renderSeries.Add(new XyScatterRenderableSeriesViewModel
            {
                DataSeries = dataSeries2,
                StyleKey = "BlueScatterSeriesStyle"
            });

            renderSeries.Add(new XyScatterRenderableSeriesViewModel
            {
                DataSeries = dataSeries3,
                StyleKey = "GreenScatterSeriesStyle"
            });

            return renderSeries;
        }
    }
}