// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// PolarChartViewModelFactory.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Model.ChartSeries;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.DashboardStylePolarCharts
{
    public static class PolarChartViewModelFactory
    {
        private const int PointAmount = 10;

        public static PolarChartViewModel New<T>() where T : IRenderableSeriesViewModel
        {
            var type = typeof (T);
            var data = Enumerable.Range(0, PointAmount).Select(x => (double)x).ToList();

            if (type == typeof (LineRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new LineRenderableSeriesViewModel {DataSeries = GetXyDataSeries(data)});
            }
            if (type == typeof (XyScatterRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new XyScatterRenderableSeriesViewModel
                {
                    DataSeries = GetXyDataSeries(data),
                    PointMarker = new EllipsePointMarker
                    {
                        Width = 10,
                        Height = 10,
                        Fill = Color.FromArgb(255, 71, 187, 255),
                        Stroke = Colors.Black
                    }
                });
            }
            if (type == typeof (MountainRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new MountainRenderableSeriesViewModel {DataSeries = GetXyDataSeries(data)});
            }
            if (type == typeof (ColumnRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new ColumnRenderableSeriesViewModel {DataSeries = GetXyDataSeries(data)});
            }
            if (type == typeof (ImpulseRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new ImpulseRenderableSeriesViewModel {DataSeries = GetHlcDataSeries(data)});
            }
            if (type == typeof (CandlestickRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new CandlestickRenderableSeriesViewModel {DataSeries = GetOhlcDataSeries(data)});
            }
            if (type == typeof (OhlcRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new OhlcRenderableSeriesViewModel {DataSeries = GetOhlcDataSeries(data)});
            }
            if (type == typeof (BoxPlotRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new BoxPlotRenderableSeriesViewModel {DataSeries = GetBoxSeries(data)});
            }
            if (type == typeof (ErrorBarsRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new ErrorBarsRenderableSeriesViewModel {DataSeries = GetHlcDataSeries(data)});
            }
            if (type == typeof (BubbleRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new BubbleRenderableSeriesViewModel
                {
                    DataSeries = GetXyzDataSeries(data),
                    BubbleColor = Color.FromArgb(255, 110, 0, 255),
                    AutoZRange = false,
                });
            }
            if (type == typeof (BandRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(new BandRenderableSeriesViewModel {DataSeries = GetXyyDataSeries(data)});
            }
            if (type == typeof (StackedColumnRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(
                    new StackedColumnRenderableSeriesViewModel
                    {
                        DataSeries = GetXyDataSeries(data),
                        Fill = new SolidColorBrush(Color.FromArgb(255, 0, 2, 195)),
                        StackedGroupId = "stackedColumns"
                    },
                    new StackedColumnRenderableSeriesViewModel
                    {
                        DataSeries = GetXyDataSeries(data),
                        Fill = new SolidColorBrush(Color.FromArgb(255, 0, 143, 255)),
                        StackedGroupId = "stackedColumns"
                    },
                    new StackedColumnRenderableSeriesViewModel
                    {
                        DataSeries = GetXyDataSeries(data),
                        Fill = new SolidColorBrush(Color.FromArgb(255, 0, 255, 84)),
                        StackedGroupId = "stackedColumns"
                    });
            }
            if (type == typeof (StackedMountainRenderableSeriesViewModel))
            {
                return new PolarChartViewModel(
                    new StackedMountainRenderableSeriesViewModel
                    {
                        DataSeries = GetXyDataSeries(data),
                        Fill = new SolidColorBrush(Color.FromArgb(255, 57, 255, 0)),
                        StackedGroupId = "stackedMountains",
                    },
                    new StackedMountainRenderableSeriesViewModel
                    {
                        DataSeries = GetXyDataSeries(data),
                        Fill = new SolidColorBrush(Color.FromArgb(255, 251, 255, 0)),
                        StackedGroupId = "stackedMountains",
                    },
                    new StackedMountainRenderableSeriesViewModel
                    {
                        DataSeries = GetXyDataSeries(data),
                        Fill = new SolidColorBrush(Color.FromArgb(255, 0, 90, 255)),
                        StackedGroupId = "stackedMountains",
                    });
            }

            throw new NotImplementedException("Unsupported Series Type");
        }

// ReSharper disable PossibleMultipleEnumeration
        private static IXyDataSeries<double, double> GetXyDataSeries(IEnumerable<double> data)
        {
            var dataSeries = new XyDataSeries<double, double>();
            dataSeries.Append(data, data);

            return dataSeries;
        }

        private static IOhlcDataSeries<double, double> GetOhlcDataSeries(IEnumerable<double> data)
        {
            var dataSeries = new OhlcDataSeries<double, double>();

            dataSeries.Append(data,
                data.Select(x => x + 2.4),
                data.Select(x => x + 4.9),
                data.Select(x => x - 5.3),
                data.Select(x => x - 2.7));

            return dataSeries;
        }

        private static BoxPlotDataSeries<double, double> GetBoxSeries(IEnumerable<double> data)
        {
            var dataSeries = new BoxPlotDataSeries<double, double>();

            dataSeries.Append(data, data, 
                data.Select(x => x - 2.0),
                data.Select(x => x - 1.0),
                data.Select(x => x + 0.5),
                data.Select(x => x + 1.5));

            return dataSeries;
        }

        private static IHlcDataSeries<double, double> GetHlcDataSeries(IEnumerable<double> data)
        {
            var dataSeries = new HlcDataSeries<double, double>();
            dataSeries.Append(data, data, data.Select(x => x + 1.0), data.Select(x => x - 0.3));

            return dataSeries;
        }

        private static IXyyDataSeries<double, double> GetXyyDataSeries(IEnumerable<double> data)
        {
            var dataSeries = new XyyDataSeries<double, double>();

            dataSeries.Append(data, 
                data.Select(x => x%2 == 0 ? 2*x : x), 
                data.Select(x => x%2 == 1 ? 2*x : x));

            return dataSeries;
        }

        private static IXyzDataSeries<double, double, double> GetXyzDataSeries(IEnumerable<double> data)
        {
            var dataSeries = new XyzDataSeries<double, double, double>();

            dataSeries.Append(data, data, Enumerable.Repeat(30d, data.Count()));

            return dataSeries;
        }
// ReSharper restore PossibleMultipleEnumeration
    }
}