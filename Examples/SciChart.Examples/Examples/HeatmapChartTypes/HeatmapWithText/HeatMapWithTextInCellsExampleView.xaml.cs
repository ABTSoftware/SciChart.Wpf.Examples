﻿// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2022. All rights reserved.
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
using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.DataSeries.Heatmap2DArrayDataSeries;
using SciChart.Charting.Visuals.Axes.LabelProviders;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.HeatmapChartTypes.HeatmapWithText
{
    /// <summary>
    /// Interaction logic for HeatMapWithTextInCellsExampleView.xaml
    /// </summary>
    public partial class HeatMapWithTextInCellsExampleView : UserControl
    {
        private class YAxisLabelProvider: LabelProviderBase
        {
            public override string FormatLabel(IComparable dataValue)
            {
                try
                {
                    switch (Convert.ToInt32(dataValue))
                    {
                        case 0: return "Mon";
                        case 1: return "Tue";
                        case 2: return "Wed";
                        case 3: return "Thu";
                        case 4: return "Fri";
                        case 5: return "Sat";
                        case 6: return "Sun";

                        default: return string.Empty;
                    }
                }
                catch
                {
                    return string.Empty;
                }
            }

            public override string FormatCursorLabel(IComparable dataValue)
            {
                return FormatLabel(dataValue);
            }
        }

        private class XAxisLabelProvider : LabelProviderBase
        {
            public override string FormatLabel(IComparable dataValue)
            {
                var h = (int) Math.Ceiling(dataValue.ToDouble());
                var dt = new DateTime(2000, 1, 1, 1, 0, 0);
                try
                {
                    dt = dt.AddHours(h);
                }
                catch (ArgumentOutOfRangeException)
                {
                    dt = h < 0 ? DateTime.MinValue : DateTime.MaxValue;
                }
                return dt.ToString("hh:mm tt", new CultureInfo("en-US"));
            }

            public override string FormatCursorLabel(IComparable dataValue)
            {
                return FormatLabel(dataValue);
            }
        }

        private IDataSeries CreateSeries()
        {
            const int w = 24;
            const int h = 7;

            var rnd = new Random(0);
            var data = new double[h, w];

            for (int x = 0; x < w; x++)
            {
                for (int y = 0; y < h; y++)
                {
                    data[y, x] = Math.Pow(rnd.NextDouble(), 0.15) * x / (w - 1) * y / (h - 1) * 100;
                }
            }

            return new UniformHeatmapDataSeries<int, int, double>(data, 0, 1, 0, 1);
        }

        public HeatMapWithTextInCellsExampleView()
        {
            InitializeComponent();

            yAxis.LabelProvider = new YAxisLabelProvider();
            xAxis.LabelProvider = new XAxisLabelProvider();

            heatmapSeries.DataSeries = CreateSeries();      
        }
    }
}