using System;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting3D.Model.DataSeries.Waterfall;

namespace DoubleAxisAsDateTimeAxisExample
{
    public partial class DoubleAxisAsDateTimeAxis : UserControl
    {
        public DoubleAxisAsDateTimeAxis()
        {
            InitializeComponent();

            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var pointsPerSlice = 100;
            var sliceCount = 10;

            var slicePositions = new double[sliceCount];
            var dtNow = DateTime.Now;
            for (int i = 0; i < sliceCount; ++i)
            {
                var date = dtNow.AddMinutes(10 * i).ToOADate();
                slicePositions[i] = date;
            }

            var dataSeries = new WaterfallDataSeries3D<double>(pointsPerSlice, slicePositions) { SeriesName = "Waterfall" };
            dataSeries.StartX = 10;
            dataSeries.StepX = 1;

            FillDataSeries(dataSeries, sliceCount, pointsPerSlice);

            WaterfallSeries.DataSeries = dataSeries;
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
    }
}
