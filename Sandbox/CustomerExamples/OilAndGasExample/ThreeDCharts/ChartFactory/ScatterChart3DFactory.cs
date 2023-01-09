using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Windows.Media;
using OilAndGasExample.Common;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Visuals.RenderableSeries;

namespace OilAndGasExample.ThreeDCharts.ChartFactory
{
    public class ScatterChart3DFactory : IChart3DFactory
    {
        public string Title => "Scatter";

        public string StyleKey => null;

        public IAxis3DViewModel GetXAxis()
        {
            return new NumericAxis3DViewModel
            {
                StyleKey = "Scatter3DAxisStyle"
            };
        }

        public IAxis3DViewModel GetYAxis()
        {
            return new NumericAxis3DViewModel
            {
                StyleKey = "Scatter3DAxisStyle"
            };
        }

        public IAxis3DViewModel GetZAxis()
        {
            return new NumericAxis3DViewModel
            {
                StyleKey = "Scatter3DAxisStyle"
            };
        }

        public IEnumerable<IRenderableSeries3DViewModel> GetSeries()
        {
            var renderSeries = new List<IRenderableSeries3DViewModel>(4);

            var xyzDataSeries1 = new XyzDataSeries3D<double>();
            var xyzDataSeries2 = new XyzDataSeries3D<double>();
            var xyzDataSeries3 = new XyzDataSeries3D<double>();
            var xyzDataSeries4 = new XyzDataSeries3D<double>();

            var colors = new[]
            {
                Color.FromRgb(10, 10, 174),
                Color.FromRgb(41, 100, 186),
                Color.FromRgb(54, 225, 90),
                Color.FromRgb(247, 226, 77),
                Color.FromRgb(184, 233, 70),
                Color.FromRgb(221, 128, 55),
                Color.FromRgb(168, 29, 10)
            };

            Color getColor(double coord)
            {
                var index = (int) Math.Ceiling(coord / 50) - 1;

                return index < colors.Length ? colors[index] : colors[colors.Length - 1];
            }

            using (var fileStream = File.OpenRead("../../../ThreeDCharts/Data/Scatter-XYZ.csv.gz"))
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
                        var y = double.Parse(data[1], CultureInfo.InvariantCulture);
                        var z = double.Parse(data[2], CultureInfo.InvariantCulture);

                        var scale = float.Parse(data[3], CultureInfo.InvariantCulture);

                        xyzDataSeries1.Append(x, y, z, new PointMetadata3D(pointScale: scale));
                        xyzDataSeries2.Append(10.0, y, z, new PointMetadata3D(getColor(y), scale));
                        xyzDataSeries3.Append(x, 10.0, z, new PointMetadata3D(getColor(z), scale));
                        xyzDataSeries4.Append(x, y, 10.0, new PointMetadata3D(getColor(y), scale));
                    }

                    line = streamReader.ReadLine();
                }
            }

            renderSeries.Add(new ScatterRenderableSeries3DViewModel
            {
                DataSeries = xyzDataSeries1,
                StyleKey = "ScatterMarkerColorSeriesStyle"
            });

            renderSeries.Add(new ScatterRenderableSeries3DViewModel
            {
                DataSeries = xyzDataSeries2,
                StyleKey = "ScatterMetadataColorSeriesStyle"
            });

            renderSeries.Add(new ScatterRenderableSeries3DViewModel
            {
                DataSeries = xyzDataSeries3,
                StyleKey = "ScatterMetadataColorSeriesStyle"
            });

            renderSeries.Add(new ScatterRenderableSeries3DViewModel
            {
                DataSeries = xyzDataSeries4,
                StyleKey = "ScatterMetadataColorSeriesStyle"
            });

            return renderSeries;
        }
    }
}
