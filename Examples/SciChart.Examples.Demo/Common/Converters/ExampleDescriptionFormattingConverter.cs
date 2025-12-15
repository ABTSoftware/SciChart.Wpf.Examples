
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using SciChart.Core.Extensions;
using SciChart.UI.Bootstrap;
using Unity;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class ExampleDescriptionFormattingConverter : TextFormattingConverterBase, IValueConverter
    {
        private IMainWindowViewModel _mainWindowViewModel;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _mainWindowViewModel ??= ServiceLocator.Container.Resolve<IMainWindowViewModel>(); 

            if (_mainWindowViewModel.SearchText.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var description = (string)value;
            var result = string.Empty;

            var terms = _mainWindowViewModel.SearchText.Split(' ').Where(word => word != "").Select(x => x.ToLower()).ToArray();
            var lines = description.Split(new[] { ". " }, StringSplitOptions.None).ToArray();
            
            var sentences = new HashSet<string>();

            foreach (var term in terms)
            {
                var containsTerm = lines.Where(x => x != "" && x.ToLower().Contains(term));
                containsTerm.Take(2).Select(FormatTextBase).ForEachDo(x => sentences.Add(x));
            }

            if (sentences.Any())
            {
                result = HighlightText(sentences.Select(x => x.Trim()).ToArray(), terms);
            }
            else
            {
                foreach (string sentence in lines.Take(2).Select(x => x.Trim()))
                {
                    result += (sentence + ". ");
                }
            }

            return string.IsNullOrEmpty(result) ? "[No Results]" : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string HighlightText(string[] lines, string[] terms)
        {
            var result = "";

            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] += '.';

                result += HighlightTermsBase(lines[i], terms);
            }
            return result;
        }
    }
}