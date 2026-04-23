// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// TimeLabelProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using SciChart.Charting.Visuals.Axes.LabelProviders;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.PerformanceDemos2D.DigitalAnalyzer.Common
{
    public class TimeLabelProvider : NumericLabelProvider
    {
        public override string FormatLabel(IComparable dataValue)
        {
            return FormatTimeLabel(dataValue.ToDouble());
        }

        public override string FormatCursorLabel(IComparable dataValue)
        {
            return FormatTimeLabel(dataValue.ToDouble());
        }

        public string FormatTimeLabel(double totalNanoseconds)
        {
            var ns = totalNanoseconds % 1000;
            var us = Math.Truncate(totalNanoseconds / 1000 % 1000);
            var ms = Math.Truncate(totalNanoseconds / 1000_000 % 1000);
            var s = Math.Truncate(totalNanoseconds / 1000_000_000 % 1000);

            if (ns > 0)
            {
                return $"{s:0}s : {ms:##0}ms : {us:##0}µs : {ns:##0.0}ns";
            }
            if (us > 0)
            {
                return $"{s:0}s : {ms:##0}ms : {us:##0}µs";
            }
            if (ms > 0)
            {
                return $"{s:0}s : {ms:##0}ms";
            }
            if (s >= 0)
            {
                return $"{s:0}s";
            }
            return string.Empty;
        }
    }
}