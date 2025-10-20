// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
//
// AgeAxisLabelProvider.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting.Visuals.Axes.LabelProviders;
using SciChart.Core.Extensions;
using System;

namespace SciChart.Examples.Examples.CreateMultiseriesChart.PopulationPyramidChart
{
    public class AgeRangeLabelProvider : NumericLabelProvider
    {
        private const double AgeLimit = 100;

        public override string FormatLabel(IComparable dataValue)
        {
            // Show ages in the format "10-14"
            var firstAgeValue = dataValue.ToDouble();
            if (firstAgeValue > AgeLimit)
                return string.Empty;

            if (firstAgeValue >= AgeLimit)
            {
                // Show "100+"
                return string.Format($"{{0:{ParentAxis.TextFormatting}}}+", AgeLimit);
            }

            var secondAgeValue = firstAgeValue + ParentAxis.MajorDelta.ToDouble() - 1;
            return string.Format($"{{0:{ParentAxis.TextFormatting}}}-{{1:{ParentAxis.TextFormatting}}}", firstAgeValue, secondAgeValue);
        }
    }
}