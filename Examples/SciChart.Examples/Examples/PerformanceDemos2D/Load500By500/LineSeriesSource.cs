// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
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
using System.Linq;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Charting3D.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Common;

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

        private static readonly SeriesStrokeProvider SeriesStrokeProvider;

        static LineSeriesSource()
        {
            // Picks linearly interpolated series strokes from this list
            SeriesStrokeProvider = new SeriesStrokeProvider();
            SeriesStrokeProvider.StrokePalette = new[]
            {
                Color.FromArgb(0xAA, 0x27, 0x4b, 0x92),
                Color.FromArgb(0xAA, 0x47, 0xbd, 0xe6),
                Color.FromArgb(0xAA, 0xa3, 0x41, 0x8d),
                Color.FromArgb(0xAA, 0xe9, 0x70, 0x64),
                Color.FromArgb(0xAA, 0x68, 0xbc, 0xae),
                Color.FromArgb(0xAA, 0x63, 0x4e, 0x96),
            };
        }

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

                var itr = (IEnumerable<IDataSeries>)e.NewValue;
                var renderSeries = new List<IRenderableSeries>();

                int index = 0;
                int max = itr.Count();
                foreach (var dataSeries in itr)
                {
                    if (dataSeries == null) continue;

                    var renderableSeries = new FastLineRenderableSeries()
                    {
                        AntiAliasing = true,
                        Stroke = SeriesStrokeProvider.GetStroke(index++, max),
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
