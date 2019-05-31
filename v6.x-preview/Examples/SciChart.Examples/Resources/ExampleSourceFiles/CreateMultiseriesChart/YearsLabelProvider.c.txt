// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2018. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// YearsLabelProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System;
using SciChart.Charting.Visuals.Axes;
using SciChart.Charting.Visuals.Axes.LabelProviders;

namespace SciChart.Examples.Examples.CreateMultiseriesChart
{
    public class YearsLabelProvider : LabelProviderBase
    {
        private readonly string[] _xLabels = { "2000", "2010", "2014", "2050" };

        public override string FormatLabel(IComparable dataValue)
        {
            var i = Convert.ToInt32(dataValue);
            string result = "";
            if (i >= 0 && i < 4)
            {
                result = _xLabels[i];
            }
            return result;
        }

        public override string FormatCursorLabel(IComparable dataValue)
        {
            var i = Convert.ToInt32(dataValue);
            string result = "";
            if (i >= 0 && i < 4)
            {
                result = _xLabels[i];
            }
            else if (i < 0)
            {
                result = _xLabels[0];
            }
            else
            {
                result = _xLabels[3];
            }
            return result;
        }
    }
}