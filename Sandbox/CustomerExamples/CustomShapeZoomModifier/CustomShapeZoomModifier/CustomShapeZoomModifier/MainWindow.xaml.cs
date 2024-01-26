using SciChart.Charting.Model.DataSeries;
using System.Windows;

namespace CustomShapeZoomModifier
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var xValues = new [] { 1d, 1.5, 2d, 4d, 6.5d, 10d };
            var yValues = new [] { -2d, -2d, 2d, 2d, 1.5d, 4d };

            var dataSeries = new XyDataSeries<double>();
            lineRenderSeries.DataSeries = dataSeries;

            // Append data to series. SciChart automatically redraws
            dataSeries.Append(xValues, yValues);
            
            sciChart.ZoomExtents();
        }
    }
}
