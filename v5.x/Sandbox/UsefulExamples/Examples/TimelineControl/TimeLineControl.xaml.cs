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
        Random _random = new Random();

        public TimeLineControl()
        {
            InitializeComponent();            

            var xyData0 = new XyzDataSeries<double, double, int>();
            var xyData1 = new XyzDataSeries<double, double, int>();

            double iLast = 0;
            for (int i = 0; i < 100; i++)
            {
                // Parameters. We use X= startX, Y= length, Z= Color of fill    
                double length = _random.Next(1, 10);
                xyData0.Append(iLast, length, GetRandomColor());                
                xyData1.Append(iLast, length, GetRandomColor());
                iLast += length;
            }            
            
            TimelineSeries0.DataSeries = xyData0;
            TimelineSeries1.DataSeries = xyData0;
        }

        private int GetRandomColor()
        {
            return (int)Color.FromArgb(0xFF, (byte)_random.Next(50, 255), (byte)_random.Next(50, 255), (byte)_random.Next(50, 255)).ToArgb();
        }
    }
}
