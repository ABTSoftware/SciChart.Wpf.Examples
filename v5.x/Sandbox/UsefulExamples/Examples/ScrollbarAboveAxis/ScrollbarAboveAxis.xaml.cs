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
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Sandbox.Examples.ScrollbarAboveAxis
{
    /// <summary>
    /// NOTE: In order for this sample to work you need to set a valid developer license. This will hide the POWERED BY SCICHART logo which makes
    /// the lower chart have a minimum size. 
    /// </summary>
    [TestCase("Scrollbars above axis")]
    public partial class ScrollbarAboveAxis : Window
    {
        public ScrollbarAboveAxis()
        {
            InitializeComponent();

            primaryChartSurface.RenderableSeries.Add(new FastLineRenderableSeries()
            {
                DataSeries=GetData(),
            });
        }

        private IDataSeries GetData()
        {
            var data = new XyDataSeries<double>();
            data.Append(0,0);
            data.Append(1, 1);
            data.Append(2, 2);
        return data;
        }
    }
}
