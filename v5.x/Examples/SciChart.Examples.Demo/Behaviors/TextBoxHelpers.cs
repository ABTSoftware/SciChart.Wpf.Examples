using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interactivity;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Demo.Behaviors
{    
    public class TextBoxHelpers
    {
        public static readonly DependencyProperty WatermarkContentProperty = DependencyProperty.RegisterAttached(
            "WatermarkContent", typeof (object), typeof (TextBoxHelpers), new PropertyMetadata(default(object), OnWatermarkPropertyChanged));       

        public static void SetWatermarkContent(DependencyObject element, object value)
        {
            element.SetValue(WatermarkContentProperty, value);
        }

        public static object GetWatermarkContent(DependencyObject element)
        {
            return (object) element.GetValue(WatermarkContentProperty);
        }

        public static readonly DependencyProperty WatermarkContentTemplateProperty = DependencyProperty.RegisterAttached(
            "WatermarkContentTemplate", typeof(DataTemplate), typeof(TextBoxHelpers), new PropertyMetadata(default(DataTemplate), OnWatermarkPropertyChanged));        

        public static void SetWatermarkContentTemplate(DependencyObject element, DataTemplate value)
        {
            element.SetValue(WatermarkContentTemplateProperty, value);
        }

        public static DataTemplate GetWatermarkContentTemplate(DependencyObject element)
        {
            return (DataTemplate) element.GetValue(WatermarkContentTemplateProperty);
        }

        private static void OnWatermarkPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
//            var tb = d as TextBox;
//            if (tb == null) return;
//
//            if (e.NewValue == null || string.IsNullOrEmpty(e.NewValue as string)) return;
//
//            tb.GotFocus += (s, arg) =>
//            {
//                SetIsShowingWatermark(tb, true);                
//            };
//
//            tb.LostFocus += (s, arg) =>
//            {
//                SetIsShowingWatermark(tb, true);
//            };
        }

        public static readonly DependencyProperty IsShowingWatermarkProperty = DependencyProperty.RegisterAttached(
            "IsShowingWatermark", typeof (bool), typeof (TextBoxHelpers), new PropertyMetadata(default(bool)));

        public static void SetIsShowingWatermark(DependencyObject element, bool value)
        {
            element.SetValue(IsShowingWatermarkProperty, value);
        }

        public static bool GetIsShowingWatermark(DependencyObject element)
        {
            return (bool) element.GetValue(IsShowingWatermarkProperty);
        }

        public static readonly DependencyProperty LoseFocusOnEscapeProperty = DependencyProperty.RegisterAttached(
            "LoseFocusOnEscape", typeof (bool), typeof (TextBoxHelpers), new PropertyMetadata(default(bool), OnLoseFocusOnEscapeChanged));        

        public static void SetLoseFocusOnEscape(DependencyObject element, bool value)
        {
            element.SetValue(LoseFocusOnEscapeProperty, value);
        }

        public static bool GetLoseFocusOnEscape(DependencyObject element)
        {
            return (bool) element.GetValue(LoseFocusOnEscapeProperty);
        }

        private static void OnLoseFocusOnEscapeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            TextBox tb = d as TextBox;
            if (tb == null || (bool)e.NewValue == false) return;

            tb.KeyDown += (s, arg) => 
            {
                if (arg.Key == Key.Escape)
                {
                    var parent = tb.Parent as UIElement;
                    if (parent != null)
                    {
                        var parentControl = parent.FindVisualParent<Control>();
                        if (parentControl != null)
                        {
                            parentControl.Focus();
                        }                        
                    }
                } 
            };
        }

        public static readonly DependencyProperty ClearButtonProperty = DependencyProperty.RegisterAttached(
            "ClearButton", typeof (bool), typeof (TextBoxHelpers), new PropertyMetadata(default(bool)));

        public static void SetClearButton(DependencyObject element, bool value)
        {
            element.SetValue(ClearButtonProperty, value);
        }

        public static bool GetClearButton(DependencyObject element)
        {
            return (bool) element.GetValue(ClearButtonProperty);
        }

        public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.RegisterAttached(
            "CornerRadius", typeof(CornerRadius), typeof(TextBoxHelpers), new PropertyMetadata(default(CornerRadius)));

        public static void SetCornerRadius(DependencyObject element, CornerRadius value)
        {
            element.SetValue(CornerRadiusProperty, value);
        }

        public static CornerRadius GetCornerRadius(DependencyObject element)
        {
            return (CornerRadius)element.GetValue(CornerRadiusProperty);
        }
    }
}
