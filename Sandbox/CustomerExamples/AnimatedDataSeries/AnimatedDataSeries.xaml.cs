using System;
using System.Windows;
using SciChart.Charting.Model.DataSeries;

namespace AnimatedDataSeriesFilter
{
    public partial class AnimatedDataSeries : Window
    {
        public AnimatedDataSeries()
        {
            InitializeComponent();

            var originalData = new XyDataSeries<double, double>();
            int count = 100;
            for (int i = 0; i < count; i++)
            {
                originalData.Append(i, Math.Sin(i*0.2));
            }

            lineSeries.DataSeries = new global::AnimatedDataSeriesFilterExample.AnimatedDataSeriesFilter(originalData).FilteredDataSeries;
        }
    }
}
