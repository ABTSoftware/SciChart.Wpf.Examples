using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting3D.Model.DataSeries.Waterfall;
using SciChart.Data.Model;
using SciChart.Data.Numerics;
using SciChart.Data.Numerics.PointResamplers;


namespace SciChart.Sandbox.Examples.ResamplingOfGridDataSeries3D
{
    /// <summary>
    /// Interaction logic for ResamplingInGridDataSeries3D.xaml
    /// </summary>
    [TestCase("Resampling of Grid Data Series 3D")]
    public partial class ResamplingOfGridDataSeries3D : UserControl
    {
        public ResamplingOfGridDataSeries3D()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var pointsPerSlice = 65000;
            var sliceCount = 10;

            var logBase = 10;
            var slicePositions = new double[sliceCount];
            for (int i = 0; i < sliceCount; ++i)
            {
                slicePositions[i] = Math.Pow(logBase, i);
            }

            var dataSeries = new WaterfallDataSeries3D<double>(pointsPerSlice, slicePositions) { SeriesName = "Waterfall" };
            dataSeries.StartX = 10;
            dataSeries.StepX = 1;

            FillDataSeries(dataSeries, sliceCount, pointsPerSlice);

            var resampleDataSeries = ResampleDataSeries(dataSeries, slicePositions);

            WaterfallSeries.DataSeries = resampleDataSeries;
        }

        private void FillDataSeries(WaterfallDataSeries3D<double> dataSeries, int sliceCount, int pointsPerSlice)
        {
            for (int sliceIndex = 0; sliceIndex < sliceCount; ++sliceIndex)
            {
                var scaleCoef = Math.Pow(1.5, sliceIndex * 0.3) / Math.Pow(1.5, sliceCount * 0.3);
                for (var pointIndex = 0; pointIndex < pointsPerSlice; pointIndex++)
                {
                    dataSeries[sliceIndex, pointIndex] = (Math.Sin(10 * Math.PI * pointIndex / pointsPerSlice) + 1.0) * scaleCoef;
                }
            }
        }

        private WaterfallDataSeries3D<double> ResampleDataSeries(WaterfallDataSeries3D<double> dataSeries, double[] slicePositions)
        {
            var pointsPerSlice = dataSeries.XSize;
            var sliceCount = dataSeries.ZSize;

            var resamplerFactory = new PointResamplerFactory();

            var resamplingParams = new ResamplingParams
            {
                IsSortedData = true,
                IsEvenlySpacedData = true,
                IsCategoryData = false,
                ContainsNaN = false,
                ZeroLineY = 0.0,
                PointRange = new IndexRange(0, pointsPerSlice - 1),
                VisibleRange = dataSeries.XRange,
                ViewportWidth = Width > 0.0 ? (int) Width : 640,
                ResamplingPrecision = 1
            };

            var resampledSeriesesArray = new IPointSeries[sliceCount];
            var sliceAsXySeries = new XyDataSeries<double>(pointsPerSlice);
            var sliceAsXySeriesYValues = sliceAsXySeries.YValues as ISciList<double>;
            sliceAsXySeriesYValues.SetCount(pointsPerSlice);

            var xValues = new double[pointsPerSlice];
            var xRangeMin = (double)dataSeries.XRange.Min;
            var stepX = dataSeries.StepX;
            for (int i = 0; i < pointsPerSlice; i++)
            {
                xValues[i] = xRangeMin + stepX * i;
            }

            // Init the XY series by first slice of the waterfall data series 
            sliceAsXySeries.InsertRange(0, xValues, dataSeries.InternalArray[0]);
            resampledSeriesesArray[0] = sliceAsXySeries.ToPointSeries(resamplingParams, ResamplingMode.Auto, resamplerFactory, null);

            for (int i = 1; i < sliceCount; i++)
            {
                var curRow = dataSeries.GetRowAt(i);
                Array.Copy(curRow, sliceAsXySeriesYValues.ItemsArray, pointsPerSlice);

                resampledSeriesesArray[i] = sliceAsXySeries.ToPointSeries(resamplingParams, ResamplingMode.Auto, resamplerFactory, null);
            }

            var resampledPerPointsPerSlice = resampledSeriesesArray[0].Count;
            var resampledDataSeries = new WaterfallDataSeries3D<double>(resampledPerPointsPerSlice, slicePositions);
            resampledDataSeries.StartX = dataSeries.StartX;
            resampledDataSeries.StepX = dataSeries.StepX;
            for (int i = 0; i < sliceCount; i++)
            {
                resampledDataSeries.SetRowAt(i, resampledSeriesesArray[i].YValues.Items);
            }

            return resampledDataSeries;
        }
    }
}
