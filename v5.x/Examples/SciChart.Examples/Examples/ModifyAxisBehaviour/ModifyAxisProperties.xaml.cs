// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// ModifyAxisProperties.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Events;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    public partial class ModifyAxisProperties : UserControl
    {
        public ModifyAxisProperties()
        {
            InitializeComponent();
            cboAlignment.Items.Add(AxisAlignment.Right);
            cboAlignment.Items.Add(AxisAlignment.Left);
        }

        public void ModifyAxisPropertiesLoaded(object sender, EventArgs e)
        {
            using (sciChart.SuspendUpdates())
            {
                // Add a data series
                var dataSeries0 = new XyDataSeries<DateTime, double>();
                var dataSeries1 = new OhlcDataSeries<DateTime, double>();
                var dataSeries2 = new XyDataSeries<DateTime, double>();

                var dataSource = DataManager.Instance;

                // Prices are in the format Time, Open, High, Low, Close (all IList)            
                var prices = dataSource.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);

                // Append data to series.                 
                dataSeries0.Append(prices.TimeData, dataSource.Offset(prices.LowData, -1000));
                dataSeries1.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);
                dataSeries2.Append(prices.TimeData, dataSource.ComputeMovingAverage(prices.CloseData, 50));

                columnSeries.DataSeries = dataSeries0;
                ohlcSeries.DataSeries = dataSeries1;
                lineSeries.DataSeries = dataSeries2;

                sciChart.ZoomExtents();
            }
        }

        private void OnMinYAxesChanged(object sender, RoutedEventArgs e)
        {
            double value;
            var isPositiveDouble = double.TryParse((sender as TextBox).Text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) && value >= 0;

            if (!SetVisibleRange(yAxis, isPositiveDouble ? value : yAxis.VisibleRange.Min, yAxis.VisibleRange.Max))
            {
                minValueTb.Text = yAxis.VisibleRange.Min.ToString();
            }
        }

        private void OnMaxYAxesChanged(object sender, RoutedEventArgs e)
        {
            double value;
            var isDouble = double.TryParse((sender as TextBox).Text, NumberStyles.Float, CultureInfo.InvariantCulture, out value) && value >= 0;

            if (!SetVisibleRange(yAxis, yAxis.VisibleRange.Min, isDouble ? value : yAxis.VisibleRange.Max))
            {
                maxValueTb.Text = yAxis.VisibleRange.Max.ToString();
            }
        }

        private bool SetVisibleRange(IAxis axis, IComparable min, IComparable max)
        {
            if (CheckVisibleRange(min, max))
            {
                axis.VisibleRange = RangeFactory.NewWithMinMax(axis.VisibleRange, min, max);

                return true;
            }

            return false;
        }

        private bool CheckVisibleRange(IComparable min, IComparable max)
        {
            return min.CompareTo(max) < 0;
        }

        private void OnXVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
        {
            ((ToValidDateTimeConverter)grid.Resources["ToValidDateTimeConverter"]).XVisibleRange = (DateRange)xAxis.VisibleRange;
        }
    }
}
