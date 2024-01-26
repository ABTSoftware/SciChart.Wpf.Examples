// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ErrorSeriesExampleViewModel.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************

using System;
using System.Linq;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    public class ErrorSeriesExampleViewModel : BaseViewModel
    {
        private double _dataPointWidth = 10;
        private int _strokeThickness = 1;

        public ErrorSeriesExampleViewModel()
        {
            // Generate some data to display 
            DataManager.Instance.SetRandomSeed(seed: 0);
            var data0 = DataManager.Instance.GetExponentialCurve(40);

            // Append data to series. SciChart automatically redraws
            DataSeries0 = new HlcDataSeries<double, double>();
            DataSeries1 = new HlcDataSeries<double, double>();

            FillSeries(DataSeries0, data0, 1.0, 0.0, 4.0, false);
            FillSeries(DataSeries1, data0, 2.0, 10.0, 2.0, true);
        }

        private void FillSeries(HlcDataSeries<double, double> hlcDataSeries, DoubleSeries sourceData, double yDataScale, double yOffset, double errorScale, bool isHorizontalError)
        {
            var xData = sourceData.XData;
            var yData = sourceData.YData.Select(x => x * yDataScale + yOffset).ToArray();

            // Generate some random error data. Errors must be absolute values, 
            // e.g. if a series has a Y-value of 5.0, and YError of =/-10% then you must enter YErrorHigh=5.5, YErrorLow=4.5 into the HlcDataSeries
            var random = new RandomWalkGenerator(seed: 0);

            var errorBase = isHorizontalError ? xData : yData;
            var yErrorHigh = errorBase.Select((y, index) => index % 8 == 0 ? double.NaN : y + (random.GetRandomDouble() * errorScale));
            var yErrorLow = errorBase.Select((y, index) => index % 10 == 0 ? double.NaN : y - (random.GetRandomDouble() * errorScale));

            // HlcDataSeries requires X, Y, High, Low. 
            // For Error bars the High, Low becomes the High Low error, 
            // while X,Y is the location of the error
            hlcDataSeries.Append(xData, yData, yErrorHigh, yErrorLow);
        }

        // Note: No need to implement INotifyPropertyChanged here since we're not updating the series instances
        public HlcDataSeries<double, double> DataSeries0 { get; private set; }
        public HlcDataSeries<double, double> DataSeries1 { get; private set; }

        public double DataPointWidth
        {
            get => _dataPointWidth;
            set
            {
                _dataPointWidth = value;
                OnPropertyChanged(nameof(DataPointWidth));
            }
        }

        public int StrokeThickness
        {
            get => _strokeThickness;
            set
            {
                _strokeThickness = value;
                OnPropertyChanged(nameof(StrokeThickness));
            }
        }
    }
}