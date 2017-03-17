using System;
using System.Collections.ObjectModel;
using System.Windows.Media;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.Model.DataSeries.Waterfall;
using SciChart.Charting3D.PointMarkers;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Core.Helpers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;


namespace SciChart.Examples.Examples.Charts3D.ManipulateSeries3DMVVM
{
    public class ManipulateSeries3DMvvmViewModel : BaseViewModel
    {
        private ColumnRenderableSeries3DViewModel _columnSeries;
        private MountainRenderableSeries3DViewModel _mountainSeries;
        private PointLineRenderableSeries3DViewModel _pointLineSeries;
        private SurfaceMeshRenderableSeries3DViewModel _surfaceMeshSeries;
        private WaterfallRenderableSeries3DViewModel _waterfallSeries;
        private ScatterRenderableSeries3DViewModel _scatterSeries;
        private ImpulseRenderableSeries3DViewModel _impulseSeries;

        private FasterRandom _random;
        private FFT2 _transform;

        public ManipulateSeries3DMvvmViewModel()
        {
            _random = new FasterRandom();
            _transform = new FFT2();

            _columnSeries = new ColumnRenderableSeries3DViewModel { ColumnShape = typeof(CylinderPointMarker3D), DataPointWidthX = 0.5, Opacity = 1.0, DataSeries = GetColumnDataSeries() };
            _impulseSeries = new ImpulseRenderableSeries3DViewModel { PointMarker = new EllipsePointMarker3D { Fill = Colors.White, Size = 4, Opacity = 1 }, Opacity = 1.0, DataSeries = GetImpulseDataSeries() };
            _pointLineSeries = new PointLineRenderableSeries3DViewModel { IsAntialiased = true, PointMarker = new EllipsePointMarker3D { Fill = Colors.LimeGreen, Size = 2.0f, Opacity = 1 }, DataSeries = GetPointLineDataSeries() };
            _surfaceMeshSeries = new SurfaceMeshRenderableSeries3DViewModel { StyleKey = "SurfaceMeshStyle", DrawMeshAs = DrawMeshAs.SolidWireFrame, StrokeThickness = 2, DrawSkirt = false, Opacity = 1, DataSeries = GetSurfaceMeshDataSeries() };
            _waterfallSeries = new WaterfallRenderableSeries3DViewModel { StyleKey = "WaterfallStyle", Stroke = Colors.Blue, Opacity = 0.8, StrokeThickness = 1, SliceThickness = 0, DataSeries = GetWaterfallDataSeries() };
            _scatterSeries = new ScatterRenderableSeries3DViewModel { PointMarker = new EllipsePointMarker3D { Fill = Colors.LimeGreen, Size = 2.0f, Opacity = 1 }, DataSeries = GetScatterDataSeries() };
            _mountainSeries = new MountainRenderableSeries3DViewModel { DataSeries = GetColumnDataSeries() };

            RenderableSeries = new ObservableCollection<IRenderableSeries3DViewModel>();

            SeriesTypes = new ObservableCollection<string>
            {
                "Column Series",
                "Impulse Series",
                "Mountain Series",
                "PointLine Series",
                "SurfaceMesh Series",
              //  "Waterfall Series",
                "Scatter Series"
            };

            RenderableSeries.Add(_waterfallSeries);
        }

        public ObservableCollection<IRenderableSeries3DViewModel> RenderableSeries { get; set; }
        public ObservableCollection<string> SeriesTypes { get; set; }

        public string SelectedType
        {
            set
            {
                if(!string.IsNullOrEmpty(value))
                    SetSeries(value);
            }
        }

        private XyzDataSeries3D<double> GetScatterDataSeries()
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>() { SeriesName = "Colorful Bubble!" };

            const int count = 250;

            var random = new Random(0);

            for (var i = 0; i < count; i++)
            {
                var x = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                var y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);
                var z = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);

                var scale = (float)((random.NextDouble() + 0.5) * 3.0);

                Color? randomColor = Color.FromArgb(0xFF, (byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255));

                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(randomColor, scale));
            }

            return xyzDataSeries3D;
        }

        public XyzDataSeries3D<double> GetPointLineDataSeries()
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            var random = new Random(0);

            for (var i = 0; i < 100; i++)
            {
                var x = 5 * Math.Sin(i);
                var y = i;
                var z = 5 * Math.Cos(i);

                Color? randomColor = Color.FromArgb(0xFF, (byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255));
                var scale = (float)((random.NextDouble() + 0.5) * 3.0);

                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(randomColor, scale));
            }

            return xyzDataSeries3D;
        }

        public UniformGridDataSeries3D<double> GetSurfaceMeshDataSeries()
        {
            var meshDataSeries = new UniformGridDataSeries3D<double>(25, 25) { StepX = 1, StepZ = 1, SeriesName = "Uniform Surface Mesh" };

            for (int x = 0; x < 25; x++)
            {
                for (int z = 0; z < 25; z++)
                {
                    double y = Math.Sin(x * 0.2) / ((z + 1) * 2);
                    meshDataSeries[z, x] = y;
                }
            }

            return meshDataSeries;
        }

        public XyzDataSeries3D<double> GetColumnDataSeries()
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            for (var i = 1; i < 15; i++)
            {
                for (var j = 1; j <= 15; j++)
                {
                    if (i != j && i % 3 == 0 && j % 3 == 0)
                    {
                        var y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);

                        var randomColor = Color.FromArgb(0xFF, (byte)_random.Next(0, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255));

                        xyzDataSeries3D.Append(i, y, j, new PointMetadata3D(randomColor));
                    }
                }
            }

            return xyzDataSeries3D;
        }

        public XyzDataSeries3D<double> GetImpulseDataSeries()
        {
            var xyzDataSeries3D = new XyzDataSeries3D<double>();

            for (var i = 1; i < 15; i++)
            {
                for (var j = 1; j <= 15; j++)
                {
                    if (i != j && i % 3 == 0 && j % 3 == 0)
                    {
                        var y = DataManager.Instance.GetGaussianRandomNumber(5, 1.5);

                        var randomColor = Color.FromArgb(0xFF, (byte)_random.Next(0, 255), (byte)_random.Next(0, 255), (byte)_random.Next(0, 255));

                        xyzDataSeries3D.Append(i, y, j, new PointMetadata3D(randomColor));
                    }
                }
            }

            return xyzDataSeries3D;
        }

        public WaterfallDataSeries3D<double> GetWaterfallDataSeries()
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

        private void SetSeries(string seriesType)
        {
            RenderableSeries.Clear();

            if (seriesType.Equals("Column Series"))
            {
                RenderableSeries.Add(_columnSeries);
            }
            else if (seriesType.Equals("Impulse Series"))
            {
                RenderableSeries.Add(_impulseSeries);
            }
            else if (seriesType.Equals("Mountain Series"))
            {
                RenderableSeries.Add(_mountainSeries);
            }
            else if (seriesType.Equals("PointLine Series"))
            {
                RenderableSeries.Add(_pointLineSeries);
            }
            else if (seriesType.Equals("SurfaceMesh Series"))
            {
                RenderableSeries.Add(_surfaceMeshSeries);
            }
            // Temporary
            //else if (seriesType.Equals("Waterfall Series"))
            //{
            //    RenderableSeries.Add(_waterfallSeries);
            //}
            else if (seriesType.Equals("Scatter Series"))
            {
                RenderableSeries.Add(_scatterSeries);
            }
        }
    }
}
