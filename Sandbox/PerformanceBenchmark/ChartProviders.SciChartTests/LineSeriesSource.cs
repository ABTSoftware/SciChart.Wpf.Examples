using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Data.Numerics;

namespace ChartProviders.SciChartTests
{
    public class LineSeriesSource
    {
        public static readonly DependencyProperty DataSeriesProperty = DependencyProperty.RegisterAttached
            ("DataSeries", typeof(IEnumerable<IDataSeries>), typeof(LineSeriesSource),
            new PropertyMetadata(default(IEnumerable<IDataSeries>), OnDataSeriesDependencyPropertyChanged));

        public static IEnumerable<IDataSeries> GetDataSeries(UIElement element)
        {
            return (IEnumerable<IDataSeries>)element.GetValue(DataSeriesProperty);
        }

        public static void SetDataSeries(UIElement element, IEnumerable<IDataSeries> value)
        {
            element.SetValue(DataSeriesProperty, value);
        }

        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.RegisterAttached
            ("StrokeThickness", typeof(float), typeof(LineSeriesSource), new PropertyMetadata(1.0f));

        public static float GetStrokeThickness(DependencyObject element)
        {
            return (float)element.GetValue(StrokeThicknessProperty);
        }

        public static void SetStrokeThickness(DependencyObject element, float value)
        {
            element.SetValue(StrokeThicknessProperty, value);
        }

        public static readonly DependencyProperty AntiAliasingProperty = DependencyProperty.RegisterAttached
            ("AntiAliasing", typeof(bool), typeof(LineSeriesSource), new PropertyMetadata(default(bool)));

        public static bool GetAntiAliasing(DependencyObject element)
        {
            return (bool)element.GetValue(AntiAliasingProperty);
        }

        public static void SetAntiAliasing(DependencyObject element, bool value)
        {
            element.SetValue(AntiAliasingProperty, value);
        }

        private static void OnDataSeriesDependencyPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is SciChartSurface sciChartSurface)
            {
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

                    var strokeThckness = GetStrokeThickness(sciChartSurface);
                    var aa = GetAntiAliasing(sciChartSurface);

                    foreach (var dataSeries in itr)
                    {
                        if (dataSeries == null) continue;

                        var rgb = new byte[3];
                        random.NextBytes(rgb);
                        var renderableSeries = new FastLineRenderableSeries()
                        {
                            ResamplingMode = ResamplingMode.MinMax,
                            StrokeThickness = (int)strokeThckness,
                            Stroke = Color.FromArgb(255, rgb[0], rgb[1], rgb[2]),
                            AntiAliasing = aa,
                            DataSeries = dataSeries,
                        };

                        renderSeries.Add(renderableSeries);
                    }

                    sciChartSurface.RenderableSeries = new ObservableCollection<IRenderableSeries>(renderSeries);
                }
            }
        }
    }
}
