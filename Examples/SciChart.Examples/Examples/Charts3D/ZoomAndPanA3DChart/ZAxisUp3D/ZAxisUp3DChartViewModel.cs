using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using SciChart.Charting3D.Axis;
using SciChart.Charting3D.Model;
using SciChart.Charting3D.Model.ChartSeries;
using SciChart.Charting3D.PointMarkers;
using SciChart.Charting3D.Visuals.RenderableSeries;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Common;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.ZoomAndPanA3DChart.ZAxisUp3D
{
    public class ZAxisUp3DChartViewModel : BaseViewModel
    {
        public AxisBase3DViewModel XAxis { get; set; }

        public AxisBase3DViewModel YAxis { get; set; }

        public AxisBase3DViewModel ZAxis { get; set; }

        public ObservableCollection<IRenderableSeries3DViewModel> RenderableSeries { get; set; }

        public ObservableCollection<ContinentsLegend> ContinentsLegend { get; set; }

        public ZAxisUp3DChartViewModel()
        {
            InitializeAxes();

            var populationData = DataManager.Instance.GetPopulationData();

            var year = populationData.Select(item => item.Year);
            var lifeExpectancy = populationData.Select(item => item.LifeExpectancy);
            var gdpPerCapita = populationData.Select(item => item.GDPPerCapita);
            var continents = populationData.Select(item => item.Continent);

            ContinentsLegend = GetContinentsAndColors();

            var metadata = FormatMetadata(continents, ContinentsLegend);
            var dataSeries3D = new XyzDataSeries3D<double, double, int>();
            dataSeries3D.Append(lifeExpectancy, gdpPerCapita, year, metadata);

            RenderableSeries = new ObservableCollection<IRenderableSeries3DViewModel>
            {
                new ScatterRenderableSeries3DViewModel
                {
                    DataSeries = dataSeries3D,
                    PointMarker = new SpherePointMarker3D { Size = 8, Opacity = 0.9 }
                }
            };
        }

        private void InitializeAxes()
        {
            XAxis = new NumericAxis3DViewModel
            {
                AxisTitle = "Life Expectancy",
                VisibleRange = new DoubleRange(30, 85),
                FontSize = 18,
                TickTextBrush = Brushes.Red
            };

            YAxis = new NumericAxis3DViewModel
            {
                AxisTitle = "GDP per capita",
                VisibleRange = new DoubleRange(0, 100000),
                FontSize = 18,
                TickTextBrush = Brushes.Green,
                PositiveSideClipping = AxisSideClipping.VisibleRange,
                NegativeSideClipping = AxisSideClipping.VisibleRange
            };

            ZAxis = new NumericAxis3DViewModel
            {
                AxisTitle = "Year",
                VisibleRange = new DoubleRange(1950, 2010),
                FontSize = 18,
                TickTextBrush = Brushes.Blue,
                TextFormatting = "F0"
            };
        }

        private ObservableCollection<ContinentsLegend> GetContinentsAndColors()
        {
            var result = new ObservableCollection<ContinentsLegend>
            {
                new ContinentsLegend { Continent = "Asia", Color  = Color.FromRgb(0xcb, 0x11, 0x36) },
                new ContinentsLegend { Continent = "Europe", Color  = Color.FromRgb(0xfb, 0x88, 0x21) },
                new ContinentsLegend { Continent = "Africa", Color  = Color.FromRgb(0xd1, 0x73, 0x65) },
                new ContinentsLegend { Continent = "Americas", Color  = Color.FromRgb(0x5c, 0xa8, 0x9f) },
                new ContinentsLegend { Continent = "Oceania", Color  = Color.FromRgb(0x48, 0xb3, 0xce) }
            };

            return result;
        }

        private IEnumerable<PointMetadata3D> FormatMetadata(IEnumerable<ContinentsEnum> continentsArray, IList<ContinentsLegend> continentsLegend)
        {
            var result = continentsArray.Select(x => new PointMetadata3D
            {
                PointScale = (float)(0.1 + 1),
                VertexColor = continentsLegend[(int)x].Color
            });

            return result;
        }
    }
}
