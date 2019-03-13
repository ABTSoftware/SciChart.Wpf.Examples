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

namespace SciChart.Sandbox.Examples.TimelineControl
{
    [TestCase("Timeline Control example using Custom Series")]
    public partial class TimeLineControl : Window
    {
        public TimeLineControl()
        {
            InitializeComponent();

            var xyData = new XyDataSeries<double>();
            xyData.Append(0, 0, new TimelinePointMetadata() { Fill = Colors.Red });
            xyData.Append(5, 0, new TimelinePointMetadata() { Fill = Colors.Green});
            xyData.Append(15, 0, new TimelinePointMetadata() { Fill = Colors.Blue});

            TimelineSeries.DataSeries = xyData;
        }
    }
}
