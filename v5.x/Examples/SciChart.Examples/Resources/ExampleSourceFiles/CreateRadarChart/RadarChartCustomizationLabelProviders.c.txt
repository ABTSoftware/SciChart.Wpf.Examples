using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SciChart.Charting.Visuals.Axes.LabelProviders;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.CreateRadarChart
{
    public class RadarChartBillionDollarCategoryLabelProvider : LabelProviderBase
    {
        public override string FormatLabel(IComparable dataValue)
        {
            return string.Format("${0} B", Math.Round(dataValue.ToDouble(), 1));
        }

        public override string FormatCursorLabel(IComparable dataValue)
        {
            return FormatLabel(dataValue);
        }
    }

    public class RadarChartPercentCategoryLabelProvider : LabelProviderBase
    {
        public override string FormatLabel(IComparable dataValue)
        {
            return string.Format("{0}%", Math.Round(dataValue.ToDouble(), 1));
        }

        public override string FormatCursorLabel(IComparable dataValue)
        {
            return FormatLabel(dataValue);
        }
    }
}
