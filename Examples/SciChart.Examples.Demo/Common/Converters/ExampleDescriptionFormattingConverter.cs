
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Unity;
using SciChart.Core.Extensions;
using SciChart.UI.Bootstrap;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class ExampleDescriptionFormattingConverter : TextFormattingConverterBase, IValueConverter
    {
        private IMainWindowViewModel _mainWindowViewModel;

        public IMainWindowViewModel MainWindowViewModel
        {
            get { return _mainWindowViewModel ?? (_mainWindowViewModel = ServiceLocator.Container.Resolve<IMainWindowViewModel>()); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (MainWindowViewModel.SearchText.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var description = (string)value;
            var result = string.Empty;

            var terms = MainWindowViewModel.SearchText.Split(' ').Where(word => word != "").Select(x => x.ToLower()).ToArray();

            var lines = description.Split(new[] { ". " }, StringSplitOptions.None).ToArray();

            var sentences = new HashSet<string>();
            foreach (var term in terms)
            {
                var containsTerm = lines.Where(x => x != "" && x.ToLower().Contains(term));
                containsTerm.Take(2).ForEachDo(x => sentences.Add(x));
            }

            if (sentences.Any())
            {
                result = HighlightText(sentences.Select(x => x.Trim()).ToArray(), terms);
            }
            else
            {
                foreach (string sentence in lines.Take(2).Select(x => x.Trim()))
                {
                    result = result + (sentence + ". ");
                }
            }

            return result;
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