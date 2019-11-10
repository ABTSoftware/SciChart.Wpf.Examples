// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// BoxPlotExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.CreateSimpleChart
{
    /// <summary>
    /// Interaction logic for BoxPlotExampleView.xaml
    /// </summary>
    public partial class BoxPlotExampleView : UserControl
    {
        public BoxPlotExampleView()
        {
            InitializeComponent();

            this.Loaded += (sender, args) =>
                {
                    // Box data is a multi-dimensional table containing minimum, lower quartile, median, upper quartile, maximum Y values vs X
                    IEnumerable<BoxPoint> boxData = GetBoxPlotData(10).ToArray();

                    var boxDataSeries = new BoxPlotDataSeries<DateTime, double>();

                    boxDataSeries.Append(boxData.Select(x => x.Date),
                        boxData.Select(x => x.Median),
                        boxData.Select(x => x.Minimum),
                        boxData.Select(x => x.LowerQuartile),
                        boxData.Select(x => x.UpperQuartile),
                        boxData.Select(x => x.Maximum));

                    boxSeries.DataSeries = boxDataSeries;
                };

            sciChart.ZoomExtents();
        }

        private IEnumerable<BoxPoint> GetBoxPlotData(int count)
        {
            var dates = Enumerable.Range(0, count).Select(i => new DateTime(2011, 01, 01).AddMonths(i)).ToArray();
            var medianValues = new RandomWalkGenerator(0).GetRandomWalkSeries(count).YData;

            var random = new Random(0);
            for (int i = 0; i < count; i++)
            {
                double med = medianValues[i];

                double min = med - random.NextDouble();
                double max = med + random.NextDouble();

                double lower = (med - min)*random.NextDouble() + min;
                double upper = (max - med)*random.NextDouble() + med;

                yield return new BoxPoint(dates[i], min, lower, med, upper, max);
            }
        }

        private struct BoxPoint
        {
            public readonly DateTime Date;
            public readonly double Minimum;
            public readonly double LowerQuartile;
            public readonly double Median;
            public readonly double UpperQuartile;
            public readonly double Maximum;

            public BoxPoint(DateTime date, double min, double lower, double med, double upper, double max)
                : this()
            {
                Date = date;
                Minimum = min;
                LowerQuartile = lower;
                Median = med;
                UpperQuartile = upper;
                Maximum = max;
            }
        }
    }
}
