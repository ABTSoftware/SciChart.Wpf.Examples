using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Management.Instrumentation;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Common.Extensions;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Events;
using SciChart.Charting.Visuals.PaletteProviders;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Core.Helpers;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;
using SciChart.Examples.ExternalDependencies.Helpers;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.WaterfallChart
{
    /// <summary>
    ///     Interaction logic for WaterfallChart.xaml
    /// </summary>
    public partial class WaterfallChart : UserControl
    {
        // A drop in replacement for System.Random which is 3x faster: https://www.codeproject.com/Articles/9187/A-fast-equivalent-for-System-Random
        private readonly Random _random = new Random();
        private readonly FFT2 _transform;
        private VerticalValuesPaletteProvider _paletteProvider;
        private XyDataSeries<double, double> _emptySeries;
        

        public WaterfallChart()
        {
            InitializeComponent();
            _transform = new FFT2();
            _transform.init(10);
            _paletteProvider = new VerticalValuesPaletteProvider();
        }

        private void SciChart_OnLoaded(object sender, RoutedEventArgs e)
        {
            var count = 50;
            var renderableSeries = new ObservableCollection<IRenderableSeries>();
            sciChart.YAxes.Clear();
            sciChart.XAxes.Clear();

            for (var i = 0; i < count; i++)
            {
                var yAxis = new CustomWaterfallNumericAxis();
                yAxis.offset = 3.0 * -i;
                yAxis.min = -60.0;
                yAxis.max = 60;
                yAxis.Id = "AxisY" + i;
                yAxis.AutoRange = AutoRange.Once;
                yAxis.DrawMinorTicks = false;
                yAxis.DrawMinorGridLines = false;
                yAxis.DrawMajorGridLines = false;
                yAxis.DrawMajorBands = false;
                yAxis.DrawMajorTicks = false;
                yAxis.AxisAlignment = AxisAlignment.Left;
                yAxis.BorderBrush = Brushes.Black;
                yAxis.BorderThickness = new Thickness(0, 0, 1, 0);
                yAxis.Margin = new Thickness(0, sciChart.ActualHeight * 0.6, 0, 0);

                var xAxis = new CustomWaterfallNumericAxis();
                xAxis.offset = 2.0 * i;
                xAxis.max = 10.0;
                xAxis.Id = "AxisX" + i;
                xAxis.DrawMinorTicks = false;
                xAxis.DrawMinorGridLines = false;
                xAxis.DrawMajorGridLines = false;
                xAxis.DrawMajorBands = false;
                xAxis.DrawMajorTicks = false;
                xAxis.BorderThickness = new Thickness(0, 1, 0, 0);
                xAxis.BorderBrush = Brushes.Black;
                xAxis.Margin = new Thickness(0, 0, sciChart.ActualWidth * 0.1, 0);

                var series = new FastLineRenderableSeries();
                series.YAxisId = yAxis.Id;
                series.PaletteProvider = _paletteProvider;
                series.XAxisId = xAxis.Id;
                series.DataSeries = UpdateXyDataSeries();
                series.Stroke = Colors.DarkGreen;
                if (i != 0)
                {
                    xAxis.Visibility = Visibility.Hidden;
                    xAxis.Height = 0.0;

                    yAxis.Visibility = Visibility.Hidden;
                    yAxis.Width = 0.0;
                }
                sciChart.YAxes.Add(yAxis);
                sciChart.XAxes.Add(xAxis);

                renderableSeries.Add(series);
            }

            sciChart.RenderableSeries = renderableSeries;
            sciChart.RenderableSeries[25].IsSelected = true;
            sciChart.XAxes[0].VisibleRangeChanged += OnVisibleRangeChanged;

            OnSeriesSelect();
            OnAxisMarkerAnnotationMove();
        }
        private void OnSeriesSelect()
        {
            var selectedSeries = sciChart.SelectedRenderableSeries.FirstOrDefault();
            if (selectedSeries != null)
            {
                LineRenderableSeries.DataSeries = selectedSeries.DataSeries;
            }
            else
            {
                LineRenderableSeries.DataSeries = _emptySeries;
            }
        }

        private void OnAxisMarkerAnnotationMove()
        {
            var doubleSeries = new XyDataSeries<double, double>();
            var axisInfo = AxisMarkerAnnotation.AxisInfo;
            var dataIndex = 0;

            if (axisInfo != null)
            {
                var position = (double)axisInfo.DataValue;
                dataIndex = sciChart.RenderableSeries[0].DataSeries.XValues.FindIndex(true, position, SearchMode.Nearest);
            }
            else dataIndex = Convert.ToInt32(AxisMarkerAnnotation.X1);

            if (_paletteProvider != null)
            {
                _paletteProvider.SelectedIndex = dataIndex;
            }

            for (var i = 0; i < sciChart.RenderableSeries.Count; i++)
            {
                doubleSeries.Append(i, (double)sciChart.RenderableSeries[i].DataSeries.YValues[dataIndex]);
                _paletteProvider.OnBeginSeriesDraw(sciChart.RenderableSeries[i]);
            }

            if (sciChart3 != null && sciChart3.RenderableSeries.Any())
            {
                sciChart3.RenderableSeries[0].DataSeries = doubleSeries;
                sciChart3.ZoomExtents();
            }
        }

        private XyDataSeries<double, double> UpdateXyDataSeries()
        {
            var doubleSeries = new XyDataSeries<double, double>();

            var _re = new double[1024];
            var _im = new double[1024];

            for (var i = 0; i < 1024; i++)
            {
                _re[i] = 2.0 * Math.Sin(2 * Math.PI * i / 20) +
                         5 * Math.Sin(2 * Math.PI * i / 10) +
                         2.0 * _random.NextDouble();
                _im[i] = -10;
            }

            _transform.run(_re, _im);
            var _re2 = new double[500];
            var _im2 = new double[500];
            for (var i = 0; i < 500; i++)
            {
                var mag = Math.Sqrt(_re[i] * _re[i] + _im[i] * _im[i]);
                var yVal = 20 * Math.Log10(mag / 500);
                _re2[i] = (yVal < -25 || yVal > -5)
                    ? (yVal < -25) ? -25 : _random.Next(-6, -3)
                    : yVal;

                _im2[i] = i;
            }
            _re2[0] = -25;
            doubleSeries.Append(_im2, _re2);

            return doubleSeries;
        }

        private void SeriesSelectionModifier_OnSelectionChanged(object sender, EventArgs e)
        {
            OnSeriesSelect();
        }

        private void AxisMarkerAnnotation_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            using (sciChart.SuspendUpdates())
            {
                customRubberBandButton.IsChecked = false;
                OnAxisMarkerAnnotationMove();
            }
        }

        private void OnVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
        {
            if (sender.Equals(sciChart2NumericXAxis))
            {
                sciChart.XAxes.ForEachDo(x => x.VisibleRange = e.NewVisibleRange);
            }
            else sciChart2NumericXAxis.VisibleRange = e.NewVisibleRange;
        }

    }
}