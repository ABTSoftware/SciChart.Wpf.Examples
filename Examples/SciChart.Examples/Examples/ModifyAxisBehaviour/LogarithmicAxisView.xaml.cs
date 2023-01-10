// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2023. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// LogarithmicAxisView.xaml.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

using SciChart.Charting;
using SciChart.Charting.Model.DataSeries;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Axes.LogarithmicAxis;
using SciChart.Data.Model;
using SciChart.Examples.ExternalDependencies.Data;

namespace SciChart.Examples.Examples.ModifyAxisBehaviour
{
    public partial class LogarithmicAxisView : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private static readonly TimeSpan duration = TimeSpan.FromMilliseconds(500);

        public LogarithmicAxisView()
        {
            InitializeComponent();
        }

        private void OnLogarithmicAxisViewLoaded(object sender, RoutedEventArgs e)
        {
            // Create some DataSeries of type X=double, Y=double
            var posDataSeries0 = new XyDataSeries<double, double>();
            var posDataSeries1 = new XyDataSeries<double, double>();
            var posDataSeries2 = new XyDataSeries<double, double>();

            var negDataSeries0 = new XyDataSeries<double, double>();
            var negDataSeries1 = new XyDataSeries<double, double>();
            var negDataSeries2 = new XyDataSeries<double, double>();

            var data1 = DataManager.Instance.GetExponentialCurve(1.8, 100);
            var data2 = DataManager.Instance.GetExponentialCurve(2.25, 100);
            var data3 = DataManager.Instance.GetExponentialCurve(3.59, 100);

            // Append data to series
            posDataSeries0.Append(data1.XData, data1.YData);
            posDataSeries1.Append(data2.XData, data2.YData);
            posDataSeries2.Append(data3.XData, data3.YData);

            negDataSeries0.Append(data1.XData, data1.YData.Select(y => -y));
            negDataSeries1.Append(data2.XData, data2.YData.Select(y => -y));
            negDataSeries2.Append(data3.XData, data3.YData.Select(y => -y));

            // Attach DataSeries to RendetableSeries
            positiveScaleChart.RenderableSeries[0].DataSeries = posDataSeries0;
            positiveScaleChart.RenderableSeries[1].DataSeries = posDataSeries1;
            positiveScaleChart.RenderableSeries[2].DataSeries = posDataSeries2;

            negativeScaleChart.RenderableSeries[0].DataSeries = negDataSeries0;
            negativeScaleChart.RenderableSeries[1].DataSeries = negDataSeries1;
            negativeScaleChart.RenderableSeries[2].DataSeries = negDataSeries2;

            // Set X,Y axes
            OnAxisTypeChanged(null, null);

            // Zoom both charts to the data extents
            positiveScaleChart.AnimateZoomExtents(duration);
            negativeScaleChart.AnimateZoomExtents(duration);
        }

        private void OnAxisTypeChanged(object sender, RoutedEventArgs e)
        {
            var isYLog = isYLogChbx != null && isYLogChbx.IsChecked.HasValue && isYLogChbx.IsChecked.Value;
            var isXLog = isXLogChbx != null && isXLogChbx.IsChecked.HasValue && isXLogChbx.IsChecked.Value;

            var positiveChartXAxis = isXLog ? GenerateLogarithmicAxis(AxisAlignment.Bottom, false) : GenerateLinearAxis(AxisAlignment.Bottom);
            var positiveChartYAxis = isYLog ? GenerateLogarithmicAxis(AxisAlignment.Left, false) : GenerateLinearAxis(AxisAlignment.Left);
            if (positiveScaleChart != null)
            {
                // Set X,Y axes
                positiveScaleChart.YAxis = positiveChartYAxis;
                positiveScaleChart.XAxis = positiveChartXAxis;

                positiveScaleChart.ZoomExtents();
            }

            if (negativeScaleChart != null)
            {
                // Set X,Y axes
                negativeScaleChart.YAxis = isYLog ? GenerateLogarithmicAxis(AxisAlignment.Left, true) : GenerateLinearAxis(AxisAlignment.Left);
                negativeScaleChart.XAxis = isXLog ? GenerateLogarithmicAxis(AxisAlignment.Bottom, false) : GenerateLinearAxis(AxisAlignment.Bottom);

                // Hide the bottom XAxis
                negativeScaleChart.XAxis.Visibility = Visibility.Collapsed;

                // Bind the bottom chart XAxis to the top chart XAxis
                var xVisibleRangeBinding = new Binding("VisibleRange")
                {
                    Source = positiveChartXAxis,
                    Mode = BindingMode.TwoWay
                };
                ((AxisBase)negativeScaleChart.XAxis).SetBinding(AxisBase.VisibleRangeProperty, xVisibleRangeBinding);

                // Bind the bottom chart YAxis to the top chart YAxis
                var yVisibleRangeBinding = new Binding("VisibleRange")
                {
                    Source = positiveChartYAxis,
                    Mode = BindingMode.TwoWay,
                    Converter = new ReverseYRangeConverter()
                };
                ((AxisBase)negativeScaleChart.YAxis).SetBinding(AxisBase.VisibleRangeProperty, yVisibleRangeBinding);

                negativeScaleChart.ZoomExtents();
            }
        }

        private IAxis GenerateLinearAxis(AxisAlignment axisAlignment)
        {
            return new NumericAxis
            {
                AxisAlignment = axisAlignment,

                TextFormatting = "#.#E+0",
                ScientificNotation = ScientificNotation.Normalized,

                GrowBy = new DoubleRange(0.1, 0.1),

                DrawMajorBands = false
            };
        }

        private IAxis GenerateLogarithmicAxis(AxisAlignment axisAlignment, bool withNegativeScale)
        {
            var axis = withNegativeScale ? new NegativeLogarithmicAxis() : new LogarithmicNumericAxis();
            axis.AxisAlignment = axisAlignment;

            axis.TextFormatting = "#.#E+0";
            axis.ScientificNotation = ScientificNotation.LogarithmicBase;

            axis.GrowBy = new DoubleRange(0.1, 0.1);

            axis.DrawMajorBands = false;

            var converter = new LogarithmicBaseConverter();
            var logBinding = new Binding("SelectedValue") { ElementName = "logBasesChbx", Converter = converter };

            axis.SetBinding(LogarithmicNumericAxis.LogarithmicBaseProperty, logBinding);

            return axis;
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class LogarithmicBaseConverter : IValueConverter
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

    public class ReverseYRangeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DoubleRange negativeRange = null;
            if (value != null)
            {
                var positiveRange = (DoubleRange) value;
                negativeRange = new DoubleRange(-positiveRange.Max, -positiveRange.Min);
            }

            return negativeRange;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DoubleRange positiveRange = null;
            if (value != null)
            {
                var negativeRange = (DoubleRange) value;
                positiveRange = new DoubleRange(-negativeRange.Min, -negativeRange.Max);
            }

            return positiveRange;
        }
    }
}
