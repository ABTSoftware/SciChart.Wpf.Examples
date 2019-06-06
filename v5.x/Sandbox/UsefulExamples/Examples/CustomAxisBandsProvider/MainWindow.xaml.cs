using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Model.Filters;
using SciChart.Sandbox;

namespace CustomAxisBandsProvider
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [TestCase("Custom AxisBandsProvider")]
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_OnLoaded(object o, RoutedEventArgs e)
        {
            DataContext = new MainViewModel();
        }

        private void OnLoadDataClick(object sender, RoutedEventArgs e)
        {
            var dataSeries = new XyDataSeries<DateTime, double>();

            var startDate = new DateTime(2017, 9, 1, 12, 0, 0);

            for (int i = 0; i < 30; ++i)
            {
                var date = startDate.AddDays(i);


                dataSeries.Append(date, i * 10);
            }

            LineRenderableSeries.DataSeries = dataSeries.ToDiscontinuousSeries(new WeekDaysAxisCalendar());
            LineRenderableSeries2.DataSeries = LineRenderableSeries.DataSeries;

            LineRenderableSeries.DataSeries.InvalidateParentSurface(RangeMode.ZoomToFit);
        }
    }
}
