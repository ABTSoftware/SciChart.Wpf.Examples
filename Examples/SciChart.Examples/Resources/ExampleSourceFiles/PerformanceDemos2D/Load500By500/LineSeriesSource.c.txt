// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2019. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LineSeriesSource.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.PerformanceDemos2D.Load500By500
{
    /// <summary>
    ///     An Attached Behaviour that creates one FastLineRenderableSeries with random colour for each IDataSeries passed in to the DataSeries property
    /// </summary>
    public class LineSeriesSource
    {
        public static readonly DependencyProperty DataSeriesProperty =
            DependencyProperty.RegisterAttached("DataSeries", typeof(IEnumerable<IDataSeries>),
                                                typeof(LineSeriesSource),
                                                new PropertyMetadata(default(IEnumerable<IDataSeries>),
                                                                     OnDataSeriesDependencyPropertyChanged));

        public static void SetDataSeries(UIElement element, IEnumerable<IDataSeries> value)
        {
            element.SetValue(DataSeriesProperty, value);
        }

        public static IEnumerable<IDataSeries> GetDataSeries(UIElement element)
        {
            return (IEnumerable<IDataSeries>)element.GetValue(DataSeriesProperty);
        }

        private static void OnDataSeriesDependencyPropertyChanged(DependencyObject d,
                                                                  DependencyPropertyChangedEventArgs e)
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
                foreach (var dataSeries in itr)
                {
                    if (dataSeries == null) continue;

                    var rgb = new byte[3];
                    random.NextBytes(rgb);
                    var renderableSeries = new FastLineRenderableSeries()
                    {
                        AntiAliasing = true,
                        Stroke = Color.FromArgb(255, rgb[0], rgb[1], rgb[2]),
                        DataSeries = dataSeries,
                        StrokeThickness = 1,
                    };

                    renderSeries.Add(renderableSeries);
                }

                sciChartSurface.RenderableSeries = new ObservableCollection<IRenderableSeries>(renderSeries);
            }
        }
    }
}
