// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// RadarChartCustomizationLabelProviders.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
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
