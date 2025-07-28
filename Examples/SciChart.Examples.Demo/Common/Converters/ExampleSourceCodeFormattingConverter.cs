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
    public class ExampleSourceCodeFormattingConverter : TextFormattingConverterBase, IValueConverter
    {
        private IMainWindowViewModel _mainWindowViewModel;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _mainWindowViewModel ??= ServiceLocator.Container.Resolve<IMainWindowViewModel>();

            if (_mainWindowViewModel.SearchText.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var terms = _mainWindowViewModel.SearchText.Split(' ').Where(word => word != "").Select(x => x.ToLower()).ToArray();
            var codeFiles = (Dictionary<string, string>)value;

            var uiCodeFiles = codeFiles.Where(x => x.Key.EndsWith(".xaml"));
            var lines = new List<string>();

            foreach (var file in uiCodeFiles)
            {
                lines.AddRange(file.Value.Split(new[] { "\r\n" }, StringSplitOptions.None));
            }

            var toHighlight = new HashSet<string>();
            foreach (var term in terms)
            {
                var containsTerm = lines.Where(x => x != "" && x.ToLower().Contains(term));
                containsTerm.Take(2).Select(FormatTextBase).ForEachDo(x => toHighlight.Add(x));
            }

            string result;

            if (toHighlight.Any())
            {
                lines = toHighlight.Take(2).Select(x => x.Trim().Replace('<', ' ').Replace('>', ' ') + '.').ToList();
                result = HighlightText(lines, terms);
            }
            else
            {
                var sentences = lines.Take(2).Select(x => string.Format("... {0} ...", x.Trim().Replace('<', ' ').Replace('>', ' ').ToList()));
                result = string.Join("\n", sentences);
            }

            return string.IsNullOrEmpty(result) ? "[No Results]" : result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        private string HighlightText(List<string> lines, string[] terms)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                lines[i] = HighlightTermsBase(lines[i], terms);
            }

            var result = string.Format("... {0} ...", string.Join(" ... ", lines));
            return result;
        }
    }
}