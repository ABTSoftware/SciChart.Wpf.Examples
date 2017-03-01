// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
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
using SciChart.Core.Helpers;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    public class ErrorSeriesExampleViewModel : BaseViewModel
    {
        private double _dataPointWidth = 0.7d;
        private int _strokeThickness = 1;

        public ErrorSeriesExampleViewModel()
        {
            // Generate some data to display 
            var data = DataManager.Instance.GetFourierSeriesZoomed(1.0, 0.1, 5.0, 5.15);

            // Append data to series. SciChart automatically redraws
            DataSeries0 = new HlcDataSeries<double, double>();
            DataSeries1 = new HlcDataSeries<double, double>();

            FillSeries(DataSeries0, data, 1.0);
            FillSeries(DataSeries1, data, 1.3);
        }

        private void FillSeries(HlcDataSeries<double, double> hlcDataSeries, DoubleSeries sourceData, double scale)
        {
            var xData = sourceData.XData;
            var yData = sourceData.YData.Select(x => x*scale).ToArray();

            // Generate some random error data. Errors must be absolute values, 
            // e.g. if a series has a Y-value of 5.0, and YError of =/-10% then you must enter YErrorHigh=5.5, YErrorLow=4.5 into the HlcDataSeries
            var random = new FasterRandom();
            var yErrorHigh = yData.Select(y => y + random.NextDouble()*0.2);
            var yErrorLow = yData.Select(y => y - random.NextDouble()*0.2);

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
            get { return _dataPointWidth; }
            set
            {
                _dataPointWidth = value;
                OnPropertyChanged("DataPointWidth");
            }
        }

        public int StrokeThickness
        {
            get { return _strokeThickness; }
            set
            {
                _strokeThickness = value;
                OnPropertyChanged("StrokeThickness");
            }
        }
    }
}