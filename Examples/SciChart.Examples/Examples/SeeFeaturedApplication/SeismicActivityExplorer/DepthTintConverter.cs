// *************************************************************************************
// SCICHART® Copyright SciChart Ltd. 2011-2026. All rights reserved.
//
// Web: http://www.scichart.com
//   Support: support@scichart.com
//   Sales:   sales@scichart.com
//
// DepthTintConverter.cs is part of the SCICHART® Examples. Permission is hereby granted
// to modify, create derivative works, distribute and publish any part of this source
// code whether for commercial, private or personal use.
//
// The SCICHART® examples are distributed in the hope that they will be useful, but
// without any warranty. It is provided "AS IS" without warranty of any kind, either
// expressed or implied.
// *************************************************************************************
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SciChart.Charting.Visuals.RenderableSeries;

namespace SciChart.Examples.Examples.SeeFeaturedApplication.SeismicActivityExplorer
{
    /// <summary>
    /// Converts an event's focal depth to a brush tinted with the same color the depth
    /// palette assigns to it: semi-transparent for the tooltip background, opaque for its
    /// border. Falls back to a neutral dark brush when no palette is active.
    /// </summary>
    public class DepthTintConverter : IValueConverter
    {
        private static readonly Brush FallbackFill = Frozen(Color.FromArgb(0xB4, 0x1F, 0x2B, 0x3F));
        private static readonly Brush FallbackBorder = Frozen(Color.FromArgb(0xFF, 0x3A, 0x4A, 0x60));

        /// <summary>
        /// The active depth palette; updated by the view whenever the user switches palettes.
        /// </summary>
        public HeatmapColorPalette Palette { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isBorder = "Border".Equals(parameter as string, StringComparison.OrdinalIgnoreCase);

            if (Palette == null || !(value is double))
            {
                return isBorder ? FallbackBorder : FallbackFill;
            }

            int argb = Palette.GetColor((double)value);
            var r = (byte)(argb >> 16);
            var g = (byte)(argb >> 8);
            var b = (byte)argb;

            return isBorder
                ? Frozen(Color.FromArgb(0xFF, r, g, b))
                : Frozen(Color.FromArgb(0xB4, r, g, b));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        private static Brush Frozen(Color color)
        {
            var brush = new SolidColorBrush(color);
            brush.Freeze();
            return brush;
        }
    }
}
