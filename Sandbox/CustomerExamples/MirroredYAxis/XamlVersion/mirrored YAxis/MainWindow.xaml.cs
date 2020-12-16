using System;
using System.Collections.Generic;
using System.Globalization;
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
using SciChart.Charting.Model.DataSeries;

namespace WpfApplication14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var data = new XyDataSeries<double, double>();
            data.Append(new double[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 }, new double[] { 0, 1, 0, -1, 0, 1, 0, -1, 0, 1, 0, -1, 0 });

            lineSeries.DataSeries = data;
        }
    }
}
