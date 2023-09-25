// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
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
using SciChart.Charting3D.Axis;
using SciChart.Charting3D.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.Charts3D.CreateA3DChart
{
    public partial class LogarithmicAxis3DView : UserControl
    {
        public LogarithmicAxis3DView()
        {
            InitializeComponent();

            Loaded += OnLoaded;
            Unloaded += OnUnloaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            var converter = new LogarithmicBase3DConverter();
            var logBinding = new Binding("SelectedValue") { ElementName = "logBasesChbx", Converter = converter };

            logarithmicNumericXAxis3D.SetBinding(LogarithmicNumericAxis3D.LogarithmicBaseProperty, logBinding);
            logarithmicNumericYAxis3D.SetBinding(LogarithmicNumericAxis3D.LogarithmicBaseProperty, logBinding);

            var xyzDataSeries3D = new XyzDataSeries3D<double>();
            var data = DataManager.Instance.GetExponentialCurve(1.8, 100);

            var count = 100;
            var random = new Random(0);

            for (int i = 0; i < count; i++)
            {
                var x = data[i].X;
                var y = data[i].Y;
                var z = DataManager.Instance.GetGaussianRandomNumber(15, 1.5);

                var color = Color.FromRgb((byte)random.Next(50, 255), (byte)random.Next(50, 255), (byte)random.Next(50, 255));
                var scale = (float)((random.NextDouble() + 0.5) * 3.0);

                xyzDataSeries3D.Append(x, y, z, new PointMetadata3D(color, scale));
            }

            pointLineSeries3D.DataSeries = xyzDataSeries3D;
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            BindingOperations.ClearBinding(logarithmicNumericXAxis3D, LogarithmicNumericAxis3D.LogarithmicBaseProperty);
            BindingOperations.ClearBinding(logarithmicNumericYAxis3D, LogarithmicNumericAxis3D.LogarithmicBaseProperty);
        }
    }

    public class LogarithmicBase3DConverter : IValueConverter
    {
        public double DefaultLogBase { get; set; } = 10d;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var str = value?.ToString();

            if (!string.IsNullOrEmpty(str))
            {
                return str.ToUpperInvariant().Equals("E") ? Math.E : double.Parse(str, CultureInfo.InvariantCulture);
            }
            return DefaultLogBase;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
