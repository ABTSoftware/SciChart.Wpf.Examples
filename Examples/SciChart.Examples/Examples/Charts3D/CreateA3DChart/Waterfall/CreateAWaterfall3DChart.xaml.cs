using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D.Model.DataSeries.Waterfall;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    /// <summary>
    /// Interaction logic for CreateAContouredChart.xaml
    /// </summary>
    public partial class CreateAWaterfall3DChart : UserControl
    {
        private readonly FFT2 _transform = new FFT2();
        private readonly Random _random = new Random();

        public CreateAWaterfall3DChart()
        {
            InitializeComponent();
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var pointsPerSlice = 100;
            var sliceCount = 20;

            var logBase = 10;
            var slicePositions = new double[sliceCount];

            for (int i = 0; i < sliceCount; ++i)
            {
                slicePositions[i] = Math.Pow(logBase, i);
            }

            var dataSeries = new WaterfallDataSeries3D<double>(pointsPerSlice, slicePositions)
            {
                SeriesName = "Waterfall",
                StartX = 10,
                StepX = 1
            };

            _transform.init((uint)Math.Log(pointsPerSlice, 2));

            FillDataSeries(dataSeries, sliceCount, pointsPerSlice);
            WaterfallSeries.DataSeries = dataSeries;
        }

        private void FillDataSeries(WaterfallDataSeries3D<double> dataSeries, int sliceCount, int pointsPerSlice)
        {
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
                    var yVal = _random.Next(10,20) * Math.Log10(mag / pointsPerSlice);
                    yVal = (yVal < -25 || yVal > -5)
                        ? (yVal < -25) ? -25 : _random.Next(-6, -3)
                        : yVal;

                    dataSeries[sliceIndex, pointIndex] = -yVal * scaleCoef;
                }
            }
        }
    }
}