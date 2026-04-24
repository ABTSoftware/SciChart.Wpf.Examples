// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// ExampleFeaturesFormattingConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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
    public class ExampleFeaturesFormattingConverter : TextFormattingConverterBase, IValueConverter
    {
        private IMainWindowViewModel _mainWindowViewModel;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            _mainWindowViewModel ??= ServiceLocator.Container.Resolve<IMainWindowViewModel>();

            if (_mainWindowViewModel.SearchText.IsNullOrEmpty())
            {
                return string.Empty;
            }

            var lines = ((IEnumerable<Features>)value).Select(x => FormatTextBase(x.ToString())).ToArray();
            var terms = _mainWindowViewModel.SearchText.Split(' ').Where(word => word != "").Select(x => x.ToLower()).ToArray();
            var result = new List<string>();

            for (int i = 0; i < lines.Length; i++)
            {
                result.Add(HighlightTermsBase(lines[i], terms));
            }

            return result.Any() ? string.Join(", ", result) : "[No Results]";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}