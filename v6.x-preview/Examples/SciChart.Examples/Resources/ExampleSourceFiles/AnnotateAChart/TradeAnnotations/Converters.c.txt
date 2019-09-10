using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            else if ((value is FibonacciExtensionAnnotationViewModel || value is FibonacciRetracementAnnotationViewModel) && (((string) parameter) == "Fill" || ((string) parameter) == "Pitchfork"))
            {
                return Visibility.Collapsed;
            }
            else if (value is ElliotWaveAnnotationViewModel && (((string)parameter) == "Fill" || ((string)parameter) == "Pitchfork"))
            {
                return Visibility.Collapsed;
            }
            else if (value is PitchforkAnnotationViewModel && (((string)parameter) == "FontSize" || ((string)parameter) == "Fill"))
            {
                return Visibility.Collapsed;
            }
            else if (value is HeadAndShouldersAnnotationViewModel && ((string)parameter) == "Pitchfork")
            {
                return Visibility.Collapsed;
            }
            else if (value is XabcdAnnotationViewModel && ((string)parameter) == "Pitchfork")
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
            double result;
            if (value != null && double.TryParse(value.ToString(), out result))
            {
                return value;
            }
            return 1d;
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
            var dataContext = value as ITradingAnnotationViewModel;
            var transparent = Brushes.Transparent;

            DataContext = dataContext;

            if (dataContext is ElliotWaveAnnotationViewModel || dataContext is FibonacciExtensionAnnotationViewModel || dataContext is FibonacciRetracementAnnotationViewModel)
            {
                return SetBrush(transparent, transparent, transparent, ((TradingAnnotationViewModel)dataContext).FontSize, (string)parameter);
            }
            else if (dataContext is BrushAnnotationViewModel)
            {
                return SetBrush(transparent, transparent, transparent, 0, (string)parameter);
            }
            else if (dataContext is HeadAndShouldersAnnotationViewModel)
            {
                var vm = (HeadAndShouldersAnnotationViewModel)dataContext;
                return SetBrush(vm.Fill, transparent, transparent, vm.FontSize, (string)parameter);
            }
            else if (dataContext is XabcdAnnotationViewModel)
            {
                var vm = (XabcdAnnotationViewModel)dataContext;
                return SetBrush(vm.Fill, transparent, transparent, vm.FontSize, (string)parameter);
            }
            else if (dataContext is PitchforkAnnotationViewModel)
            {
                var vm = (PitchforkAnnotationViewModel)dataContext;
                return SetBrush(transparent, vm.MiddleFill, vm.SidesFill, vm.FontSize, (string)parameter);
            }

            return null;
        }

        private object SetBrush(Brush fill, Brush middleFill, Brush sidesFill, double fontSize, string parameter)
        {
            object value = Brushes.Transparent;

            if ((string)parameter == "Fill")
            {
                value = fill;
            }
            else if ((string)parameter == "SidesFill")
            {
                value = sidesFill;
            }
            else if ((string)parameter == "MiddleFill")
            {
                value = middleFill;
            }
            else if ((string)parameter == "FontSize")
            {
                value = fontSize;
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var dataContext = DataContext as ITradingAnnotationViewModel;

            if (dataContext is HeadAndShouldersAnnotationViewModel && ((string)parameter == "Fill"))
            {
                ((HeadAndShouldersAnnotationViewModel)dataContext).Fill = (Brush)value;
            }
            else if (dataContext is XabcdAnnotationViewModel && ((string)parameter == "Fill"))
            {
                ((XabcdAnnotationViewModel)dataContext).Fill = (Brush)value;
            }
            else if (dataContext is PitchforkAnnotationViewModel)
            {
                if ((string)parameter == "SidesFill")
                {
                    ((PitchforkAnnotationViewModel)dataContext).SidesFill = (System.Windows.Media.Brush)value;
                }
                else if ((string)parameter == "MiddleFill")
                {
                    ((PitchforkAnnotationViewModel)dataContext).MiddleFill = (System.Windows.Media.Brush)value;
                }
            }

            if (!(dataContext is BrushAnnotationViewModel) && (string)parameter == "FontSize")
            {
                ((TradingAnnotationViewModel)dataContext).FontSize = (double)value;
            }

            return null;
        }
    }
}
