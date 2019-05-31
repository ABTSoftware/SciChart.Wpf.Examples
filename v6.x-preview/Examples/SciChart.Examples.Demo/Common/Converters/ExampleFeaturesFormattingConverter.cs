using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Unity;
using SciChart.UI.Bootstrap;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Demo.Common.Converters
{
    public class ExampleFeaturesFormattingConverter : TextFormattingConverterBase, IValueConverter
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
            var lines = ((IEnumerable<Features>)value).Select(x => x.ToString()).ToArray();
            var terms = MainWindowViewModel.SearchText.Split(' ').Where(word => word != "").Select(x => x.ToLower()).ToArray();

            var result = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                result.Add(HighlightTermsBase(lines[i], terms));
            }

            return string.Join(", ", result);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}