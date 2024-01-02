// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// InteractionWithAnnotations.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.AnnotateAChart
{
    /// <summary>
    /// Interaction logic for InteractionWithAnnotations.xaml
    /// </summary>
    public partial class InteractionWithAnnotations : UserControl
    {
        public InteractionWithAnnotations()
        {
            InitializeComponent();
        }

        void InteractionWithAnnotationsLoaded(object sender, RoutedEventArgs e)
        {
            var series = new OhlcDataSeries<DateTime, double>();

            var marketDataService = new MarketDataService(DateTime.Now, 5, 5);
            var data = marketDataService.GetHistoricalData(200);
            series.Append(data.Select(x => x.DateTime), data.Select(x => x.Open), data.Select(x => x.High), data.Select(x => x.Low), data.Select(x => x.Close));

            sciChart.RenderableSeries[0].DataSeries = series;
        }
    }
}