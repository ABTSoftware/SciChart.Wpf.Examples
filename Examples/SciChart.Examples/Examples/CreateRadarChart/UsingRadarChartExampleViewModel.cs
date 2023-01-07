﻿using System.Collections.ObjectModel;
using System.Windows.Ink;
using System.Windows.Media;
using SciChart.Charting.Model.ChartSeries;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.PointMarkers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Examples.ExternalDependencies.Common;

namespace SciChart.Examples.Examples.CreateRadarChart
{
    public class UsingRadarChartExampleViewModel : BaseViewModel
    {
        private readonly ObservableCollection<IRadarPolygonRenderableSeriesViewModel> _renderableSeries;

        public UsingRadarChartExampleViewModel()
        {
            var productADataSeries = new ObservableCollection<IRadarPointViewModel>
            {
                new RadarPointViewModel {DataValue = 75, AxisId = "salesId"},
                new RadarPointViewModel {DataValue = 95, AxisId = "marketingId"},
                new RadarPointViewModel {DataValue = 33, AxisId = "developmentId"},
                new RadarPointViewModel {DataValue = 40, AxisId = "customerSupportId"},
                new RadarPointViewModel {DataValue = 15, AxisId = "informationTechnologyId"},
                new RadarPointViewModel {DataValue = 20, AxisId = "administrationId"},
            };

            var productBDataSeries = new ObservableCollection<IRadarPointViewModel>
            {
                new RadarPointViewModel {DataValue = 15, AxisId = "salesId"},
                new RadarPointViewModel {DataValue = 66, AxisId = "marketingId"},
                new RadarPointViewModel {DataValue = 40, AxisId = "developmentId"},
                new RadarPointViewModel {DataValue = 90, AxisId = "customerSupportId"},
                new RadarPointViewModel {DataValue = 45, AxisId = "informationTechnologyId"},
                new RadarPointViewModel {DataValue = 10, AxisId = "administrationId"},
            };

            var productCDataSeries = new ObservableCollection<IRadarPointViewModel>
            {
                new RadarPointViewModel {DataValue = 30, AxisId = "salesId"},
                new RadarPointViewModel {DataValue = 42, AxisId = "marketingId"},
                new RadarPointViewModel {DataValue = 15, AxisId = "developmentId"},
                new RadarPointViewModel {DataValue = 30, AxisId = "customerSupportId"},
                new RadarPointViewModel {DataValue = 90, AxisId = "informationTechnologyId"},
                new RadarPointViewModel {DataValue = 80, AxisId = "administrationId"},
            };

            var productDDataSeries = new ObservableCollection<IRadarPointViewModel>
            {
                new RadarPointViewModel {DataValue = 20, AxisId = "salesId"},
                new RadarPointViewModel {DataValue = 22, AxisId = "marketingId"},
                new RadarPointViewModel {DataValue = 84, AxisId = "developmentId"},
                new RadarPointViewModel {DataValue = 15, AxisId = "customerSupportId"},
                new RadarPointViewModel {DataValue = 40, AxisId = "informationTechnologyId"},
                new RadarPointViewModel {DataValue = 5, AxisId = "administrationId"},
            };

            _renderableSeries = new ObservableCollection<IRadarPolygonRenderableSeriesViewModel>()
            {
                new RadarPolygonRenderableSeriesViewModel()
                {
                    DataSeries = productADataSeries,
                    SeriesName = "Product A",
                    Stroke = Colors.Blue,
                    StrokeThickness = 2,
                    StyleKey = "BlueRadarSeriesStyle",
                    Fill = new SolidColorBrush {Color = Colors.Blue, Opacity = 0.4},
                    PointMarker = new TrianglePointMarker { Width = 10, Height = 10, Fill = Colors.Blue, StrokeThickness = 0}
                },

                new RadarPolygonRenderableSeriesViewModel()
                {
                    DataSeries = productBDataSeries,
                    SeriesName = "Product B",
                    Stroke = Colors.Coral,
                    StrokeThickness = 2,
                    StyleKey = "CoralRadarSeriesStyle",
                    Fill = new SolidColorBrush {Color = Colors.Coral, Opacity = 0.4},
                    PointMarker = new EllipsePointMarker { Width = 10, Height = 10, Fill = Colors.Coral, StrokeThickness = 0}
                },
                new RadarPolygonRenderableSeriesViewModel()
                {
                    DataSeries = productCDataSeries,
                    SeriesName = "Product C",
                    Stroke = Colors.Green,
                    StrokeThickness = 2,
                    StyleKey = "GreenRadarSeriesStyle",
                    Fill = new SolidColorBrush {Color = Colors.Green, Opacity = 0.4},
                    PointMarker = new CrossPointMarker { Width = 10, Height = 10, Stroke = Colors.Green, StrokeThickness = 2}
                },
                new RadarPolygonRenderableSeriesViewModel()
                {
                    DataSeries = productDDataSeries, 
                    SeriesName = "Product D",
                    Stroke = Colors.YellowGreen,
                    StrokeThickness = 2,
                    StyleKey = "YellowGreenRadarSeriesStyle",
                    Fill = new SolidColorBrush {Color = Colors.YellowGreen, Opacity = 0.4},
                    PointMarker = new SquarePointMarker { Width = 10, Height = 10, Fill = Colors.YellowGreen, StrokeThickness = 0}
                }
            };
        }

        public ObservableCollection<IRadarPolygonRenderableSeriesViewModel> RenderableSeries
        {
            get { return _renderableSeries; }
        }
        
    }
}
