using System;
using SciChart.Charting.Visuals.Axes.LabelProviders;
using SciChart.Core.Extensions;

namespace SciChart_DigitalAnalyzerPerformanceDemo
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