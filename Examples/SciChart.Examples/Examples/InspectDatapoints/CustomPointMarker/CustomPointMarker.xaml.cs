// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2021. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// CustomPointMarker.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.InspectDatapoints.CustomPointMarker
{
    /// <summary>
    /// Interaction logic for CustomPointMarker.xaml
    /// </summary>
    public partial class CustomPointMarker : UserControl
    {
        private readonly RandomWalkGenerator _dataSource;

        public CustomPointMarker()
        {
            InitializeComponent();

            var count = 100;

            // Create a DataSeriesSet
            var dataSeries = new XyDataSeries<double, double>();

            // Create a single data-series
            _dataSource = new RandomWalkGenerator();
            var data = _dataSource.GetRandomWalkSeries(count);

            // Create a single metadata
            IList<IPointMetadata> metadata = new List<IPointMetadata>();

            for (int i = 0; i < 100; ++i)
            {
                IPointMetadata pointMetadata = new SelectedPointMetadata();

                if (i % 9 == 0)
                {
                    pointMetadata.IsSelected = true;
                }

                metadata.Add(pointMetadata);
            }

            // Append data to series.
            dataSeries.Append(data.XData, data.YData, metadata);
            fastLineSeries.DataSeries = dataSeries;
        }
    }

    public class SelectedPointMetadata : IPointMetadata
    {
        public bool IsSelected { get; set; }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
    }
}
