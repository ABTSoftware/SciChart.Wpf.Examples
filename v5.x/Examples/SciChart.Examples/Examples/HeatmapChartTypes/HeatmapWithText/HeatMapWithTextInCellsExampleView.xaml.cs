// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2017. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// HeatMapWithTextInCellsExampleView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Axes.LabelProviders;
using SciChart.Core.Helpers;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;
using SciChart.Examples.Examples.AnnotateAChart.CompositeAnnotations.FibonacciAnnotations;

namespace SciChart.Examples.Examples.HeatmapChartTypes.HeatmapWithText
{
    /// <summary>
    /// Interaction logic for HeatMapWithTextInCellsExampleView.xaml
    /// </summary>
    public partial class HeatMapWithTextInCellsExampleView : UserControl
    {
        class YAxisLabelProvider: LabelProviderBase
        {
            public override string FormatLabel(IComparable dataValue)
            {
                var day = Convert.ToInt32(dataValue);
                switch (day)
                {
                    case 0: return "Mon";
                    case 1: return "Tue";
                    case 2: return "Wed";
                    case 3: return "Thu";
                    case 4: return "Fri";
                    case 5: return "Sat";
                    case 6: return "Sun";
                }
                return "";
            }

            public override string FormatCursorLabel(IComparable dataValue)
            {
                return FormatLabel(dataValue);
            }
        }

        class XAxisLabelProvider : LabelProviderBase
        {
            public override string FormatLabel(IComparable dataValue)
            {
                var h = Convert.ToInt32(dataValue);
                var dt = new DateTime(2000, 1, 1, 1, 0, 0).AddHours(h);
                return dt.ToString("hh:mm tt", new CultureInfo("en-US"));
            }

            public override string FormatCursorLabel(IComparable dataValue)
            {
                return FormatLabel(dataValue);
            }
        }

        private void EnableContours(object sender, RoutedEventArgs e)
        {
            contourSeries.IsVisible = true;
        }

        private void DisableContours(object sender, RoutedEventArgs e)
        {
            contourSeries.IsVisible = false;
        }

        private IDataSeries CreateSeries()
        {
            var rnd = new FasterRandom();
            int w = 24, h = 7;
            var data = new double[h, w];
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    data[y, x] = Math.Pow(rnd.NextDouble(), 0.15) * x / (w-1) * y / (h-1) * 100;
                }
            return new UniformHeatmapDataSeries<int, int, double>(data, 0, 1, 0, 1);
        }

        public HeatMapWithTextInCellsExampleView()
        {
            InitializeComponent();
            yAxis.LabelProvider = new YAxisLabelProvider();
            xAxis.LabelProvider = new XAxisLabelProvider();
            heatmapSeries.DataSeries = CreateSeries();

            contourSeries.DataSeries = heatmapSeries.DataSeries;
            
        }

        
        private void OnZMinKeyUp(object sender, KeyEventArgs e)
        {
            OnTxtBoxValueChanged(sender, e, zMin => contourSeries.ZMin = zMin);
        }

        private void OnZMaxKeyUp(object sender, KeyEventArgs e)
        {
            OnTxtBoxValueChanged(sender, e, zMax => contourSeries.ZMax = zMax);
        }

        private void OnZStepKeyUp(object sender, KeyEventArgs e)
        {
            OnTxtBoxValueChanged(sender, e, zStep => contourSeries.ZStep = zStep);
        }

        private void OnTxtBoxValueChanged(object sender, KeyEventArgs e, Action<double> action)
        {
            var txBox = sender as TextBox;

            if (txBox != null && (e.Key == Key.Enter || e.Key == Key.Return))
            {
                var str = txBox.Text.Trim();

                double value;
                var isValid = double.TryParse(str, out value);

                if (isValid)
                {
                    action(value);
                }
            }
        }

        private void OnMinorsPerMajorChanged(object sender, KeyEventArgs e)
        {
            OnTxtBoxValueChanged(sender, e, mPm => contourSeries.MinorsPerMajor = (int)mPm);
        }

        private void OnApplyMajorStyle(object sender, RoutedEventArgs e)
        {
            contourSeries.MajorLineStyle = ApplyMajorStyleCkb.IsChecked.Value ? (Style)Resources["MajorContourLineStyle"] : null;
            sciChart.ZoomExtents();
        }

        private void OnApplyMinorStyle(object sender, RoutedEventArgs e)
        {
            contourSeries.MinorLineStyle = ApplyMinorStyleCkb.IsChecked.Value ? (Style)Resources["MinorContourLineStyle"] : null;
            sciChart.ZoomExtents();
        }

        private void OnApplyPalette(object sender, RoutedEventArgs e)
        {
            contourSeries.ColorMap = ApplyPaletteCkb.IsChecked.Value ? (HeatmapColorPalette)Resources["ColorPalette"] : null;
            sciChart.ZoomExtents();
        }
    }
}
