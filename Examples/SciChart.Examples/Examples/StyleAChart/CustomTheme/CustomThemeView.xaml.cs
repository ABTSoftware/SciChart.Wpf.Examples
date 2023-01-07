// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomThemeView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.StyleAChart.CustomTheme
{
    public partial class ChangeThemeView : UserControl
    {
        public ChangeThemeView()
        {
            InitializeComponent();

            var customTheme = new CustomTheme();
            ThemeManager.AddTheme("BerryBlue", customTheme);
            ThemeManager.SetTheme(sciChart, "BerryBlue");
            ThemeManager.SetTheme(overview, "BerryBlue");
        }

        private void ChangeTheme_Loaded(object sender, RoutedEventArgs e)
        {
            using (sciChart.SuspendUpdates())
            {
                // Create some dataseries of type x=DateTime, y=Double
                var dataSeries0 = new XyDataSeries<DateTime, double> {SeriesName = "Mountain Series"};
                var dataSeries1 = new OhlcDataSeries<DateTime, double> {SeriesName = "Candlestick Series"};
                var dataSeries2 = new XyDataSeries<DateTime, double> {SeriesName = "Line Series"};
                var dataSeries3 = new XyDataSeries<DateTime, double> {SeriesName = "Column Series"};

                var dataSource = DataManager.Instance;

                // Prices are in the format Time, Open, High, Low, Close (all IList)            
                var prices = dataSource.GetPriceData(Instrument.Indu.Value, TimeFrame.Daily);

                // Append data to series.                 
                dataSeries0.Append(prices.TimeData, dataSource.Offset(prices.LowData, -1000));
                dataSeries1.Append(prices.TimeData, prices.OpenData, prices.HighData, prices.LowData, prices.CloseData);                
                dataSeries2.Append(prices.TimeData, dataSource.ComputeMovingAverage(prices.CloseData, 50));                
                dataSeries3.Append(prices.TimeData, prices.VolumeData.Select(x => (double) x));

                // Assign data to RenderableSeries
                mountainSeries.DataSeries = dataSeries0;
                candlestickSeries.DataSeries = dataSeries1;
                lineSeries.DataSeries = dataSeries2;
                columnSeries.DataSeries = dataSeries3;

                overview.ParentSurface = sciChart;

                sciChart.ZoomExtents();
            }
        }
    }
}