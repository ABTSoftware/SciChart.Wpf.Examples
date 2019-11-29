// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2020. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LogarithmicAxis3DView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using SciChart.Charting.Visuals.Axes.LogarithmicAxis;
using SciChart.Charting3D.Model;
using SciChart.Examples.ExternalDependencies.Data;
using SciChart.Charting3D.Axis;
   
namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    public partial class LogarithmicAxis3DView : UserControl
    {
        public LogarithmicAxis3DView()
        {
            InitializeComponent();

            this.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var converter = new LogarithmicBase3DConverter();
            var logBinding = new Binding("SelectedValue") { ElementName = "logBasesChbx", Converter = converter };

            logarithmicNumericYAxis3D.SetBinding(LogarithmicNumericAxis3D.LogarithmicBaseProperty, logBinding);
            logarithmicNumericXAxis3D.SetBinding(LogarithmicNumericAxis3D.LogarithmicBaseProperty, logBinding);

            var xyzDataSeries3D = new XyzDataSeries3D<double>();
            var data = DataManager.Instance.GetExponentialCurve(1.8, 100);

            int count = 100;
            var random = new Random(0);

            for (int i = 0; i < count; i++)
            {
                double x = data[i].X;
                double y = data[i].Y;
                double z = DataManager.Instance.GetGaussianRandomNumber(15, 1.5);

                Color? randomColor = Color.FromArgb(0xFF, (byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255));
                float scale = (float)((random.NextDouble() + 0.5) * 3.0);

                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(randomColor, scale));
            }

            pointLineSeries3D.DataSeries = xyzDataSeries3D;
        }
    }

    public class LogarithmicBase3DConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = (string)value;

            var result = str.ToUpperInvariant().Equals("E") ? Math.E : Double.Parse(str, CultureInfo.InvariantCulture);

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
