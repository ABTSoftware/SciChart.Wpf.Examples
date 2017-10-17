using System;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Model.DataSeries.Waterfall;
using SciChart.Charting3D.PointMarkers;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Core.Helpers;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.ManipulateSeries3DMVVM
{
    public static class ViewModelFactory3D
    {
        private static readonly FasterRandom _random = new FasterRandom(251916);
        private const int Count = 50;
        private static FFT2 _transform = new FFT2();

        public static IRenderableSeries3DViewModel New(string  typeName)
        {

            Type type = GetViewModelType(typeName);
            if (type == typeof(ColumnRenderableSeries3DViewModel))
            {
                return new ColumnRenderableSeries3DViewModel { DataSeries = GetDataSeries(), StyleKey = "ColumnStyle3D" };
            }

            if (type == typeof(ImpulseRenderableSeries3DViewModel))
            {
                return new ImpulseRenderableSeries3DViewModel { DataSeries = GetDataSeries(), StyleKey = "Impulse3DStyle" };
            }

            if (type == typeof(PointLineRenderableSeries3DViewModel))
            {
                return new PointLineRenderableSeries3DViewModel { DataSeries = GetScaledDataSeries(), StyleKey = "PointLine3DStyle" };
            }

            if (type == typeof(MountainRenderableSeries3DViewModel))
            {
                return new MountainRenderableSeries3DViewModel { DataSeries = GetScaledDataSeries(), StyleKey = "Mountain3DStyle" };
            }

            if (type == typeof(SurfaceMeshRenderableSeries3DViewModel))
            {
                return new SurfaceMeshRenderableSeries3DViewModel { DataSeries = GetSurfaceMeshDataSeries(), StyleKey = "SurfaceMeshStyle" };
            }

            if (type == typeof(ScatterRenderableSeries3DViewModel))
            {
                return new ScatterRenderableSeries3DViewModel { DataSeries = GetScaledDataSeries(), StyleKey = "Scatter3DStyle" };
            }

            if (type == typeof(WaterfallRenderableSeries3DViewModel))
            {
                return new WaterfallRenderableSeries3DViewModel { DataSeries = GetWaterfallDataSeries(), StyleKey = "WaterfallStyle" };
            }

            throw new NotImplementedException("Unsupported Series Type.");

        }

        private static XyzDataSeries3D<double> GetDataSeries()
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            for (var i = 1; i < 15; i++)
            {
                for (var j = 1; j <= 15; j++)
                {
                    if (i != j && i % 3 == 0 && j % 3 == 0)
                    {
                        var x = DataManager.Instance.GetGaussianRandomNumber(40, 19);
                        var y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                        var z = DataManager.Instance.GetGaussianRandomNumber(10, 5);

                        var randomColor = Color.FromArgb(0xFF, (byte)_random.Next(1, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255));

                        xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(randomColor));
                    }
                }
            }

            return xyzDataSeries3D;
        }

        private static XyzDataSeries3D<double> GetScaledDataSeries()
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>() { SeriesName = "Colorful Bubble!" };

            const int count = 250;

            var random = new Random(0);

            for (var i = 0; i < count; i++)
            {
                var x = DataManager.Instance.GetGaussianRandomNumber(40, 19);
                var y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                var z = DataManager.Instance.GetGaussianRandomNumber(10, 5);

                var scale = (float)((random.NextDouble() + 0.5) * 3.0);

                Color? randomColor = Color.FromArgb(0xFF, (byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255));

                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(randomColor, scale));
            }

            return xyzDataSeries3D;
        }


        private static UniformGridDataSeries3D<double> GetSurfaceMeshDataSeries()
        {
            var meshDataSeries = new UniformGridDataSeries3D<double>(80, 25) { StepX = 1, StepZ = 1, SeriesName = "Uniform Surface Mesh" };

            for (int x = 0; x < 80; x++)
            {
                for (int z = 0; z < 25; z++)
                {
                    var s = DataManager.Instance.GetGaussianRandomNumber(100, 100);
                    double y = Math.Sin(x * 0.2 * s) / ((z + 1) * 2);
                    y = y >= 0 ? y : 0;
                    meshDataSeries[z, x] = y * Math.Abs(s);
                }
            }

            return meshDataSeries;
        }

        private static WaterfallDataSeries3D<double> GetWaterfallDataSeries()
        {
            var pointsPerSlice = 100;
            var sliceCount = 20;

            var logBase = 10;
            var slicePositions = new double[sliceCount];
            for (int i = 0; i < sliceCount; ++i)
            {
                slicePositions[i] = i;
            }

            var dataSeries = new WaterfallDataSeries3D<double>(pointsPerSlice, slicePositions);
            dataSeries.StartX = 10;
            dataSeries.StepX = 1;

            _transform.init((uint)Math.Log(pointsPerSlice, 2));

            var count = pointsPerSlice * 2;
            var re = new double[count];
            var im = new double[count];

            for (int sliceIndex = 0; sliceIndex < sliceCount; ++sliceIndex)
            {
                for (var i = 0; i < count; i++)
                {
                    re[i] = 2.0 * Math.Sin(2 * Math.PI * i / 20) +
                            5 * Math.Sin(2 * Math.PI * i / 10) +
                            2.0 * _random.NextDouble();
                    im[i] = -10;
                }

                _transform.run(re, im);

                var scaleCoef = Math.Pow(1.5, sliceIndex * 0.3) / Math.Pow(1.5, sliceCount * 0.3);
                for (var pointIndex = 0; pointIndex < pointsPerSlice; pointIndex++)
                {
                    var mag = Math.Sqrt(re[pointIndex] * re[pointIndex] + im[pointIndex] * im[pointIndex]);
                    var yVal = _random.Next(10, 20) * Math.Log10(mag / pointsPerSlice);
                    yVal = (yVal < -25 || yVal > -5)
                        ? (yVal < -25) ? -25 : _random.Next(-6, -3)
                        : yVal;

                    dataSeries[sliceIndex, pointIndex] = -yVal * scaleCoef;
                }
            }

            return dataSeries;
        }


        private static Type GetViewModelType(string seriesType)
        {
            if (seriesType.Equals("Column Series"))
            {
                return typeof(ColumnRenderableSeries3DViewModel);
            }

            if (seriesType.Equals("Impulse Series"))
            {
                return typeof(ImpulseRenderableSeries3DViewModel);
            }

            if (seriesType.Equals("Mountain Series"))
            {
                return typeof(MountainRenderableSeries3DViewModel);
            }

            if (seriesType.Equals("PointLine Series"))
            {
                return typeof(PointLineRenderableSeries3DViewModel);
            }

            if (seriesType.Equals("SurfaceMesh Series"))
            {
                return typeof(SurfaceMeshRenderableSeries3DViewModel);
            }

            if (seriesType.Equals("Waterfall Series"))
            {
                return typeof(WaterfallRenderableSeries3DViewModel);
            }

            if (seriesType.Equals("Scatter Series"))
            {
                return typeof(ScatterRenderableSeries3DViewModel);
            }

            throw new ArgumentException("Not supported type!");
        }
    }
}
