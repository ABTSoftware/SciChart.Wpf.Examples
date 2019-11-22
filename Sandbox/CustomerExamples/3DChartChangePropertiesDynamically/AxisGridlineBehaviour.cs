using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using SciChart.Charting.Visuals.Axes;

namespace ChangePropertiesDynamically
{
    public class AxisGridLineStyleBehavior
    {
        public static readonly DependencyProperty GridStrokeProperty = DependencyProperty.RegisterAttached(
            "GridStroke", typeof(SolidColorBrush), typeof(AxisGridLineStyleBehavior), new PropertyMetadata(Brushes.Black, OnGridStrokeChanged));

        private static void OnGridStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is AxisCore axisBase))
            {
                throw new ArgumentException(@"unexpected object", nameof(d));
            }
            if (e.NewValue is SolidColorBrush color)
            {
                Style newStyle = new Style(typeof(Line));

                newStyle.Setters.Add(new Setter(Line.StrokeProperty, e.NewValue));
                newStyle.Setters.Add(new Setter(Line.StrokeThicknessProperty, 5.0d));
                newStyle.Setters.Add(new Setter(Line.StrokeDashArrayProperty, new DoubleCollection(new[] { 1d, 0d })));

                axisBase.MajorGridLineStyle = newStyle;
            }
        }
        public static void SetGridStroke(DependencyObject element, SolidColorBrush value)
        {
            element.SetValue(GridStrokeProperty,value);
        }

        public static SolidColorBrush GetGridStroke(DependencyObject element)
        {
            return (SolidColorBrush)element.GetValue(GridStrokeProperty);
        }
    }
}
