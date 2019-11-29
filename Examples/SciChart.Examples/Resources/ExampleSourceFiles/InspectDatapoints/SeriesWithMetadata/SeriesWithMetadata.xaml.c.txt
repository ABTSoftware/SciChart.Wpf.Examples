// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// SeriesWithMetadata.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.InspectDatapoints.SeriesWithMetadata
{
    /// <summary>
    /// Interaction logic for SeriesWithMetadata.xaml
    /// </summary>
    public partial class SeriesWithMetadata : UserControl
    {
        public SeriesWithMetadata()
        {
            InitializeComponent();
        }

        private void OnSeriesWithMetadataLoaded(object sender, RoutedEventArgs e)
        {
            var startDate = new DateTime(1995, 1, 1);

            // Budget data
            var yearsData = Enumerable.Range(0, 18).Select(startDate.AddYears).ToArray();
            var gainLossData = new [] {0, -20.5, -30.06, -70.1, -100.22, 10.34, 30.00, 60.12, 50.1, 70.4, 40.55, 30.76, -50.2, -60.00, -20.01, 50.01, 60.32, 60.44};

            // Metadata
            var executivesData = new [] {"Emerson Irwin", "Reynold Harding", "Carl Carpenter", "Merle Godfrey", "Karl Atterberry"};
            var checkPointIndicies = new []{0, 4, 9, 13, 16};

            var ceo = executivesData[0];
            var budgetMetadata = gainLossData.Select((value, index) =>
            {
                var metadata = new BudgetPointMetadata(value);

                if (checkPointIndicies.Contains(index))
                {
                    metadata.IsCheckPoint = true;

                    var ceoIndex = checkPointIndicies.IndexOf(index);
                    ceo = executivesData[ceoIndex];
                }

                metadata.CEO = ceo;

                return metadata;
            }).ToArray();

            var budgetDataSeries = new XyDataSeries<DateTime, double>();

            budgetDataSeries.Append(yearsData, gainLossData, budgetMetadata);

            lineSeries.DataSeries = budgetDataSeries;

            sciChart.ZoomExtents();
        }
    }
}
