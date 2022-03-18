using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;

namespace OilAndGasExample.VerticalCharts.ChartFactory
{
    public class PoreSpaceChartFactory : IChartFactory
    {
        public string Title => "Pore Space";

        public IAxisViewModel GetXAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "PoreSpaceChartXAxisStyle"
            };
        }

        public IAxisViewModel GetYAxis()
        {
            return new NumericAxisViewModel
            {
                StyleKey = "PoreSpaceChartYAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(3);

            var dataSeries1 = new XyDataSeries<double>();
            var dataSeries2 = new XyDataSeries<double>();
            var dataSeries3 = new XyDataSeries<double>();

            using (var fileStream = File.OpenRead("../../Data/PoreSpace.csv.gz"))
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

                        if (data[3] != "-") //scatter point marker
                        { 
                            dataSeries3.Append(x, double.Parse(data[3], CultureInfo.InvariantCulture));
                        }
                    }

                    line = streamReader.ReadLine();
                }
            }

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries1,
                StyleKey = "BluePoreSpaceSeriesStyle"
            });

            renderSeries.Add(new StackedMountainRenderableSeriesViewModel
            {
                DataSeries = dataSeries2,
                StyleKey = "OlivePoreSpaceSeriesStyle"
            });

            renderSeries.Add(new XyScatterRenderableSeriesViewModel
            {
                DataSeries = dataSeries3,
                StyleKey = "ScatterPoreSpaceSeriesStyle"
            });

            return renderSeries;
        }
    }
}