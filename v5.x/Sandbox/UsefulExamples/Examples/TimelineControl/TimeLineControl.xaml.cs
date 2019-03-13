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
using SciChart.Charting3D.Extensions;

namespace SciChart.Sandbox.Examples.TimelineControl
{
    [TestCase("Timeline Control example using Custom Series")]
    public partial class TimeLineControl : Window
    {
        public TimeLineControl()
        {
            InitializeComponent();

            var xyData = new XyzDataSeries<double, double, int>();

            // Parameters. We use X= startX, Y= length, Z= Color of fill 
            xyData.Append(0, 5, (int)Colors.Red.ToArgb());
            xyData.Append(5, 10, (int)Colors.Green.ToArgb());
            xyData.Append(15, 2, (int)Colors.Blue.ToArgb());

            TimelineSeries.DataSeries = xyData;
        }
    }
}
