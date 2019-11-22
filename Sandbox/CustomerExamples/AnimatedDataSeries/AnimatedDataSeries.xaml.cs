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
using System.Windows.Shapes;
using SciChart.Charting.Model.DataSeries;

namespace SciChart.Sandbox.Examples.AnimatedDataSeries
{
    /// <summary>
    /// Animates a data-series using the Filters API 
    /// </summary>
    [TestCase("Animated DataSeries")]
    public partial class AnimatedDataSeries : Window
    {
        public AnimatedDataSeries()
        {
            InitializeComponent();

            var originalData = new XyDataSeries<double, double>();
            int count = 100;
            for (int i = 0; i < count; i++)
            {
                originalData.Append(i, Math.Sin(i*0.2));
            }

            lineSeries.DataSeries = new AnimatedDataSeriesFilter(originalData).FilteredDataSeries;
        }
    }
}
