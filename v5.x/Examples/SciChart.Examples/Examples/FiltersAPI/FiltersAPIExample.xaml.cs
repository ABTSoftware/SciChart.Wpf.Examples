// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LogarithmicAxisView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
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
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;
using SciChart.Charting.Model.Filters;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.Examples.ZoomAndPanAChart;

namespace SciChart.Examples.Examples.FiltersAPI
{
    /// <summary>
    /// Interaction logic for FiltersAPIExample.xaml
    /// </summary>
    public partial class FiltersAPIExample : UserControl
    {
        private readonly XyDataSeries<double, double> _originalData = new XyDataSeries<double,double>();

        public FiltersAPIExample()
        {
            InitializeComponent();
            
            OnUpdatedData(1.0d, 0.01d);

            var filteredDataLinearTrendline = _originalData.ToLinearTrendline();
            var filteredDataPolynomialTrendline = _originalData.ToPolynomialTrend(3);
            var filteredDataScale = _originalData.Scale(0.5);
            var filteredDataOffset = _originalData.Offset(-0.2);
            //var filteredDataCustom = new CustomFilter(_originalData);

            _originalData.SeriesName = "Original Data";
            filteredDataLinearTrendline.SeriesName = "Linear Trendline";
            filteredDataPolynomialTrendline.SeriesName = "Polynomial (3rd Order)";
            filteredDataScale.SeriesName = "Scaled";
            filteredDataOffset.SeriesName = "Offset";
            //filteredDataCustom.FilteredDataSeries.SeriesName = "Custom Filter";

            sciChart.RenderableSeries.Add(new XyScatterRenderableSeries() { DataSeries = _originalData, Stroke=Colors.Red, PointMarker = new EllipsePointMarker() { Fill = Colors.Red, Stroke=Colors.Red}});
            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()  { DataSeries = filteredDataLinearTrendline, StrokeThickness = 2, Stroke = Colors.GreenYellow });
            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()  { DataSeries = filteredDataPolynomialTrendline, StrokeThickness = 2, Stroke = Colors.Yellow });
            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()  { DataSeries = filteredDataScale, StrokeThickness = 2, Stroke = Colors.DeepSkyBlue });
            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()  { DataSeries = filteredDataOffset, StrokeThickness = 2, Stroke = Color.FromArgb(0x77, 0xFF, 0x33, 0x33) });
            //sciChart.RenderableSeries.Add(new FastLineRenderableSeries()  { DataSeries = filteredDataCustom.FilteredDataSeries, StrokeThickness = 2, Stroke = Color.FromArgb(0x33, 0xFF, 0x66, 0x00) });
        }

        private void OnUpdatedData(double randomness, double curviness)
        {
            var someDummyData = CreateSomeScatterData(randomness, curviness);
            _originalData.Clear();
            _originalData.Append(someDummyData.XData, someDummyData.YData);
        }

        private DoubleSeries CreateSomeScatterData(double randomness, double curviness)
        {
            var doubleSeries = new DoubleSeries();            
            Random r = new Random();
            const double c = 0;
            const double m = 0.02;

            // Create some data which looks like a scatter chart with points randomly spaced around a 
            // slightly curved / linear trendline dataset 
            // 
            // this will be used to apply filters such as Trendline, Polynomial, Scale, Offset etc... 
            for (int x = 0; x < 200; x++)
            {
                double y = (m * x + c + (r.NextDouble() * randomness)) * x*(1+ curviness);
                doubleSeries.Add(new XYPoint{X = x, Y = y});
            }

            return doubleSeries;
        }

        private void Slider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (randomSlider == null || slopeSlider == null) return;
            OnUpdatedData(randomSlider.Value, slopeSlider.Value);
        }
    }

    public class CustomFilter : FilterBase
    {
        private readonly XyDataSeries<double, double> _originalDataSeries;
        private readonly XyDataSeries<double, double> _filteredDataSeries = new XyDataSeries<double, double>();

        public CustomFilter(XyDataSeries<double,double> originalDataSeries) : base(originalDataSeries)
        {
            _originalDataSeries = originalDataSeries;

            // Store a reference in the base class to the FilteredDataSeries
            FilteredDataSeries = _filteredDataSeries;
        }

        public override void FilterAll()
        {
            // When FilterAll is called, recreate the FilteredDataSeries and apply the filtering. 

            _filteredDataSeries.Append(_originalDataSeries.XValues[0], _originalDataSeries.YValues[0]);

            for (int i = 1; i < _originalDataSeries.Count; i++)
            {
                _filteredDataSeries.Append(_originalDataSeries.XValues[i], _originalDataSeries.YValues[i] + 0.5*_originalDataSeries.YValues[i-1]);
            }
        }
    }
}
