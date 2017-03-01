using System;
using System.Windows;
using System.Windows.Controls;
using CodeHighlighter;

namespace SciChart.Examples.Demo.Helpers
{
    public class CodeHighlighter : FrameworkElement
    {
        public static readonly DependencyProperty SourceLanguageProperty = DependencyProperty.RegisterAttached( "SourceLanguage", typeof (SourceLanguageType), typeof (CodeHighlighter), new PropertyMetadata(SourceLanguageType.CSharp, OnSourceLanguagePropertyChanged));

        public static SourceLanguageType GetSourceLanguage(DependencyObject o)
        {
            return (SourceLanguageType)o.GetValue(SourceCodeProperty);
        }

        public static void SetSourceLanguage(DependencyObject o, SourceLanguageType lang)
        {
            o.SetValue(SourceLanguageProperty, lang);
        }

        public static readonly DependencyProperty SourceCodeProperty = DependencyProperty.RegisterAttached("SourceCode", typeof(string), typeof(CodeHighlighter), new PropertyMetadata(string.Empty, OnSourceCodePropertyChanged));

        public static string GetSourceCode(DependencyObject o)
        {
            return (string)o.GetValue(SourceCodeProperty);
        }

        public static void SetSourceCode(DependencyObject o, string code)
        {
            o.SetValue(SourceCodeProperty, code);
        }

        private static void OnSourceLanguagePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var value = (SourceLanguageType) e.NewValue;

            if (!Enum.IsDefined(typeof (SourceLanguageType), value))
            {
                throw new ArgumentException("Invalid source language type.");
            }
        }

        private static void OnSourceCodePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var rtb = (RichTextBox) d;
            rtb.Highlight();
        }
    }
}