using System;
using System.Windows;
using System.Windows.Media;
using SciChart.Charting.Model.DataSeries;

namespace TimeLineControlExample
{
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
