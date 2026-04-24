// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// SeriesInfoValueConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;
using SciChart.Charting.Model.ChartData;

namespace SciChart.Examples.Examples.PerformanceDemos2D.RacingTelemetryDashboard.Views
{
    /// <summary>
    /// Base for converters that resolve a <see cref="SeriesInfo"/> by matching
    /// <c>values[0]</c> (series name) against <c>values[1]</c> (SeriesInfo collection).
    /// </summary>
    public abstract class SeriesInfoConverterBase : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values == null || values.Length < 2) return NoMatch(anyItems: false);

            var name = values[0] as string;
            var infos = values[1] as IEnumerable;
            if (string.IsNullOrEmpty(name) || infos == null) return NoMatch(anyItems: false);

            bool any = false;
            foreach (var item in infos)
            {
                any = true;
                if (item is SeriesInfo info && info.SeriesName == name)
                    return FromMatch(info);
            }

            return NoMatch(anyItems: any);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        protected abstract object FromMatch(SeriesInfo info);
        protected abstract object NoMatch(bool anyItems);
    }

    /// <summary>
    /// Returns <see cref="SeriesInfo.FormattedYValue"/> on match; "NaN" when the collection
    /// is populated but no match; "—" before any data arrives.
    /// </summary>
    public class SeriesInfoValueConverter : SeriesInfoConverterBase
    {
        protected override object FromMatch(SeriesInfo info) => info.FormattedYValue;
        protected override object NoMatch(bool anyItems) => anyItems ? "NaN" : "—";
    }

}
