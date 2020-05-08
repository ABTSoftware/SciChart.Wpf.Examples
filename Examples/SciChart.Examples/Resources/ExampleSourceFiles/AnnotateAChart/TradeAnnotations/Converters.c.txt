using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using SciChart.Charting.DrawingTools.TradingAnnotations.ViewModels;

namespace SciChart.Examples.Examples.AnnotateAChart.TradeAnnotations
{
    public class AnnotationViewModelToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is BrushAnnotationViewModel && (((string)parameter) == "FontSize" || ((string)parameter) == "Fill" || ((string)parameter) == "Pitchfork"))
            {
                return Visibility.Collapsed;
            } 
            
            if ((value is FibonacciExtensionAnnotationViewModel || value is FibonacciRetracementAnnotationViewModel) && (((string) parameter) == "Fill" || ((string) parameter) == "Pitchfork"))
            {
                return Visibility.Collapsed;
            } 
            
            if (value is ElliotWaveAnnotationViewModel && (((string)parameter) == "Fill" || ((string)parameter) == "Pitchfork"))
            {
                return Visibility.Collapsed;
            } 
            
            if (value is PitchforkAnnotationViewModel && (((string)parameter) == "FontSize" || ((string)parameter) == "Fill"))
            {
                return Visibility.Collapsed;
            } 
            
            if (value is HeadAndShouldersAnnotationViewModel && ((string)parameter) == "Pitchfork")
            {
                return Visibility.Collapsed;
            } 
            
            if (value is XabcdAnnotationViewModel && ((string)parameter) == "Pitchfork")
            {
                return Visibility.Collapsed;
            }

            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class StrokeThicknessToEditPanelConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            return double.TryParse(value?.ToString(), out double result) ? result : 1d;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class DrawingToolViewModelToEditPanelOptionsConverter : IValueConverter
    {
        private object DataContext;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;

            var transparent = Brushes.Transparent;

            DataContext = value;

            if (DataContext is ElliotWaveAnnotationViewModel || DataContext is FibonacciExtensionAnnotationViewModel || DataContext is FibonacciRetracementAnnotationViewModel)
            {
                return SetValue(transparent, transparent, transparent, ((TradingAnnotationViewModel)DataContext).FontSize, (string)parameter);
            } 
            
            if (DataContext is BrushAnnotationViewModel)
            {
                return SetValue(transparent, transparent, transparent, 0, (string)parameter);
            } 
            
            if (DataContext is HeadAndShouldersAnnotationViewModel vm1)
            {
                return SetValue(vm1.Fill, transparent, transparent, vm1.FontSize, (string)parameter);
            }
            
            if (DataContext is XabcdAnnotationViewModel vm2)
            {
                return SetValue(vm2.Fill, transparent, transparent, vm2.FontSize, (string)parameter);
            }
            
            if (DataContext is PitchforkAnnotationViewModel vm3)
            {
                return SetValue(transparent, vm3.MiddleFill, vm3.SidesFill, vm3.FontSize, (string)parameter);
            }

            return null;
        }

        private object SetValue(Brush fill, Brush middleFill, Brush sidesFill, double fontSize, string parameter)
        {
            object value = Brushes.Transparent;

            if (parameter == "Fill")
            {
                value = fill;
            }
            else if (parameter == "SidesFill")
            {
                value = sidesFill;
            }
            else if (parameter == "MiddleFill")
            {
                value = middleFill;
            }
            else if (parameter == "FontSize")
            {
                value = fontSize;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return DataContext;

            if (DataContext is HeadAndShouldersAnnotationViewModel vm1 && ((string)parameter == "Fill"))
            {
                vm1.Fill = (Brush)value;
            }
            else if (DataContext is XabcdAnnotationViewModel vm2 && ((string)parameter == "Fill"))
            {
                vm2.Fill = (Brush)value;
            }
            else if (DataContext is TradingAnnotationViewModel vm3 && (string)parameter == "FontSize")
            {
                vm3.FontSize = (double)value;
            }
            else if (DataContext is PitchforkAnnotationViewModel vm4)
            {
                if ((string)parameter == "SidesFill")
                {
                    vm4.SidesFill = (Brush)value;
                }
                else if ((string)parameter == "MiddleFill")
                {
                    vm4.MiddleFill = (Brush)value;
                }
            }

            return DataContext;
        }
    }
}