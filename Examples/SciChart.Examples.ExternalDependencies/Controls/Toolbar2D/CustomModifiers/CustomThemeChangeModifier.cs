// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web:     http://www.scichart.com
// Support: support@scichart.com
// Sales:   sales@scichart.com
//
// CustomThemeChangeModifier.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using SciChart.Charting;
using SciChart.Charting.ChartModifiers;
using SciChart.Charting.Visuals;

namespace SciChart.Examples.ExternalDependencies.Controls.Toolbar2D.CustomModifiers
{
    public class CustomThemeChangeModifier : ChartModifierBase
    {
        public string SelectedTheme
        {
            get
            {
                var theme = ThemeManager.GetTheme((SciChartSurface)ParentSurface);

                return string.IsNullOrEmpty(theme) ? ThemeManager.DefaultTheme : theme;
            }
            set
            {
                ThemeManager.SetTheme((SciChartSurface)ParentSurface, value);
            }
        }
    }
}