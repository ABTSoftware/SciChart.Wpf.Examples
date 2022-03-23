using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using OilAndGasExample.Common;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;

namespace OilAndGasExample.GridCharts.ChartFactory
{
    public class MountainChartFactory : IChartFactory
    {
        private readonly string _dataFileName;

        public string Title { get; }

        public string StyleKey => null;

        public MountainChartFactory(string dataFileName)
        {
            _dataFileName = dataFileName;

            Title = dataFileName.Substring(0, dataFileName.IndexOf('.'));
        }

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
                StyleKey = "SharedYAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(3);

            var dataSeries1 = new XyDataSeries<double>();
            var dataSeries2 = new XyDataSeries<double>();
            var dataSeries3 = new XyDataSeries<double>();

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
                        var x = double.Parse(data[0], CultureInfo.InvariantCulture);

                        dataSeries1.Append(x, double.Parse(data[1], CultureInfo.InvariantCulture));
                        dataSeries2.Append(x, double.Parse(data[2], CultureInfo.InvariantCulture));
                        dataSeries3.Append(x, double.Parse(data[3], CultureInfo.InvariantCulture));
                    }

                    line = streamReader.ReadLine();
                }
            }

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