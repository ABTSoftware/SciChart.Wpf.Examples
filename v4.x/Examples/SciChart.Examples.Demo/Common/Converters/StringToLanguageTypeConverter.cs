using System;
using System.Windows.Data;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class StringToLanguageTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            const string codeExtension = "cs";
            const string markupExtension = "xaml";

            var fileName = value as string;

            if (fileName.Trim().EndsWith(codeExtension))
            {
                return CodeHighlighter.SourceLanguageType.CSharp;
            }

            if (fileName.Trim().EndsWith(markupExtension))
            {
                return CodeHighlighter.SourceLanguageType.Xaml;
            }

            throw new Exception("Unknown file extension - language type isn't recognizible.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}