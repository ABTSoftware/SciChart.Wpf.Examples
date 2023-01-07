using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Numerics;

namespace ChartProviderSciChart_v1_7_2
{
    /// <summary>
    ///     An Attached Behaviour that creates one FastLineRenderableSeries with random colour for each IDataSeries passed in to the DataSeries property
    /// </summary>
    public class LineSeriesSource
    {
        public static readonly DependencyProperty DataSeriesProperty =
            DependencyProperty.RegisterAttached("DataSeries", typeof(IEnumerable<IDataSeries>), typeof(LineSeriesSource), new PropertyMetadata(default(IEnumerable<IDataSeries>), OnDataSeriesDependencyPropertyChanged));

        public static void SetDataSeries(UIElement element, IEnumerable<IDataSeries> value)
        {
            element.SetValue(DataSeriesProperty, value);
        }

        public static IEnumerable<IDataSeries> GetDataSeries(UIElement element)
        {
            return (IEnumerable<IDataSeries>)element.GetValue(DataSeriesProperty);
        }

        private static void OnDataSeriesDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var sciChartSurface = d as SciChartSurface;
            if (sciChartSurface == null) return;

            if (e.NewValue == null)
            {
                sciChartSurface.RenderableSeries.Clear();
                return;
            }

            using (sciChartSurface.SuspendUpdates())
            {
                sciChartSurface.RenderableSeries.Clear();

                var random = new Random();
                var itr = (IEnumerable<IDataSeries>)e.NewValue;
                var renderSeries = new List<IRenderableSeries>();

                // 1.7.2 specific
                var dataset = new DataSeriesSet<double, double>();

                foreach (var dataSeries in itr)
                {
                    if (dataSeries == null) continue;

                    // 1.7.2 specific 
                    dataset.Add(dataSeries);

                    var rgb = new byte[3];
                    random.NextBytes(rgb);
                    var renderableSeries = new FastLineRenderableSeries()
                    {
                        ResamplingMode = ResamplingMode.MinMax,
                        SeriesColor = Color.FromArgb(255, rgb[0], rgb[1], rgb[2]),
                        DataSeries = dataSeries,
                    };

                    renderSeries.Add(renderableSeries);
                }

                // 1.7.2 specific
                sciChartSurface.DataSet = dataset;

                sciChartSurface.RenderableSeries = new ObservableCollection<IRenderableSeries>(renderSeries);
            }

        }
    }
}
