using System.Collections.ObjectModel;
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
                    StrokeThickness = 2,
                    StyleKey = "FirstRadarSeriesStyle",
                },

                new RadarPolygonRenderableSeriesViewModel()
                {
                    DataSeries = productBDataSeries,
                    SeriesName = "Product B",
                    StrokeThickness = 2,
                    StyleKey = "SecondRadarSeriesStyle",
                },
                new RadarPolygonRenderableSeriesViewModel()
                {
                    DataSeries = productCDataSeries,
                    SeriesName = "Product C",
                    StrokeThickness = 2,
                    StyleKey = "ThirdRadarSeriesStyle",
                },
                new RadarPolygonRenderableSeriesViewModel()
                {
                    DataSeries = productDDataSeries, 
                    SeriesName = "Product D",
                    StrokeThickness = 2,
                    StyleKey = "FourthtRadarSeriesStyle",
                }
            };
        }

        public ObservableCollection<IRadarPolygonRenderableSeriesViewModel> RenderableSeries
        {
            get { return _renderableSeries; }
        }
        
    }
}
