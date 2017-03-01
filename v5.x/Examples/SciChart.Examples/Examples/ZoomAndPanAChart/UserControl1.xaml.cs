using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Abt.Controls.SciChart.Example.Data;

namespace Abt.Controls.SciChart.Example.Examples.IWantTo.ZoomAndPanAChart
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Create a dataset of type X=double, Y=double
            var dataSeriesSet = new DataSeriesSet<double, double>();
            sciChart1.DataSet = dataSeriesSet;
            sciChart2.DataSet = dataSeriesSet;
            // Add a data series
            var series = dataSeriesSet.AddSeries();

            var data = DataManager.Instance.GetFourierSeries(1.0, 0.1);

            // Append data to series. SciChart automatically redraws
            series.Append(data.XData, data.YData);


            // Create a dataset of type X=double, Y=double
            var dataSeriesSet1 = new DataSeriesSet<double, double>();
            sciChart3.DataSet = dataSeriesSet1;

            // Add a data series
            var dataSeries0 = dataSeriesSet1.AddSeries();
            dataSeries0.SeriesName = "Curve A";
            var dataSeries1 = dataSeriesSet1.AddSeries();
            dataSeries1.SeriesName = "Curve B";
            var dataSeries2 = dataSeriesSet1.AddSeries();
            dataSeries2.SeriesName = "Curve C";

            var data1 = DataManager.Instance.GetExponentialCurve(0.4, 100);
            var data2 = DataManager.Instance.GetExponentialCurve(0.44, 100);
            var data3 = DataManager.Instance.GetExponentialCurve(0.49, 100);

            // Append data to series.
            dataSeries0.Append(data1.XData, data1.YData);
            dataSeries1.Append(data2.XData, data2.YData);
            dataSeries2.Append(data3.XData, data3.YData);

            // Zoom to extents of the data
            sciChart3.ZoomExtents();
        }
    }
}
