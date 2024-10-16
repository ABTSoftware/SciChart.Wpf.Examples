using System.Windows;
using System.Windows.Input;

namespace PerformanceBenchmark.Extensions
{
    public class TextBoxExtension
    {
        public static readonly DependencyProperty WatermarkProperty = DependencyProperty.RegisterAttached
            ("Watermark", typeof(string), typeof(TextBoxExtension), new FrameworkPropertyMetadata(string.Empty));

        public static string GetWatermark(DependencyObject obj)
        {
            return (string)obj.GetValue(WatermarkProperty);
        }

        public static void SetWatermark(DependencyObject obj, string value)
        {
            obj.SetValue(WatermarkProperty, value);
        }

        public static readonly DependencyProperty ButtonCommandProperty = DependencyProperty.RegisterAttached
            ("ButtonCommand", typeof(ICommand), typeof(TextBoxExtension), new PropertyMetadata(null));

        public static ICommand GetButtonCommand(DependencyObject d)
        {
            return (ICommand)d.GetValue(ButtonCommandProperty);
        }

        public static void SetButtonCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ButtonCommandProperty, value);
        }

        public static readonly DependencyProperty ButtonCommandParameterProperty = DependencyProperty.RegisterAttached
            ("ButtonCommandParameter", typeof(object), typeof(TextBoxExtension), new PropertyMetadata(null));

        public static object GetButtonCommandParameter(DependencyObject d)
        {
            return d.GetValue(ButtonCommandParameterProperty);
        }

        public static void SetButtonCommandParameter(DependencyObject obj, object value)
        {
            obj.SetValue(ButtonCommandParameterProperty, value);
        }

        public static readonly DependencyProperty ButtonContentProperty = DependencyProperty.RegisterAttached
            ("ButtonContent", typeof(object), typeof(TextBoxExtension), new PropertyMetadata("button"));

        public static object GetButtonContent(DependencyObject d)
        {
            return d.GetValue(ButtonContentProperty);
        }

        public static void SetButtonContent(DependencyObject obj, object value)
        {
            obj.SetValue(ButtonContentProperty, value);
        }
    }
}
