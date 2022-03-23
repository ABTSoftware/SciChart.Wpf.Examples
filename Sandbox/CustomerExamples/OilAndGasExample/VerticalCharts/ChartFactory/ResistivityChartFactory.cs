using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using OilAndGasExample.Common;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;

namespace OilAndGasExample.VerticalCharts.ChartFactory
{
    public class ResistivityChartFactory : IChartFactory
    {
        public string Title => "Resistivity";
        
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
                StyleKey = "ResistivityChartYAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeriesViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeriesViewModel>(2);
            var dataSeries = new XyDataSeries<double>();

            using (var fileStream = File.OpenRead("../../Data/Resistivity.csv.gz"))
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
                            double.Parse(data[1], CultureInfo.InvariantCulture));
                    }

                    line = streamReader.ReadLine();
                }
            }

            renderSeries.Add(new LineRenderableSeriesViewModel
            {
                DataSeries = dataSeries,
                StyleKey = "ResistivitySeriesStyle"
            });

            renderSeries.Add(new LineRenderableSeriesViewModel
            {
                DataSeries = dataSeries.ToMovingAverage(40),
                StyleKey = "ResistivityAverageSeriesStyle"

            });

            return renderSeries;
        }
    }
}