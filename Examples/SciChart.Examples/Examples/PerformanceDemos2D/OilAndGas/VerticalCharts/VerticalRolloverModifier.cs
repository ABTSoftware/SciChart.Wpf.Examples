// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2025. All rights reserved.
//  
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
// 
// VerticalRolloverModifier.cs is part of the SCICHART® Examples. Permission is hereby
// granted to modify, create derivative works, distribute and publish any part of this
// source code whether for commercial, private or personal use. 
// 
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied. 
// *************************************************************************************
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals.RenderableSeries;
using SciChart.Core.Extensions;

namespace SciChart.Examples.Examples.PerformanceDemos2D.OilAndGas.VerticalCharts
{
    public class VerticalRolloverModifier : RolloverModifier
    {
        private new const double TooltipOffset = 10;

        private IEnumerable<TooltipControl> GetTooltipLabels(IEnumerable<FrameworkElement> rolloverMarkers)
        {
            if (!rolloverMarkers.IsEmpty())
            {
                foreach (var marker in rolloverMarkers)
                {
                    MergeTooltipLabelFor(marker);
                }

                if (!_tooltipLabels.IsEmpty())
                {
                    foreach (var tooltipLabel in _tooltipLabels)
                    {
                        if (tooltipLabel is Panel panel)
                        {
                            foreach (var tooltip in panel.Children.OfType<TooltipControl>().ToList())
                            {
                                yield return tooltip;
                            }
                        }
                        else if (tooltipLabel is TooltipControl tooltip)
                        {
                            yield return tooltip;
                        }
                    }
                }
            }
        }

        protected override void UpdateTooltipLabels(IEnumerable<FrameworkElement> rolloverMarkers)
        {
            ClearTooltipLabels();

            if (!rolloverMarkers.IsEmpty())
            {
                var tooltipOverlay = new StackPanel();
                foreach (var tooltip in GetTooltipLabels(rolloverMarkers))
                {
                    (tooltip.Parent as Panel)?.SafeRemoveChild(tooltip);
                    tooltipOverlay.SafeAddChild(tooltip);
                }

                _tooltipLabels.Clear();
                _tooltipLabels.Add(tooltipOverlay);

                tooltipOverlay.MaxWidth = ModifierSurface.ActualWidth - TooltipOffset;
                tooltipOverlay.MeasureArrange();

                var rolloverCoord = Canvas.GetTop(rolloverMarkers.First());
                var x = ModifierSurface.ActualWidth / 2 - tooltipOverlay.ActualWidth / 2;
                var y = rolloverCoord + TooltipOffset;

                if (y + tooltipOverlay.ActualHeight > ModifierSurface.ActualHeight)
                    y = rolloverCoord - tooltipOverlay.ActualHeight - TooltipOffset;

                Canvas.SetLeft(tooltipOverlay, x);
                Canvas.SetTop(tooltipOverlay, y);

                ModifierSurface.Children.Add(tooltipOverlay);
            }
        }
    }
}