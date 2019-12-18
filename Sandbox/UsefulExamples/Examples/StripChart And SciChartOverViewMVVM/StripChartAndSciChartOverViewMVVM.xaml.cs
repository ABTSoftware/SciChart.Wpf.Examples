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

namespace SciChart.Wpf.TestSuite.ExampleSandbox.StripChart_And_SciChartOverViewMVVM
{
    /// <summary>
    /// Interaction logic for StripChartAndSciChartOverViewMVVM.xaml
    /// </summary>
    [TestCase("StripChart And SciChartOverViewMVVM")]
    public partial class StripChartAndSciChartOverViewMVVM : Window
    {
        public StripChartAndSciChartOverViewMVVM()
        {
            InitializeComponent();
            this.DataContext = new StripChartViewModel();
        }

        private void OnOverviewSurfaceLoaded(object sender, RoutedEventArgs e)
        {
            OverviewSurface.ZoomExtents();
        }
    }
}
