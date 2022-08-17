using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace LegendAxisVisibilityCheckboxExample
{
    public partial class LegendAxisVisibilityCheckbox : Window
    {
        public LegendAxisVisibilityCheckbox()
        {
            InitializeComponent();

            this.LineSeries0.DataSeries = GetData(0, "Series 0");
            this.LineSeries1.DataSeries = GetData(90, "Series 1");
        }

        private IDataSeries GetData(double phase, string name)
        {
            var dataSeries = new XyDataSeries<double>();
            var xValues = Enumerable.Range(0, 25).Select(x => (double)x).ToArray();
            dataSeries.Append(xValues, xValues.Select(x => Math.Sin(x * 0.1 + phase)));
            dataSeries.SeriesName = name;
            return dataSeries;
        }
    }
}
