using System.Windows.Controls;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Examples.Examples.CreateTernaryChart
{
    /// <summary>
    /// Interaction logic for PolygonSeriesTernaryChartExampleView.xaml
    /// </summary>
    public partial class PolygonSeriesTernaryChartExampleView : UserControl
    {
        public PolygonSeriesTernaryChartExampleView()
        {
            InitializeComponent();

            cursorModButton.IsChecked = false;
            tooltipModButton.IsChecked = false;

            // Filled areas
            var polygonDataSeries1 = new XyzDataSeries<double> { AcceptsUnsortedData = true, SeriesName = "Clay" };
            var polygonDataSeries2 = new XyzDataSeries<double> { AcceptsUnsortedData = true, SeriesName = "Sandy clay" };
            var polygonDataSeries3 = new XyzDataSeries<double> { AcceptsUnsortedData = true, SeriesName = "Silty clay loam" };
            var polygonDataSeries4 = new XyzDataSeries<double> { AcceptsUnsortedData = true, SeriesName = "Sandy loam" };
            var polygonDataSeries5 = new XyzDataSeries<double> { AcceptsUnsortedData = true, SeriesName = "Loam" };
            var polygonDataSeries6 = new XyzDataSeries<double> { AcceptsUnsortedData = true, SeriesName = "Silt loam" };

            // Сlay series
            polygonDataSeries1.Append(0, 100, 0);
            polygonDataSeries1.Append(40, 60, 0);
            polygonDataSeries1.Append(40, 50, 10);
            polygonDataSeries1.Append(20, 50, 30);
            polygonDataSeries1.Append(0, 70, 30);

            // Sandy clay series
            polygonDataSeries2.Append(0, 70, 30);
            polygonDataSeries2.Append(20, 50, 30);
            polygonDataSeries2.Append(30, 50, 20);
            polygonDataSeries2.Append(30, 30, 40);
            polygonDataSeries2.Append(0, 30, 70);

            // Silty clay loam series
            polygonDataSeries3.Append(30, 50, 20);
            polygonDataSeries3.Append(40, 50, 10);
            polygonDataSeries3.Append(40, 60, 0);
            polygonDataSeries3.Append(70, 30, 0);
            polygonDataSeries3.Append(30, 30, 40);

            // Sandy loam series
            polygonDataSeries4.Append(30, 30, 40);
            polygonDataSeries4.Append(30, 0, 70);
            polygonDataSeries4.Append(0, 0, 100);
            polygonDataSeries4.Append(0, 30, 70);

            // Loam series
            polygonDataSeries5.Append(30, 30, 40);
            polygonDataSeries5.Append(50, 30, 20);
            polygonDataSeries5.Append(80, 0, 20);
            polygonDataSeries5.Append(30, 0, 70);

            // Silt loam series
            polygonDataSeries6.Append(50, 30, 20);
            polygonDataSeries6.Append(70, 30, 0);
            polygonDataSeries6.Append(100, 0, 0);
            polygonDataSeries6.Append(80, 0, 20);

            polygonSeries.DataSeries =  polygonDataSeries1;
            polygonSeries1.DataSeries = polygonDataSeries2;
            polygonSeries2.DataSeries = polygonDataSeries3;
            polygonSeries3.DataSeries = polygonDataSeries4;
            polygonSeries4.DataSeries = polygonDataSeries5;
            polygonSeries5.DataSeries = polygonDataSeries6;
        }
    }
}
