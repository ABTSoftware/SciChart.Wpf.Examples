// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2024. All rights reserved.
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
using SciChart.Charting.Visuals.RenderableSeries.Animations;

namespace SciChart.Examples.Examples.FiltersAPI
{
    /// <summary>
    /// Interaction logic for FiltersAPIExample.xaml
    /// </summary>
    public partial class FiltersAPIExample : UserControl
    {
        private readonly XyDataSeries<TimeSpan, double> _originalData = new XyDataSeries<TimeSpan, double>();

        public FiltersAPIExample()
        {
            InitializeComponent();

            OnUpdatedData(1.0d, 0.01d);

            var filteredDataLinearTrendline = _originalData.ToLinearTrendline();
            var filteredDataPolynomialTrendline = _originalData.ToPolynomialTrend(3);
            var filteredDataScale = _originalData.Scale(0.5);
            var filteredDataOffset = _originalData.Offset(-50.0);
            var filteredDataSpline = _originalData.ToSpline(5);
            var filteredDataCustom = new CustomFilter(_originalData);

            var sweepAnimation1 = new SweepAnimation() { AnimationDelay = TimeSpan.FromSeconds(0.5), Duration = TimeSpan.FromSeconds(1) };
            var sweepAnimation2 = new SweepAnimation() { AnimationDelay = TimeSpan.FromSeconds(0.5), Duration = TimeSpan.FromSeconds(1) };
            var sweepAnimation3 = new SweepAnimation() { AnimationDelay = TimeSpan.FromSeconds(0.5), Duration = TimeSpan.FromSeconds(1) };
            var sweepAnimation4 = new SweepAnimation() { AnimationDelay = TimeSpan.FromSeconds(0.5), Duration = TimeSpan.FromSeconds(1) };
            var sweepAnimation5 = new SweepAnimation() { AnimationDelay = TimeSpan.FromSeconds(0.5), Duration = TimeSpan.FromSeconds(1) };
            var sweepAnimation6 = new SweepAnimation() { AnimationDelay = TimeSpan.FromSeconds(0.5), Duration = TimeSpan.FromSeconds(1) };
            var sweepAnimation7 = new SweepAnimation() { AnimationDelay = TimeSpan.FromSeconds(0.5), Duration = TimeSpan.FromSeconds(1) };


            _originalData.SeriesName = "Original Data";
            filteredDataLinearTrendline.SeriesName = "Linear Trendline";
            filteredDataPolynomialTrendline.SeriesName = "Polynomial (3rd Order)";
            filteredDataScale.SeriesName = "Scaled * 0.5";
            filteredDataOffset.SeriesName = "Offset -50";
            filteredDataSpline.SeriesName = "Spline, Tension=5";
            filteredDataCustom.FilteredDataSeries.SeriesName = "Custom Filter";

            sciChart.RenderableSeries.Add(new XyScatterRenderableSeries()
            {
                DataSeries = _originalData,
                Stroke = Color.FromArgb(0xFF, 0xE9, 0x70, 0x64),
                PointMarker = new EllipsePointMarker()
                {
                    Fill = Color.FromArgb(0xFF, 0xE9, 0x70, 0x64),
                    Stroke = Color.FromArgb(0xFF, 0xE9, 0x70, 0x64)
                },
                SeriesAnimation = sweepAnimation1
            });

            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()
            {
                DataSeries = filteredDataLinearTrendline,
                StrokeThickness = 2,
                Stroke = Color.FromArgb(0xFF, 0xAE, 0x41, 0x8D),
                SeriesAnimation = sweepAnimation2
            });

            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()
            {
                DataSeries = filteredDataPolynomialTrendline,
                StrokeThickness = 2,
                Stroke = Color.FromArgb(0xFF, 0x68, 0xBC, 0xAE),
                SeriesAnimation = sweepAnimation3
            });

            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()
            {
                DataSeries = filteredDataScale,
                StrokeThickness = 2,
                Stroke = Color.FromArgb(0xFF, 0x47, 0xBD, 0xE6),
                SeriesAnimation = sweepAnimation4
            });

            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()
            {
                DataSeries = filteredDataSpline,
                StrokeThickness = 1,
                Stroke = Color.FromArgb(0xFF, 0x63, 0x4E, 0x96),
                Opacity = 0.5,
                SeriesAnimation = sweepAnimation5
            });

            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()
            {
                DataSeries = filteredDataOffset,
                StrokeThickness = 2,
                Stroke = Color.FromArgb(0xFF, 0x0B, 0xDE, 0xF4),
                SeriesAnimation = sweepAnimation6
            });

            sciChart.RenderableSeries.Add(new FastLineRenderableSeries()
            {
                DataSeries = filteredDataCustom.FilteredDataSeries,
                StrokeThickness = 2,
                Stroke = Color.FromArgb(0xFF, 0xF6, 0x08, 0x6C),
                SeriesAnimation = sweepAnimation7
            });

            sweepAnimation1.StartAnimation();
            sweepAnimation2.StartAnimation();
            sweepAnimation3.StartAnimation();
            sweepAnimation4.StartAnimation();
            sweepAnimation5.StartAnimation();
            sweepAnimation6.StartAnimation();
            sweepAnimation7.StartAnimation();
        }

        private void FiltersAPIExample_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void OnUpdatedData(double randomness, double curviness)
        {
            var someDummyData = CreateSomeScatterData(randomness, curviness);
            _originalData.Clear();
            _originalData.Append(someDummyData.XData.Select(x => TimeSpan.FromSeconds(x)), someDummyData.YData);
        }

        private DoubleSeries CreateSomeScatterData(double randomness, double curviness)
        {
            var doubleSeries = new DoubleSeries();
            Random r = new Random(0);
            const double c = 0;
            const double m = 0.02;

            // Create some data which looks like a scatter chart with points randomly spaced around a 
            // slightly curved / linear trendline dataset 
            // 
            // this will be used to apply filters such as Trendline, Polynomial, Scale, Offset etc... 
            for (int x = 0; x < 200; x++)
            {
                double y = (m * x + c + (r.NextDouble() * randomness)) * x * (1 + curviness);
                doubleSeries.Add(new XYPoint { X = x, Y = y });
            }

            return doubleSeries;
        }

        private void Slider_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (randomSlider == null || slopeSlider == null) return;
            OnUpdatedData(randomSlider.Value, slopeSlider.Value);
        }
    }
}
