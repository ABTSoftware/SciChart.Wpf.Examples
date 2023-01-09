using System;
using System.Windows;
using SciChart.Charting.Model.DataSeries;

namespace EventOnZoomExtentsCompleted
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var data = new XyDataSeries<double>();

            data.Append(0, 0);
            data.Append(1, 1);
            data.Append(2, 2);

            lineSeries.DataSeries = data;
        }

        private void OnZoomExtentsCompleted(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Zoom extents completed!");
        }
    }
}
